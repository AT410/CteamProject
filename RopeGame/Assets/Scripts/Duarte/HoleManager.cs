using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    private string ResurrectionEffect = "ResurrectionEffect";
    private string FallInHoleEffect = "FallInHoleEffect";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        EffectManager.instance.playInPlace(transform.position, FallInHoleEffect);
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.currentHealth--;
            PlayerController player = other.GetComponent<PlayerController>();
            player.respawn();
            EffectManager.instance.playInPlace(PlayerController.instance.playerTrans.position, ResurrectionEffect);
            return;
        }
        Destroy(other.gameObject);
    }
}
