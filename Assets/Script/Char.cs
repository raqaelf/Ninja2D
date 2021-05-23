using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public delegate void DeadEventHandler();

public class Char : Character
{
    private static Char instance;

    public event DeadEventHandler Dead;

    [SerializeField]
    private Stat healthStat;

    public static Char Instance
    { 
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Char>();
            }
            return instance;
        } 
    
    }

    // private bool attack;

    // private bool slide;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    /** private bool isGrounded;

    private bool jump;

    private bool jumpAttack; **/

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    private bool immortal = false;

    [SerializeField]
    private float immortalTime;

    private SpriteRenderer spriteRenderer;

    public Rigidbody2D myRigiBody { get; set; }

    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    public override bool IsDead
    {
        get
        {
            if(healthStat.CurrentVal <= 0)
            {
                OnDead();
            }
            return healthStat.CurrentVal <= 0;
        }
    }

    private Vector2 startPos;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        myRigiBody = GetComponent<Rigidbody2D>();   
        healthStat.Initialize();
    }

    void Update()
    {
        if(!TakingDamage && !IsDead)
        {
            if(transform.position.y <= -14f)
            {
                Death();
            }
            HandleInput();
        }
       
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");
            OnGround = IsGrounded();
            Debug.Log(horizontal);
            movement(horizontal);
            Flip(horizontal);
            // HandleAttacks();
            HandleLayers();
            // ResetValues();
        }

    }

    public void OnDead()
    {
        if(Dead != null)
        {
            Dead();
        }
    }

    public void movement(float horizontal)
    {
        // if(myRigidBody.velocity.y < 0)
        // {
        //     myAnimator.SetBool("land",true);
        // }

        // if(!myAnimator.GetBool("slide") &&!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack")&&(isGrounded || airControl))
        // {
        //     myRigidBody.velocity = new Vector2(horizontal * speed, myRigidBody.velocity.y);
        // }

        // if(isGrounded && jump)
        // {
        //     isGrounded = false;
        //     myRigidBody.AddForce(new Vector2(0, jumpForce));
        //     myAnimator.SetTrigger("jump");
        // }

        // if(slide && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        // {
        //     myAnimator.SetBool("slide",true);
        // }
        // else if(!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        // {
        //     myAnimator.SetBool("slide",false);
        // }

        // myAnimator.SetFloat("speed",Mathf.Abs (horizontal));

        if (myRigiBody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }        

        if (!Attack && !Slide && (OnGround || airControl))
        {
            myRigiBody.velocity = new Vector2(horizontal * speed, myRigiBody.velocity.y);
        }

        if (Jump && myRigiBody.velocity.y == 0)
        {
            myRigiBody.AddForce(new Vector2(0,jumpForce));
        }

        MyAnimator.SetFloat("speed",Mathf.Abs(horizontal));
    }

    // private void HandleAttacks()
    // {
    //     if(attack && isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
    //     {
    //        myAnimator.SetTrigger("serang");
    //        myRigidBody.velocity = Vector2.zero;
    //     }

    //     if (jumpAttack && !isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
    //     {
    //         myAnimator.SetBool("jumpAttack",true);
    //     }

    //     if(!jumpAttack && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
    //     {
    //         myAnimator.SetBool("jumpAttack",false);
    //     }
    // }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            MyAnimator.SetTrigger("jump");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // attack = true;
            // jumpAttack = true;
            MyAnimator.SetTrigger("attack");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            MyAnimator.SetTrigger("slide");
            // slide = true;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            MyAnimator.SetTrigger("throw");
            ThrowKunai(0);
        }
    }

    public void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ChangeDirection();
        }
    }

    // private void ResetValues(){
    //     attack = false;
    //     slide = false; 
    //     jump = false;
    //     jumpAttack = false;
    // }

    public bool IsGrounded()
    {
        if(myRigiBody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position,groundRadius,whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].gameObject != gameObject)
                    {
                        // myAnimator.ResetTrigger("jump");
                        // myAnimator.SetBool("land",false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if(!OnGround)
        {
            MyAnimator.SetLayerWeight(1,1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1,0);
        }
    }

    public override void ThrowKunai(int value)
    {
        if (!OnGround && value == 1 || OnGround && value == 0)
        {
            base.ThrowKunai(value);
        }

    }

    private IEnumerator IndicateImmortal()
    {
        while(immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    public override IEnumerator TakeDamage()
    {
        if(!immortal)
        {
            healthStat.CurrentVal -= 10;
            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("die");
            }
        }
    }

    public override void Death()
    {
        SceneManager.LoadScene("Failed");
        // myRigiBody.velocity = Vector2.zero;
        // MyAnimator.SetTrigger("idle");
        // healthStat.CurrentVal = healthStat.MaxVal;
        // transform.position = startPos;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag =="Coin")
        {
            GameManager.Instance.CollectedCoins++;
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag =="NextLevel")
        {
            Destroy(other.gameObject);
            SceneManager.LoadScene("Scene2");
        }
        if(other.gameObject.tag =="Finish")
        {
            Destroy(other.gameObject);
            SceneManager.LoadScene("Finish");
        }
    }
}
