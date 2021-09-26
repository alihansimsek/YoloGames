using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public delegate void CollectableAction();
    public static event CollectableAction collectableHitEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(collectableHitEvent != null)
                collectableHitEvent();
            Destroy(gameObject);
        }
    }
}
