using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newHookShot : MonoBehaviour
{
    public GameObject star;
    Vector3 targ;           //レイが当たったオブジェクトの座標
    RaycastHit hit;         //レイが当たったオブジェクトの様々な情報を格納する変数
    float dist;             //プレイヤーとオブジェクトの距離を格納する変数
    float nowDist;          //プレイヤーとオブジェクトの距離を格納する変数
    bool move = false;      //プレイヤーが移動しているか判断する変数
    float hori = 0;
    float vert = 0;
    int nextStickAngle = 0;
    int stickAngle = 3;
    Vector3 oldMousePos = new Vector3(242.5f, 136.5f, 0);

    public GameObject rope;
    public LineRenderer line;

    void Start()
    {
        AudioManager.Instance.PlayBGM("Title");  //タイトルBGM再生
        line.positionCount = 2;                                                 //線の頂点数
        line.startWidth = 0.1f;                                                 //線の太さ
        line.endWidth = 0.1f;                                                   //〃
        rope.SetActive(false);                                                  //ラインを使うまで隠しておく
    }


    void Update()
    {
        hori = Input.GetAxis("Horizontal2");
        vert = Input.GetAxis("Vertical2");

        if (Input.GetKeyDown("joystick button 5")) ObjectDesignation();
        //if (Input.GetMouseButtonDown(0)) ObjectDesignation();

        if (move)
        {
            //右回り
            if (stickAngle == 3 && nextStickAngle == 0 && vert <= -0.7) PullIn();
            else if (stickAngle == 0 && nextStickAngle == 1 && hori <= -0.7) PullIn();
            else if (stickAngle == 1 && nextStickAngle == 2 && vert >= 0.7) PullIn();
            else if (stickAngle == 2 && nextStickAngle == 3 && hori >= 0.7) PullIn();

            if (vert <= -0.7) stickAngle = 0;
            else if (hori <= -0.7) stickAngle = 1;
            else if (vert >= 0.7) stickAngle = 2;
            else if (hori >= 0.7) stickAngle = 3;

            //左回り
            //if (stickAngle == 3 && nextStickAngle == 0 && hori >= 0.7) PullIn();
            //else if (stickAngle == 0 && nextStickAngle == 1 && vert >= 0.7) PullIn();
            //else if (stickAngle == 1 && nextStickAngle == 2 && hori <= -0.7) PullIn();
            //else if (stickAngle == 2 && nextStickAngle == 3 && vert <= -0.7) PullIn();

            //if (hori >= 0.7) stickAngle = 0;
            //else if (vert >= 0.7) stickAngle = 1;
            //else if (hori <= -0.7) stickAngle = 2;
            //else if (vert <= -0.7) stickAngle = 3;

            //パソコン用
            if (Input.mouseScrollDelta.y != 0) PullIn();
            //右回り
            if (nextStickAngle == 3 && oldMousePos.y + 1 < Input.mousePosition.y) PullIn();
            else if (nextStickAngle == 0 && oldMousePos.x + 1 < Input.mousePosition.x) PullIn();
            else if (nextStickAngle == 1 && oldMousePos.y - 1 > Input.mousePosition.y) PullIn();
            else if (nextStickAngle == 2 && oldMousePos.x - 1 > Input.mousePosition.x) PullIn();
        }

        //テスト用
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0, 0, -2);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0, 0, 2);
            }
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
                targ = hit.transform.position;
                dist = Vector3.Distance(transform.position, targ); //現在位置と選択したオブジェクトまでの距離を測る
                move = true;
                rope.SetActive(true);
                //線の座標指定
                line.SetPosition(0, transform.position);
                line.SetPosition(1, targ);
            }
        }
    }

    void PullIn()
    {
        nowDist = Vector3.Distance(transform.position, targ); //現在位置と選択したオブジェクトまでの距離を測る

        //指定した座標まで移動させる
        transform.position = Vector3.MoveTowards(transform.position, targ, dist * 3 * Time.deltaTime);

        //線の座標指定
        line.SetPosition(0, transform.position);
        line.SetPosition(1, targ);

        if (0 >= nowDist - 1.5) //オブジェクトまでの距離に対応した時間だけ移動したら止まる
        {
            rope.SetActive(false);
            move = false;
        }

        nextStickAngle++;
        if (nextStickAngle == 4) nextStickAngle = 0;

    }
}
