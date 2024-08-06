using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCoin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        
         if (collider.CompareTag("coin")){
            Destroy(collider.gameObject);
        }

    }
   
}
