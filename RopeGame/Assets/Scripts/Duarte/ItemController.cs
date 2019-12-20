using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public static ItemController instance;
    public Transform itemTrans;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
    }
}
