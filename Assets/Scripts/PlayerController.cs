using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;

    public bool hit;
    private float speed = 4f;
    private float jumpingPower = 10f;
    private bool isFacingRight = true;

    private Collider2D platformCollider;
    public Animator anim;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public GameObject Ui;
    public GameObject GameOver;

    public float jumpFloat = 0.0f;
    public int maxHeart = 3;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public bool isMovingLeft = false;
    public bool isMovingRight = false;

    public bool HitCuy = false;

    public SpriteRenderer sprites;
    public Sprite sprite;

    public bool jumpIn;

    public bool isJumping;
    private AudioSource audioSource;
    public AudioClip footstepClip;

    void Start()
    {
        anim.SetBool("in", true);
        StartCoroutine(StartGame(1.0f));


        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (maxHeart == 3)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
        }
        else if (maxHeart == 2)
        {
            heart1.SetActive(false);
            heart2.SetActive(true);
            heart3.SetActive(true);
        }
        else if (maxHeart == 1)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(true);
        }
        else
        {
            anim.SetBool("dead", true);
            StartCoroutine(StopDead(0.5f));
            GameOver.SetActive(true);
            Ui.SetActive(false);
            sprites.sprite = sprite;
        }

        horizontal = Input.GetAxis("Horizontal");
        if (isMovingLeft)
        {
            horizontal = -1f;
        }
        else if (isMovingRight)
        {
            horizontal = 1f;
        }

        if (horizontal > 0.4f && IsGrounded() || horizontal < -0.4f && IsGrounded())
        {
            anim.SetBool("run", true);
            PlayFootstepSound();
        }
        else
        {
            anim.SetBool("run", false);
            StopFootstepSound();
        }

        if (!IsGrounded())
        {
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (isJumping && IsGrounded())
        {
            Jump();
        }


        Flip();
        Hit();
        AttackAnimation();
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
    }

    public void StopJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }

    public void OnJumpButtonDown()
    {
        isJumping = true;
    }

    public void OnJumpButtonUp()
    {
        isJumping = false;
        if (rb.velocity.y > 0f)
        {
            StopJump();
        }
    }



    IEnumerator StopDead(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("dead", false);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Hit()
    {
        var hitDown = Input.GetKeyDown(KeyCode.C);
        var hitUp = Input.GetKeyUp(KeyCode.C);

        if (hitDown && !hitUp || HitCuy)
        {
            hit = true;
        }
        else if (!hitDown && hitUp || !HitCuy)
        {
            hit = false;
        }
        else if (hitDown && hitUp)
        {
            hit = false;
        }
        else if (!hitDown && !hitUp)
        {
            hit = false;
        }
    }

    public void AttackAnimation()
    {
        if (hit == true)
        {
            anim.SetBool("attack", true);
        }
        else
        {
            anim.SetBool("attack", false);
        }
    }

    IEnumerator StartGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("in", false);
    }

    IEnumerator StopHit(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("hit", false);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Enemy"))
        {
            anim.SetBool("hit", true);
            maxHeart -= 1;
            StartCoroutine(StopHit(0.3f));
        }
    }

    public void LeftUp()
    {
        isMovingLeft = false;
    }

    public void LeftDown()
    {
        isMovingLeft = true;
    }

    public void RightUp()
    {
        isMovingRight = false;
    }

    public void RightDown()
    {
        isMovingRight = true;
    }

    public void HitDown()
    {
        HitCuy = true;
        StartCoroutine(Stop(0.00001f));
    }

    IEnumerator Stop(float delay)
    {
        yield return new WaitForSeconds(delay);
        HitCuy = false;
    }

    void PlayFootstepSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = footstepClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void StopFootstepSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
