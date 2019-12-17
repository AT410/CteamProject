﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : EnemyBase
{
    private float m_currentTime = 0;
    private float m_stopTime = 1f;
    [SerializeField]
    private EnemyProximity enemyProximity;
    // Update is called once per frame
    //  private string state;
    void Update()
    {
        if (state == "Sleep")
        {
            m_currentTime += Time.deltaTime;
            if (m_stopTime < m_currentTime)
            {
                m_currentTime = 0;
                state = "Randam";
            }
        }
        else
        {
            ActionPolicy();
        }
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
      //  enemyProximity.Shot();
        if (m_distance < playerDistance && m_distance < MAXDISTANCE)
        {
            state = "Away";

        }
        else if (m_distance > playerDistance && m_distance < MAXDISTANCE)
        {
            state = "Approach";
        }
        else
        {
            state = "RandamMove";
        }
        StateCheck();

    }
    /// <summary>
    /// 状態チェック
    /// </summary>
    private void StateCheck()
    {
        switch (state)
        {
            case ("Approch"):
             //   enemyProximity.Shot();
                break;
            case ("Away"):
                m_moveX *= -1;
                m_moveY *= -1;
            //    enemyProximity.Shot();
                break;
            case ("Sleep"):
                m_moveX = 0;
                m_moveY = 0;
              //  enemyProximity.Shot();
                break;
            case ("RandamMove"):
                RandamMove();
                break;
        }
        transform.Translate(m_moveX, m_moveY, 0, Space.World);
    }
}