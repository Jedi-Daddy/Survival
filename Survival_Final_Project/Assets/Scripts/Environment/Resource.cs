using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceDrop
{
    public ItemData item;
    public int quantityPerHit = 1;
}

public class Resource : MonoBehaviour
{
    [Header("Everytime")]
    public ResourceDrop guaranteedDrop;

    [Header("Random")]
    public List<ResourceDrop> secondaryDropOptions;

    public int capacity;
    public GameObject hitParticle;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        
        for (int i = 0; i < guaranteedDrop.quantityPerHit; i++)
        {
            if (capacity <= 0) break;
            Inventory.instance.AddItem(guaranteedDrop.item);
            capacity--;
        }

        
        if (secondaryDropOptions.Count > 0 && capacity > 0)
        {
            int index = Random.Range(0, secondaryDropOptions.Count);
            var randomDrop = secondaryDropOptions[index];

            for (int i = 0; i < randomDrop.quantityPerHit; i++)
            {
                if (capacity <= 0) break;
                Inventory.instance.AddItem(randomDrop.item);
                capacity--;
            }
        }

        
        Destroy(Instantiate(hitParticle, hitPoint, Quaternion.LookRotation(hitNormal, Vector3.up)), 1.0f);

        
        if (capacity <= 0)
            Destroy(gameObject);
    }
}
