using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Executioner行動ルーチン
  *製作者　篠﨑*/
public class ExecutionerController : EnemyBase
{
    [SerializeField]
    private EnemyShot enemyShot;    //弾を格納する変数

    void Update()
    {
        if (state != "Caught"&&state != "Die")
        {
            if (m_distance < MAXDISTANCE)
            {
                enemyShot.Shot();
            }
            if (state == "Sleep")
            {
                m_currentTime += Time.deltaTime;
                if (m_stopTime < m_currentTime)
                {
                    m_currentTime = 0;
                    state = "RandamMove";
                }
            }
            else
            {
                ActionPolicy();
            }
        }
        StateCheck();

    }
    /// <summary>
    /// 行動方針を決定する
    /// </summary>
    private void ActionPolicy()
    {
        //距離測定関数
        DistancePlayer();
        //追跡関数
        PlayerChase();
        if (m_distance < playerDistance+1 )
        {
            state = "Sleep";
        }
        if (m_distance < playerDistance && m_distance < MAXDISTANCE)
        {
            state = "Away";      

        }
        else if (m_distance > playerDistance && m_distance < MAXDISTANCE)
        {
            state = "Approach";
        }
        else if (m_distance > MAXDISTANCE)
        {
            state = "RandamMove";
        }
    }
}
