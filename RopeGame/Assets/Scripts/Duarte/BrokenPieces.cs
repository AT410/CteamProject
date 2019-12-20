using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    //レベルオブジェクトのパーツの変数Variables of the level object's parts
    private Vector3 moveDirection;
    public float moveSpeed = 3f;
    public float deceleration = 5f;
    public float fadeSpeed = 2.5f;
    public float lifetime = 3f;
    public SpriteRenderer theSR;

    void Start()
    {
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }

    //破壊されたレベルオブジェクトがだんだんなくなるために //To disappear the broken parts of the level's objects progressively
    void Update()
    {
        transform.position += moveDirection * Time.deltaTime;

        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);

        lifetime -= Time.deltaTime;

        if(lifetime < 0)
        {
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, Mathf.MoveTowards(theSR.color.a, 0f, fadeSpeed * Time.deltaTime));

            if(theSR.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
