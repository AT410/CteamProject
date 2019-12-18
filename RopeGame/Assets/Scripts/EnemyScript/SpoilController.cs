using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoilController : EnemyBase
{
    private float m_currentTime = 0;
    private float m_stopTime = 1f;
    // Update is called once per frame
    //  private string state;
    void Update()
    {
        Debug.Log(state);
        if (state != "Caught")
        {
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

        if (m_distance < playerDistance + 1)
        {
            state = "Sleep";
        }
        if (m_distance < playerDistance && m_distance < MAXDISTANCE)
        {
            state = "Away";

        }
        else if (m_distance > MAXDISTANCE)
        {
            state = "RandamMove";
        }


    }
    /// <summary>
    /// 状態チェック
    /// </summary>
    private void StateCheck()
    {
        switch (state)
        {
            case ("Away"):
                m_moveX *= -1;
                m_moveY *= -1;

                break;
            case ("Sleep"):
                m_moveX = 0;
                m_moveY = 0;
                break;
            case ("RandamMove"):
                RandamMove();
                break;
            case ("Caught"):
                PlayerChase();
                m_moveX *= -0.8f;
                m_moveY *= -0.8f;
                break;
        }
        transform.Translate(m_moveX, m_moveY, 0, Space.World);
    }
}
