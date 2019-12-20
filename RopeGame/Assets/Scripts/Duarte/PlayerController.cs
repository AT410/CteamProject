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

    //Movement variables
    public float moveSpeed;
    private Vector2 moveInput;
    private float activeMoveSpeed;

    //Dash variables
    public float dashSpeed = 8f;
    public float dashLength = .5f;
    public float dashCooldown = 1f;
    public float dashInvincibility = .5f;
    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;
    
    //Attack variables
    [SerializeField]
    private float attackTime = 0.3f;
    private bool attacking = false;
    private float attackTimeCounter;
    public bool dodge = false;
    
   
    //Spawn variables
    Vector2 spawnpoint = Vector2.zero;

    public bool isattacking = false;

    //Effects
    private string DodgeEffect = "DodgeEffect";

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isattacking = false;
        theCam = Camera.main;
        activeMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0.0f))
            return;

        //Functions
            if (!isattacking)
        {
            MovePlayer();
        }
        Attack();
        Dodge();
    }

    //Movement function
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
    //Attack function
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


    //Dodge function
    public void Dodge()
    {
        if (Input.GetButtonDown("Dodge"))
        {
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
                EffectManager.instance.playInPlace(transform.position, DodgeEffect);
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

    //Get check point
    public void getSpawnpoint(Vector2 point)
    {
        spawnpoint = point;
    }
    //Respawn function
    public void respawn()
    {
        transform.position = spawnpoint;
    }
}

