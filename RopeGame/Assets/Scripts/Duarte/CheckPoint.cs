using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }
    //プレイヤーはチェックポイントに入ったら、そのチェックポイントは
    //When the player enter to a new check point, the new check point will become to the new respawn point
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.getSpawnPoint(transform.position);
        }
    }
}
