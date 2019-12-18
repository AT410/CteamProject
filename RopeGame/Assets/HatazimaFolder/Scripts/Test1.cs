using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    float speed = 0.1f; // 移動の速さ
    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        var velocity = new Vector3(h, v) * speed;
        transform.localPosition += velocity;
    }
}
