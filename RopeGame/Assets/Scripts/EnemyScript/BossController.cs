using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyBase
{
    private int tiredCount = 0;
    private float SERIOUS_DISTANCE = 2.0f;
    [SerializeField]
    private EnemyShot enemyShot;

    Animator anim;
    private string EnemyDeathEffect = "EnemyDeathEffect";

    void Update()
    {
        Debug.Log(state);
        if (state != "Caught")
        {
            if (m_distance < MAXDISTANCE)
            {
                enemyShot.Shot();
            }
            ActionPolicy();
        }
        if (state == "Die")
        {
            anim.Play("Boss_Tangled");
            EffectManager.instance.playInPlace(transform.position, EnemyDeathEffect);
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
        if (m_distance < playerDistance - SERIOUS_DISTANCE*tiredCount)
        {
           // m_speed = 3.0f;
            tiredCount += 1;
            m_player.GetComponent<newHookShot>().RopeCut();
        }
        else
        {
            m_speed = m_defalutSpeed;
        }
    }
}