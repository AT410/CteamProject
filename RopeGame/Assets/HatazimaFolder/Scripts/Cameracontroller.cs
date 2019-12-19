using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontroller : MonoBehaviour
{
    public GameObject player;
    int move = 0;

    void LateUpdate()
    {
        move = 0;

        if(player.transform.position.y > 17 || player.transform.position.y < -5.2f)
        {
            move++;
        }

        if(player.transform.position.x > 19.9f || player.transform.position.x < -19.9f)
        {
            move += 2;
        }

        if(move == 0)
        {
            Vector3 playerPos = player.transform.position;
            transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        }
        else if(move == 1)
        {
            Vector3 playerPos = player.transform.position;
            transform.position = new Vector3(playerPos.x, transform.position.y, transform.position.z);
        }
        else if(move == 2)
        {
            Vector3 playerPos = player.transform.position;
            transform.position = new Vector3(transform.position.x, playerPos.y, transform.position.z);
        }

    }
}
