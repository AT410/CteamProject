using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    private string ResurrectionEffect = "ResurrectionEffect";
    private string FallInHoleEffect = "FallInHoleEffect";

    void Start()
    {  
    }

    void Update()
    {  
    }

    //プレイヤーは穴落としに入ったら、ダメージを受けて、チェックポイントまで移動される。他のものが穴落としに入ったら、破壊される 
    //If the player enter to the hole, he will be hurt and will be respawned in the check point
    private void OnTriggerEnter2D(Collider2D other)
    {
        EffectManager.instance.playInPlace(transform.position, FallInHoleEffect);

        if (other.tag == "Player")
        {
            PlayerHealthController.instance.currentHealth--;
            PlayerController player = other.GetComponent<PlayerController>();
            player.Respawn();
            EffectManager.instance.playInPlace(PlayerController.instance.playerTrans.position, ResurrectionEffect);
            return;
        }

        Destroy(other.gameObject);
    }
}
