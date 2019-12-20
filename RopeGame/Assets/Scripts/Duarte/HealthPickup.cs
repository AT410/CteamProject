using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;
    private string HealthPickUpEffect = "HealthPickUpEffect";
    public float waitToBeCollected;

    void Start()
    {
        
    }
    void Update()
    {
        if(waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerHealthController.instance.HealPlayer(healAmount);
            //EffectManager.instance.playInPlace(transform.position, HealthPickUpEffect);
            Destroy(gameObject);
        }
    }
}
