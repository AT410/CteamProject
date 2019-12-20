using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveF : MonoBehaviour
{
    public GameObject XMK;
    // Start is called before the first frame update
    void Start()
    {
        XMK.SetActive(Test.Instance().ClearFlag);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
