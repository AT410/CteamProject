using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShot3D : MonoBehaviour
{
    Vector3 targ;           //レイが当たったオブジェクトの座標
    RaycastHit hit;         //レイが当たったオブジェクトの様々な情報を格納する変数
    float dist;             //プレイヤーとオブジェクトの距離を格納する変数
    float nowDist;          //プレイヤーとオブジェクトの距離を格納する変数
    bool move = false;      //プレイヤーが移動しているか判断する変数
    float hori = 0;
    float vert = 0;
    //bool horiVert = false;
    int nextStickAngle = 0;
    int stickAngle = 3;

    public GameObject rope;
    LineRenderer line;

    void Start()
    {
        AudioManager.Instance.PlayBGM("Title");  //タイトルBGM再生
        line = rope.AddComponent<LineRenderer>();                               //LineRendererコンポーネントの追加
        line.material = new Material(Shader.Find("Mobile/Particles/Additive")); //加算合成をマテリアルに適用
        line.positionCount = 2;                                                 //線の頂点数
        line.startWidth = 0.1f;                                                 //線の太さ
        line.endWidth = 0.1f;                                                   //〃
        line.startColor = new Color(255, 180, 0, 255);                          //線の色
        line.endColor = new Color(255, 180, 0, 255);                            //〃
        rope.SetActive(false);                                                  //ラインを使うまで隠しておく
    }


    void Update()
    {
        hori = Input.GetAxis("Horizontal2");
        vert = Input.GetAxis("Vertical2");

        if (Input.GetMouseButtonDown(0)) ObjectDesignation();

        if (move)
        {
            //コントローラ用
            //右回りと左回りどちらでもOK
            //if (horiVert && (hori >= 0.8 || hori <= -0.8)) PullIn();
            //else if (!horiVert && (vert >= 0.8 || vert <= -0.8)) PullIn();
            
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
        AudioManager.Instance.PlaySE("Hook");  //フックSE再生
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.up * 0.5f)); //レイを正面に飛ばす
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

        if (0 >= nowDist - 1.2) //オブジェクトまでの距離に対応した時間だけ移動したら止まる
        {
            rope.SetActive(false);
            move = false;
        }

        //if (horiVert) horiVert = false;
        //else if (!horiVert) horiVert = true;

        nextStickAngle++;
        if (nextStickAngle == 4) nextStickAngle = 0;
    }
}
