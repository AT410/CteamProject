using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximity : MonoBehaviour
{
    private GameObject m_player;//プレイヤーのgameobjectを格納
    private float m_shotTime = 0.01f;//弾の発射速度
    private float m_speed = 10f;//弾の速度
    private float m_currentTime = 0;//弾の発射カウント
    private ObjectPool m_pool;//ObjectPoolを格納
    const string PLAYERNAME = "Player";//ヒエルラキー上のプレイヤーの名前

    private void Start()
    {
        m_player = GameObject.Find(PLAYERNAME);
        m_pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();//objectpoolのscriptを取得
    }
    /// <summary>
    /// 弾発射関数
    /// </summary>
    public void Shot()
    {
        //指定時間経つごとに弾を発射する
        m_currentTime += Time.deltaTime;

        if (m_shotTime < m_currentTime)
        {
            m_currentTime = 0;
            var bullet = PositionSetting();
            //弾に速度をつけて発射
            bullet.GetComponent<Rigidbody2D>().velocity = (m_player.transform.position - bullet.transform.position).normalized * m_speed;
        }
    }
    /// <summary>
    /// 初期位置設定関数及び弾のアクティブ化（生成）
    /// </summary>
    /// <returns></returns>
    private GameObject PositionSetting()
    {
        var bullet = m_pool.GetObject();
        bullet.transform.position = this.gameObject.transform.position;
        return bullet;
    }
}
