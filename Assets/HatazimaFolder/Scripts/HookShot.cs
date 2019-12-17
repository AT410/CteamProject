using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShot : MonoBehaviour
{
    Vector3 targ;           //レイが当たったオブジェクトの座標
    RaycastHit hit;         //レイが当たったオブジェクトの様々な情報を格納する変数
    float dist;             //プレイヤーとオブジェクトの距離を格納する変数
    float time = 0;         //プレイヤーが移動する時間を測る変数
    float moveTime = 0.8f; //プレイヤーが移動する時間
    bool move = false;      //プレイヤーが移動しているか判断する変数

    public GameObject rope;
    LineRenderer line;

    void Start()
    {
        //LineRendererコンポーネントの追加
        line = rope.AddComponent<LineRenderer>();
        //加算合成をマテリアルに適用
        line.material = new Material(Shader.Find("Mobile/Particles/Additive"));
        //線の頂点数
        line.positionCount = 2;
        //線の太さ
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        //線の色
        line.startColor = new Color(255, 180, 0, 255);
        line.endColor = new Color(255, 180, 0, 255);
        rope.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) ObjectDesignation();
        //if (!move)
        //{
        //    if (Input.GetMouseButtonDown(0)) ObjectDesignation();
        //}
        if (move && Input.mouseScrollDelta.y != 0)
        {
            time += Time.deltaTime;
            
            //指定した座標まで移動させる
            transform.position = Vector3.MoveTowards(transform.position, targ, dist * Time.deltaTime);
            
            //線の座標指定
            line.SetPosition(0, transform.position);
            line.SetPosition(1, targ);

            if (time >= dist * 0.12f) //moveTimeの時間だけ移動したら止まる
            {
                rope.SetActive(false);
                move = false;
                time = 0;
            }
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
            Debug.Log(dist);
            move = true;
            rope.SetActive(true);
            //線の座標指定
            line.SetPosition(0, transform.position);
            line.SetPosition(1, targ);
            time = 0;
        }
    }
}
