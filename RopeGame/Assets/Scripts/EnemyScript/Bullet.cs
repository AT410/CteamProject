using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*弾の操作script
  *製作者　篠﨑*/
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float m_deleteTime = 2.0f;//弾の消滅時間

    // Update is called once per frame
    void Update()
    {
        m_deleteTime -= Time.deltaTime;
        if (m_deleteTime < 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }

        gameObject.SetActive(false);
    }
    /// <summary>
    /// 再利用をするため、リセットを行う。
    /// </summary>
    private void OnEnable()
    {
        m_deleteTime = 2.0f;
    }
}
