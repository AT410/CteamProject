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

    //Spawn variables
    Vector2 spawnpoint = Vector2.zero;

    //Effects
    private string DodgeEffect = "DodgeEffect";

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        theCam = Camera.main;
        activeMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Functions
        MovePlayer();
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

    //Respawn function
    public void getSpawnpoint(Vector2 point)
    {
        spawnpoint = point;
    }

    public void respawn()
    {
        transform.position = spawnpoint;
    }
}
    
