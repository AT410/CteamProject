using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Rigidbody2D theRB;
    private Camera theCam;
    public SpriteRenderer bodySR;
    public Animator anim;
    public Transform playerTrans;

    //動き変数Movement variables
    public float moveSpeed;
    private Vector2 moveInput;
    private float activeMoveSpeed;

    //回避変数 Dodge variables
    public float dashSpeed = 8f;
    public float dashLength = .5f;
    public float dashCooldown = 1f;
    public float dashInvincibility = .5f;
    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;

    //動き変数 Attack variables
    [SerializeField]
    private float attackTime = 0.3f;
    private float attackTimeCounter;
    private bool attacking = false;
    public bool isattacking = false;

    //スキル変数 Skill variables
    public bool dodge = false;


    //スポーン変数Spawn variables
    Vector2 spawnpoint = Vector2.zero;

    //エフェクト変数 Effects
    private string DodgeEffect = "DodgeEffect";

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isattacking = false;
        theCam = Camera.main;
        activeMoveSpeed = moveSpeed;
    }

    void Update()
    {
        if(!isattacking)
        {
            MovePlayer();
        }
        Attack();
        Dodge();
    }

    //動き関数 Movement function
    public void MovePlayer()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        theRB.velocity = moveInput * activeMoveSpeed;

        anim.SetFloat("moveX", theRB.velocity.x);
        anim.SetFloat("moveY", theRB.velocity.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            anim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }

    }

    //攻撃関数 Attack function
    public void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            attackTimeCounter = attackTime;
            isattacking = true;
            attacking = true;
            theRB.velocity = Vector2.zero;
            anim.SetBool("attacking", true);
        }

        if (attackTimeCounter > 0)
        {
            attackTimeCounter -= Time.deltaTime;
        }

        if (attackTimeCounter <= 0)
        {
            attacking = false;
            isattacking = false;
            anim.SetBool("attacking", false);
        }
    }


    //スキル関数（回避） Skill function (Dodge)
    public void Dodge()
    {
        if (Input.GetButtonDown("Dodge"))
        {
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
                //EffectManager.instance.playInPlace(transform.position, DodgeEffect);
                PlayerHealthController.instance.MakeInvincible(dashInvincibility);
            }
            dodge = true;
        }
        else
        {
            dodge = false;
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    //チェックポイント関数 Check point function
    public void getSpawnPoint(Vector2 point)
    {
        spawnpoint = point;
    }
    //リスポーン関数 Respawn function
    public void Respawn()
    {
        transform.position = spawnpoint;
    }
}

