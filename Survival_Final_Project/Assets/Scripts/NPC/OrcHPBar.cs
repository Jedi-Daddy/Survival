using UnityEngine;
using UnityEngine.UI;

public class OrcHPBar : MonoBehaviour
{
    public Image healthFill;
    public Image healthAll;
    public Transform target;  

    void Update()
    {
        
        if (target != null)
        {
            transform.position = target.position + Vector3.up * 2.5f;
            transform.LookAt(Camera.main.transform);  
        }
    }

    
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthFill != null)
        {
            healthFill.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        }
    }
}
