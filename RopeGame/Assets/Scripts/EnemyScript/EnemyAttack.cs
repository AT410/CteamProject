using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float m_currentTime = 0;//攻撃カウント
    private float m_shotTime = 0.5f;//攻撃速度
    const string PLAYERNAME = "Player";//ヒエルラキー上のプレイヤーの名前
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            m_currentTime += Time.deltaTime;
            if (m_shotTime < m_currentTime)
            {
                //攻撃エフェクトはここに
                PlayerHealthController.instance.DamagePlayer();
                m_currentTime = 0;
            }
        }
    }
}
