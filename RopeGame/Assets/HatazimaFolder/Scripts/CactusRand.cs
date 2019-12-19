using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CactusRand : MonoBehaviour
{
    System.Random rand = new System.Random(Environment.TickCount);
    public GameObject[] cactus;
    int type;
    float time = 0, goTime = 1f;

    void Start()
    {

    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > goTime)
        {
            goTime = rand.Next(2, 10);
            type = rand.Next(0, 2);
            Instantiate(cactus[type], new Vector3(-10, -2.2f, 0), Quaternion.identity);
            time = 0;
        }
    }
}
