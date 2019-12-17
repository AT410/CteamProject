using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShot : MonoBehaviour
{
    Vector3 targ;           //レイが当たったオブジェクト
    RaycastHit hit;         //レイが当たったオブジェクトの様々な情報を格納する変数
    float dist;             //プレイヤーとオブジェクトの距離を格納する変数
    float time = 0;         //プレイヤーが移動する時間を測る変数
    float moveTime = 0.85f; //プレイヤーが移動する時間
    bool move = false;      //プレイヤーが移動しているか判断する変数
    
    void Update()
    {
        if (!move)
        {
            if (Input.GetKeyDown(KeyCode.Return)) ObjectDesignation();
        }
        if (move)
        {
            time += Time.deltaTime;
            
            if(time >= moveTime) //moveTimeの時間だけ移動したら止まる
            {
                move = false;
                time = 0;
            }

            //指定した座標まで移動させる
            transform.position = Vector3.MoveTowards(transform.position, targ, dist * Time.deltaTime);
        }

        //テスト用
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0, 0, -1);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0, 0, 1);
            }
        }

    }
    
    void ObjectDesignation()
    {
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.up * 0.5f));
        if (Physics.Raycast(ray, out hit, 10.0f))
        {
            targ = hit.transform.position;
            dist = Vector3.Distance(transform.position, targ); //現在位置と選択したオブジェクトまでの距離を測る
            move = true;
        }
    }
}
