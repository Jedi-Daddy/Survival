﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerNeeds : MonoBehaviour, IDamagable
{
    public Need health;
    public Need hunger;
    public Need thirst;
    public Need sleep;

    public float noHungerHealthDecay;
    public float noThirstHealthDecay;

    public UnityEvent onTakeDamage;

    public static PlayerNeeds instance;
    private SurvivalWinCondition survivalManager;

    void Awake ()
    {
        instance = this;
        survivalManager = FindObjectOfType<SurvivalWinCondition>();

        if (survivalManager == null)
        {
            Debug.LogError("SurvivalWinCondition not found on the scene!");
        }
    }

    void Start ()
    {
        health.curValue = health.startValue;
        hunger.curValue = hunger.startValue;
        thirst.curValue = thirst.startValue;
        sleep.curValue = sleep.startValue;
    }

    void Update ()
    {
        hunger.Subtract(hunger.decayRate * Time.deltaTime);
        thirst.Subtract(thirst.decayRate * Time.deltaTime);
        sleep.Add(sleep.regenRate * Time.deltaTime);

        if(hunger.curValue == 0.0f)
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        if(thirst.curValue == 0.0f)
            health.Subtract(noThirstHealthDecay * Time.deltaTime);

        if(health.curValue == 0.0f)
        {
            Die();
        }

        health.uiBar.fillAmount = health.GetPercentage();
        hunger.uiBar.fillAmount = hunger.GetPercentage();
        thirst.uiBar.fillAmount = thirst.GetPercentage();
        sleep.uiBar.fillAmount = sleep.GetPercentage();
    }

    public void Heal (float amount)
    {
        health.Add(amount);
    }

    public void Eat (float amount)
    {
        hunger.Add(amount);
    }

    public void Drink (float amount)
    {
        thirst.Add(amount);
    }

    public void Sleep (float amount)
    {
        sleep.Subtract(amount);
    }

    public void TakePhysicalDamage (int amount)
    {
        health.Subtract(amount);
        onTakeDamage?.Invoke();
        Debug.Log($"Player took {amount} damage. Current health: {health.curValue}");

        if (health.curValue <= 0.0f)
        {
            Die();
        }
    }

    public void Die ()
    {
        Debug.Log("Player is dead");
        if (survivalManager != null)
        {
            survivalManager.PlayerDied();
        }
    }
}

[System.Serializable]
public class Need
{
    [HideInInspector]
    public float curValue;
    public float maxValue;
    public float startValue;
    public float regenRate;
    public float decayRate;
    public Image uiBar;

    public void Add (float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract (float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPercentage ()
    {
        return curValue / maxValue;
    }
}

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}