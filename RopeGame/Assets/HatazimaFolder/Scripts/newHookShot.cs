using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newHookShot : MonoBehaviour
{
    public GameObject star;   //このオブジェクトの方向にレイを飛ばす
    public GameObject rope;   //ロープ(LineRenderer)を使うためのオブジェクト
    public GameObject healthController;
    public LineRenderer line; //ロープ
    GameObject targ;          //レイが当たったオブジェクトの座標
    RaycastHit hit;           //レイが当たったオブジェクトの様々な情報を格納する変数
    float dist;               //プレイヤーとオブジェクトの距離を格納する変数
    float nowDist;            //プレイヤーとオブジェクトの距離を格納する変数
    float hori, vert;         //コントローラーのx軸y軸を格納する変数
    float stickAngle = 0.65f; //コントローラースティックの傾き
    float rollPower = 25;     //巻き取る力
    int rollSe = 0;           //巻き取る際のSEの鳴る頻度
    bool move = false;        //プレイヤーが移動しているか判断する変数
    bool horiVert = false;    //スティックの傾きが前回x軸y軸のどちらに傾いたかの変数


    void Start()
    {
        AudioManager.Instance.PlayBGM("Main"); //タイトルBGM再生
        line.positionCount = 2;                //線の頂点数
        line.startWidth = 0.1f;                //線の太さ
        line.endWidth = 0.1f;                  //〃
        rope.SetActive(false);                 //ラインを使うまで隠しておく
    }
    
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Valley"))
        {
            transform.position = new Vector3(0, 0, -1);
            healthController.GetComponent<PlayerHealthController>().DamagePlayer();
        }
    }

    void Update()
    {
        if(!move)
        {
            //縄を投げる
            if (Input.GetKeyDown("joystick button 5")) ObjectDesignation();
            if (Input.GetMouseButtonDown(0)) ObjectDesignation();
        }
        if (move)
        {
            //コントローラーのx軸y軸を取得
            hori = Input.GetAxis("Horizontal2");
            vert = Input.GetAxis("Vertical2");

            //コントローラー用
            if (horiVert && (hori >= stickAngle || hori <= -stickAngle)) Roll();
            else if (!horiVert && (vert >= stickAngle || vert <= -stickAngle)) Roll();
            
            //パソコン用
            if (Input.mouseScrollDelta.y != 0) Roll();

            //縄を切る
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
            //GetComponent<Test1>().enabled = false;
            if (targ != null && targ.CompareTag("Enemy")) targ.GetComponent<EnemyBase>().SleepState();
            targ = hit.collider.gameObject;
            if (!targ.CompareTag("Bullet") && !targ.CompareTag("Untagged"))
            {
                dist = Vector3.Distance(transform.position, targ.transform.position); //現在位置と選択したオブジェクトまでの距離を測る
                move = true;
                rope.SetActive(true);
                if (targ.CompareTag("Enemy")) targ.GetComponent<EnemyBase>().EscapeState();
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
            case "Ground": transform.position = Vector3.MoveTowards(transform.position, targ.transform.position, rollPower * Time.deltaTime); break;
        }

        if (10 <= nowDist) //敵に逃げられたらロープを切る
        {
            AudioManager.Instance.PlaySE("RopeCut");  //ロープが切れるSE再生
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
                GameManager.GetGameManager().QUIUpdate(targ.GetComponent<EnemyBase>().GetEnemyType());
                targ.SetActive(false); //仮置き
            }
            //GetComponent<Test1>().enabled = true;
        }

        rollSe++;
        if (rollSe == 4)
        {
            AudioManager.Instance.PlaySE("Roll");  //4回に一回巻き取るSEを再生
            rollSe = 0;
        }

        if (horiVert) horiVert = false;
        else if (!horiVert) horiVert = true;
    }

    public void RopeCut()
    {
        AudioManager.Instance.PlaySE("RopeCut");  //ロープが切れるSE再生
        rope.SetActive(false);
        move = false;
        if (targ.CompareTag("Enemy"))
        {
            targ.GetComponent<EnemyBase>().SleepState();
        }
        //GetComponent<Test1>().enabled = true;
    }

}
