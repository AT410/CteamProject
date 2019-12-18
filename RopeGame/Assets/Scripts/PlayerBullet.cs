using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public static PlayerBullet instance;
    public float speed = 7.5f;
    public Rigidbody2D theRB;
    public Transform bulletTrans;

    public GameObject impactEffect;

    public int damageToGive = 50;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        Instantiate(impactEffect, transform.position, transform.rotation);
        if (other.tag == "Scenary")
        {
            PlayerController.instance.playerTrans.position = PlayerBullet.instance.bulletTrans.position;
        }
        if (other.tag == "Item")
        {
            ItemController.instance.itemTrans.position = new Vector3(PlayerController.instance.playerTrans.position.x, PlayerController.instance.playerTrans.position.y - 1, PlayerController.instance.playerTrans.position.z);
        }

        if (other.tag == "Enemy")
        {
          //  other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }
    }

    private void OnBecomeInvisible()
    {
        Destroy(gameObject);
    }
}
