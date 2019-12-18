using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 0"))
        {
            Debug.Log("□");
        }
        if (Input.GetKeyDown("joystick button 1"))
        {
            Debug.Log("×");
        }
        if (Input.GetKeyDown("joystick button 2"))
        {
            Debug.Log("○");
        }
        if (Input.GetKeyDown("joystick button 3"))
        {
            Debug.Log("△");
        }
        if (Input.GetKeyDown("joystick button 4"))
        {
            Debug.Log("L1");
        }
        if (Input.GetKeyDown("joystick button 5"))
        {
            Debug.Log("R1");
        }
        if (Input.GetKeyDown("joystick button 6"))
        {
            Debug.Log("L2");
        }
        if (Input.GetKeyDown("joystick button 7"))
        {
            Debug.Log("R2");
        }
        if (Input.GetKeyDown("joystick button 8"))
        {
            Debug.Log("button8");
        }
        if (Input.GetKeyDown("joystick button 9"))
        {
            Debug.Log("button9");
        }
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        if ((hori != 0) || (vert != 0))
        {
            Debug.Log("stick:" + hori + "," + vert);
        }
        float hori2 = Input.GetAxis("Horizontal2");
        float vert2 = Input.GetAxis("Vertical2");
        if ((hori2 != 0) || (vert2 != 0))
        {
            Debug.Log("stick2:" + hori2 + "," + vert2);
        }
        //   float hori2 = Input.GetAxis("3rdaxis");
        //   float hori2 = Input.GetAxis("3rdaxis");
        // float vert2 = Input.GetAxis("6rdaxis");
    }
}
