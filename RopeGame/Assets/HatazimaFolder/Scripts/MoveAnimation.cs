using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAnimation : MonoBehaviour
{
    Animator animator;
    Vector3 oldPos;
    Vector3 movementAmount;
    int flameCount = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        oldPos = transform.position;
    }

    private void FixedUpdate()
    {
        flameCount++;
        if(flameCount == 15)
        {
            movementAmount = transform.position - oldPos;

            if(Math.Abs(movementAmount.x) >= Math.Abs(movementAmount.y))
            {
                if(movementAmount.x > 0) animator.SetInteger("Action", 3);
                else animator.SetInteger("Action", 1);
            }
            else
            {
                if (movementAmount.y > 0) animator.SetInteger("Action", 2);
                else animator.SetInteger("Action", 0);
            }
            oldPos = transform.position;
            flameCount = 0;
        }
    }
}
