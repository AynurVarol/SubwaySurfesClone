using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    
    void Update()
    {
        //coinin dönmesi için
        transform.Rotate( 100 * Time.deltaTime, 0, 0);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.numberOfCoins += 1;
            Destroy(gameObject);
        }
    }
}
