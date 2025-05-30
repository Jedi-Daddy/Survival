﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIType
{
    Passive,
    Scared,
    Aggressive
}

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Fleeing
}

public class NPC : MonoBehaviour, IDamagable
{
    public NPCData data;

    [Header("Stats")]
    public int maxHealth = 100;
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;

    [Header("AI")]
    public AIType aiType;
    public AIState aiState;
    public float detectDistance;
    public float safeDistance;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;

    [HideInInspector]
    public NavMeshAgent agent;
    private Animator anim;
    private SkinnedMeshRenderer[] meshRenderers;

    private OrcHPBar orcHPBar;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        orcHPBar = GetComponentInChildren<OrcHPBar>();
        if (orcHPBar == null)
        {
            Debug.LogError("OrcHPBar not found in NPC prefab.");
        }
        else
        {
            orcHPBar.target = transform;
            Debug.Log("HP Bar successfully linked to NPC.");
        }
    }


    void Start()
    {
        health = maxHealth;  
        SetState(AIState.Wandering);
    }

    void Update ()
    {
        playerDistance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

        anim.SetBool("Moving", aiState != AIState.Idle);

        switch(aiState)
        {
            case AIState.Idle: PassiveUpdate(); break;
            case AIState.Wandering: PassiveUpdate(); break;
            case AIState.Attacking: AttackingUpdate(); break;
            case AIState.Fleeing: FleeingUpdate(); break;
        }
    }

    void PassiveUpdate ()
    {
        if(aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if(aiType == AIType.Aggressive && playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);
        }
        else if(aiType == AIType.Scared && playerDistance < detectDistance)
        {
            SetState(AIState.Fleeing);
            agent.SetDestination(GetFleeLocation());
        }
    }

    void AttackingUpdate()
    {
        if (playerDistance > attackDistance)
        {
            agent.isStopped = false;
            if (PlayerController.instance != null)
            {
                agent.SetDestination(PlayerController.instance.transform.position);
            }
        }
        else
        {
            agent.isStopped = true;

            if (PlayerController.instance != null)
            {
                IDamagable player = PlayerController.instance.GetComponent<IDamagable>();
                if (player != null)
                {
                    if (Time.time - lastAttackTime > attackRate)
                    {
                        lastAttackTime = Time.time;
                        player.TakePhysicalDamage(damage);
                        anim.SetTrigger("Attack");
                        Debug.Log("Player attacked by NPC: " + damage + " damage.");
                    }
                }
                else
                {
                    Debug.LogError("Player does not implement IDamagable.");
                }
            }
            else
            {
                Debug.LogError("PlayerController instance is null.");
            }
        }
    }

    void FleeingUpdate ()
    {
        if(playerDistance < safeDistance && agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(GetFleeLocation());
        }
        else if(playerDistance > safeDistance)
        {
            SetState(AIState.Wandering);
        }
    }

    void SetState (AIState newState)
    {
        aiState = newState;

        switch(aiState)
        {
            case AIState.Idle:
            {
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            }
            case AIState.Wandering:
            {
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            }
            case AIState.Attacking:
            {
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
            }
            case AIState.Fleeing:
            {
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
            }
        }
    }

    void WanderToNewLocation ()
    {
        if(aiState != AIState.Idle)
            return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation ()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;

        while(Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

            i++;

            if(i == 30)
                break;
        }

        return hit.position;
    }

    Vector3 GetFleeLocation ()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, safeDistance, NavMesh.AllAreas);

        int i = 0;

        while(GetDestinationAngle(hit.position) > 90 || playerDistance < safeDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, safeDistance, NavMesh.AllAreas);

            i++;

            if(i == 30)
                break;
        }

        return hit.position;
    }

    float GetDestinationAngle (Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - PlayerController.instance.transform.position, transform.position + targetPos);
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health -= damageAmount;

        if (orcHPBar != null)
        {
            orcHPBar.UpdateHealth(health, maxHealth);
        }

        if (health <= 0)
            Die();

        StartCoroutine(DamageFlash());

        if (aiType == AIType.Passive)
            SetState(AIState.Fleeing);
    }

    void Die()
    {
        if (orcHPBar != null)
        {
            Destroy(orcHPBar.gameObject);
        }

        for (int x = 0; x < dropOnDeath.Length; x++)
        {
            Instantiate(dropOnDeath[x].dropPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    IEnumerator DamageFlash ()
    {
        for(int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f);

        for(int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = Color.white;
    }
}