﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Spoli行動ルーチン
  *製作者　篠﨑*/
public class SpoilController : EnemyBase
{
    private float m_currentTime = 0;
    private float m_stopTime = 1f;
    [SerializeField]
    private EnemyShot enemyShot;
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
        if (Mathf.Round((float)m_distance) == playerDistance || Mathf.Round((float)m_distance) == playerDistance + 1)
        {
            state = "sleep";
        }
        else if (m_distance < playerDistance && m_distance < MAXDISTANCE)
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
                enemyShot.Shot();
                break;
            case ("Away"):
                m_moveX *= -1;
                m_moveY *= -1;
                enemyShot.Shot();
                break;
            case ("Sleep"):
                m_moveX = 0;
                m_moveY = 0;
                break;
            case ("RandamMove"):
                RandamMove();
                break;
        }
        transform.Translate(m_moveX, m_moveY, 0, Space.World);
    }

}