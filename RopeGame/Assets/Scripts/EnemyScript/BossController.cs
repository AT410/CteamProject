using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyBase
{
    private int m_tiredCount = 0;
    private float SERIOUS_DISTANCE = 2.0f;
    private float m_cutTime = 1.0f;
    private float m_cutCount = 0;
    [SerializeField]
    private EnemyShot enemyShot;
    private newHookShot newhookshot;
    private void Start()
    {
        base.Start();
        newhookshot = m_player.GetComponent<newHookShot>();
    }
    void Update()
    {
        Debug.Log(state);
        if (state != "Caught" && state != "Die")
        {
            if (m_distance < MAXDISTANCE)
            {
                enemyShot.Shot();
            }
            ActionPolicy();
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
        else if (m_distance > playerDistance && m_distance < MAXDISTANCE)
        {
            state = "Approach";
        }
        else if (m_distance > MAXDISTANCE)
        {
            state = "Sleep";
        }
    }
    public override void Resistance()
    {
        if (m_distance < playerDistance - SERIOUS_DISTANCE*m_tiredCount)
        {
            m_cutCount += Time.deltaTime;
            if (m_cutTime < m_cutCount)
            {
                m_speed = 3.0f;
                m_tiredCount += 1;
                newhookshot.RopeCut();
                m_cutCount = 0;
            }
        }
        else
        {
            m_cutCount = 0;
            m_speed = m_defalutSpeed;
        }
    }
}