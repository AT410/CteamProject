﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*敵AI基底行動Script
  *製作者　篠﨑*/
public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    protected float m_speed = 0.05f;//移動速度
    protected float m_rad;//ラジアン変数
    protected float m_moveX;//移動方向代入変数x
    protected float m_moveY;//移動方向代入変数y
    protected float m_destinationX;//ランダム移動目的地x
    protected float m_destinationY;//ランダム移動目的地y
    protected int m_tmp;//sqrtを使う前に入れる
    protected double m_distance;//プレイヤーとの距離を代入
    protected GameObject m_player;//プレイヤー格納変数
    protected string state = "Randam";
    const string PLAYER_NAME = "Player";//ヒエルラキー上のプレイヤー名
    const string PLAYER_SHOT = "Player_Shot";
    [SerializeField]
    protected float playerDistance = 8.0f;//プレイヤーとの開ける距離
    protected const float MAXDISTANCE = 15.0f;//最大検知範囲
    protected const float MOVEMENT_RANGE = 2f;//最大目的地移動範囲
   

    // Start is called before the first frame update
    protected void Start()
    {
        m_player = GameObject.Find(PLAYER_NAME);
        DestinationDecision();
    }
    /// <summary>
    /// 衝突判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains(PLAYER_SHOT))
        {
            //捕獲エフェクト等はここに
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 敵追跡関数
    /// </summary>
    /// <param name="player"></param>
    protected void PlayerChase()
    {
        m_rad = Mathf.Atan2(m_player.transform.position.y - transform.position.y,
                            m_player.transform.position.x - transform.position.x);
        m_moveX = m_speed * Mathf.Cos(m_rad);
        m_moveY = m_speed * Mathf.Sin(m_rad);
    }
    /// <summary>
    /// プレイヤーとの距離を求める
    /// </summary>
    protected void DistancePlayer()
    {
        //三平方の定理を用いて
        m_tmp = (int)((transform.position.x - m_player.transform.position.x) * (transform.position.x - m_player.transform.position.x) + (transform.position.y - m_player.transform.position.y) * (transform.position.y - m_player.transform.position.y));
        //プレイヤーとの距離を測定
        m_distance = System.Math.Sqrt(m_tmp);
    }
    /// <summary>
    /// ランダム移動目的地決定関数
    /// </summary>
    protected void DestinationDecision()
    {
        state = "Sleep";
        m_destinationX = Random.Range(-MOVEMENT_RANGE + transform.position.x, MOVEMENT_RANGE + transform.position.x);
        m_destinationY = Random.Range(-MOVEMENT_RANGE + transform.position.y, MOVEMENT_RANGE + transform.position.y);
    }
    protected void　RandamMove()
    {
        if ((int)m_destinationY == (int)transform.position.y&&(int)m_destinationX == (int)transform.position.x)
        {
            DestinationDecision();
        }
        m_rad = Mathf.Atan2(m_destinationY - transform.position.y,
                            m_destinationX - transform.position.x);
        m_moveX = m_speed * Mathf.Cos(m_rad);
        m_moveY = m_speed * Mathf.Sin(m_rad);
    }


}