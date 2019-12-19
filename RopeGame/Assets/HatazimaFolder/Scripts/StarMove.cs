using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMove : MonoBehaviour
{
    public GameObject player;
    float speed = 0.15f; // 移動の速さ
    bool canSee = false;

    void Update()
    {
        if (canSee)
        {
            var h = Input.GetAxis("Horizontal2");
            var v = Input.GetAxis("Vertical2");
            var velocity = new Vector3(h, v) * speed;
            transform.localPosition += velocity;
        }
        if(!canSee)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 40* Time.deltaTime);
        }
        canSee = false;
    }

    void OnWillRenderObject()
    {
        if (Camera.current.name == "Main Camera") canSee = true;
    }
}
