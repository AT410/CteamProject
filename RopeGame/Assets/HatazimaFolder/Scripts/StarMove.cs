using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMove : MonoBehaviour
{
    float speed = 0.05f; // 移動の速さ
    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal2");
        var v = Input.GetAxis("Vertical2");
        var velocity = new Vector3(h, v) * speed;
        transform.localPosition += velocity;
    }
}
