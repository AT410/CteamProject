using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newHookShot : MonoBehaviour
{
    public GameObject star;   //このオブジェクトの方向にレイを飛ばす
    public GameObject rope;   //ロープ(LineRenderer)を使うためのオブジェクト
    public LineRenderer line; //ロープ
    GameObject targ;          //レイが当たったオブジェクトの座標
    RaycastHit hit;           //レイが当たったオブジェクトの様々な情報を格納する変数
    float dist;               //プレイヤーとオブジェクトの距離を格納する変数
    float nowDist;            //プレイヤーとオブジェクトの距離を格納する変数
    float hori, vert;         //コントローラーのHorizontal&Verticalを格納する変数
    float stickAngle = 0.65f; //コントローラースティックの傾き
    float rollPower = 20;     //巻き取る力
    bool move = false;        //プレイヤーが移動しているか判断する変数
    bool horiVert = false;    //スティックの傾きが前回x軸y軸のどちらに傾いたかの変数


    void Start()
    {
        AudioManager.Instance.PlayBGM("Main");  //タイトルBGM再生
        line.positionCount = 2;                                                 //線の頂点数
        line.startWidth = 0.1f;                                                 //線の太さ
        line.endWidth = 0.1f;                                                   //〃
        rope.SetActive(false);                                                  //ラインを使うまで隠しておく
    }
    
    void Update()
    {
        hori = Input.GetAxis("Horizontal2");
        vert = Input.GetAxis("Vertical2");
        
        if(!move)
        {
            if (Input.GetKeyDown("joystick button 5")) ObjectDesignation();
            if (Input.GetMouseButtonDown(0)) ObjectDesignation();
        }
        if (move)
        {
            //コントローラ用
            if (horiVert && (hori >= stickAngle || hori <= -stickAngle)) Roll();
            else if (!horiVert && (vert >= stickAngle || vert <= -stickAngle)) Roll();
            
            //パソコン用
            if (Input.mouseScrollDelta.y != 0) Roll();

            if (Input.GetKeyDown("joystick button 1") || Input.GetMouseButtonDown(1)) RopeCut();

            //線の座標指定
            line.SetPosition(0, transform.position);
            line.SetPosition(1, targ.transform.position);
        }
    }

    /// <summary>
    /// レイを正面に飛ばし距離を測ったあと、それに対応した長さのLine(ロープ)を作り
    /// 手繰り寄せれるようにするメソッド
    /// </summary>
    void ObjectDesignation()
    {
        Vector3 vec = (star.transform.position - transform.position).normalized;
        
        AudioManager.Instance.PlaySE("Hook");  //フックSE再生
        
        Ray2D ray = new Ray2D(transform.position, vec);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10f);
        Debug.DrawRay(transform.position, vec * 10, Color.red, 3);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject != gameObject)
            {
                //GetComponent<Test1>().enabled = false;
                if (targ != null && targ.CompareTag("Enemy")) targ.GetComponent<EnemyBase>().SleepState();
                targ = hit.collider.gameObject;
                dist = Vector3.Distance(transform.position, targ.transform.position); //現在位置と選択したオブジェクトまでの距離を測る
                move = true;
                rope.SetActive(true);
                if(targ.CompareTag("Enemy")) targ.GetComponent<EnemyBase>().EscapeState();
                //線の座標指定
                line.SetPosition(0, transform.position);
                line.SetPosition(1, targ.transform.position);
            }
        }
    }

    void Roll()
    {
        
        nowDist = Vector3.Distance(transform.position, targ.transform.position); //現在位置と選択したオブジェクトまでの距離を測る

        //指定したオブジェクトを引き寄せるor指定したオブジェクトまで移動する
        switch (targ.tag)
        {
            case "Enemy": targ.transform.position = Vector3.MoveTowards(targ.transform.position, transform.position, rollPower * Time.deltaTime); break;
            case "Item": targ.transform.position = Vector3.MoveTowards(targ.transform.position, transform.position, rollPower * Time.deltaTime); break;
            default: transform.position = Vector3.MoveTowards(transform.position, targ.transform.position, rollPower * Time.deltaTime); break;
        }

        if (10 <= nowDist) //敵に逃げられたら止まる
        {
            rope.SetActive(false);
            move = false;
            if (targ.CompareTag("Enemy")) targ.GetComponent<EnemyBase>().SleepState();
            //GetComponent<Test1>().enabled = true;
        }

        if (0 >= nowDist - 1.5) //オブジェクトまでの距離に対応した時間だけ移動したら止まる
        {
            rope.SetActive(false);
            move = false;
            if (targ.CompareTag("Enemy"))
            {
                targ.GetComponent<EnemyBase>().SleepState();
               // GameManager.GetGameManager().QUIUpdate(targ.GetComponent<EnemyBase>().GetEnemyType());
                targ.SetActive(false); //仮置き
            }
            //GetComponent<Test1>().enabled = true;
        }

        if (horiVert)
        {
            horiVert = false;
            AudioManager.Instance.PlaySE("Roll");  //巻き取るSE再生
        }
        else if (!horiVert) horiVert = true;
    }

    public void RopeCut()
    {
        AudioManager.Instance.PlaySE("RopeCut");  //フックSE再生
        rope.SetActive(false);
        move = false;
        targ.GetComponent<EnemyBase>().SleepState();
        //GetComponent<Test1>().enabled = true;
    }

}
