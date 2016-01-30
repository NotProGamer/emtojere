using UnityEngine;
using System.Collections;

public class NPCScript : MonoBehaviour
{
    private bool m_paused;
    private Vector3 m_playVelocity;

    public float maxSpeed = 10f;
    private bool m_facingRight = true;

    private bool m_lured = true;
    private Vector3 m_target;
    private float hDir = 0f;

    //private Animator m_anim;

    private Rigidbody2D m_rigidbody;
    public bool m_grounded;
    public Transform m_groundCheck;
    float m_groundRadius = 0.2f; // remember to modify the npc colliders
    public LayerMask whatIsGround;

    public float maxFearRating = 5f;
    public float currentFearRating;
    public float scareRating = 1f;
    float fadeTimer;
    public float fadeTime = 0.5f;
    bool fadingDown;
    bool fadingUp;
    Vector3 m_destination;
    public SpriteRenderer sprite;

    public float GetFearRating()
    {
        return currentFearRating / maxFearRating;
    }

    public float reactionDelay = 2f;
    private float m_reactionTimer = 0f;

    public bool IsLured()
    {
        return m_lured;
    }


    // Use this for initialization
    void Start()
    {
        m_target = transform.position;
        m_destination = transform.position;
        m_rigidbody = GetComponent<Rigidbody2D>();
        //m_anim = GetComponent<Animator>();
        currentFearRating = 0f;
        fadeTimer = fadeTime;
        fadingDown = false;
        fadingUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingDown)
        {
            fadeTimer -= Time.deltaTime;

            // fade to invisible
            float alpha = (fadeTimer / fadeTime);
            SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer>();
            //gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alpha);
            renderer.color = new Color(255, 255, 255, alpha);

            if (fadeTimer <= 0)
            {
                fadeTimer = 0.0f;
                fadingDown = false;
                fadingUp = true;
                transform.position = m_destination;
            }

            return;
        }
        else if (fadingUp)
        {
            fadeTimer += Time.deltaTime;

            // fade up to opaque
            float alpha = (fadeTimer / fadeTime);
            SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer>();
            //gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alpha);
            renderer.color = new Color(255, 255, 255, alpha);

            if (fadeTimer >= fadeTime)
            {
                fadeTimer = fadeTime;
                fadingUp = false;
                UnFreezeVelocity();
            }

            SetDirection(m_target, IsLured());

            return;
        }

        if (m_paused) return;

        // DEBUG: MANUAL TELEPORT -- REMOVE BEFORE PUBLISH
        if (Input.GetKeyDown(KeyCode.T))
        {
            Teleport(m_target);
        }

        if (Input.GetMouseButtonDown(0))
        {
            SetDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition), true);
        }

        if (m_reactionTimer > 0)
        {
            m_reactionTimer -= Time.deltaTime;
        }
        else if (m_reactionTimer < 0)
        {
            m_reactionTimer = 0;
        }
    }

    public void SetDirection(Vector3 source, bool lure)
    {
        if (m_paused || fadingDown || fadingUp) return;

        m_reactionTimer = reactionDelay;

        m_target = source;
        m_target.z = transform.position.z;
        float horizontalDirection = transform.position.x - m_target.x;
        if (horizontalDirection > 0)
        {
            hDir = 1f;
        }
        else if (horizontalDirection < 0)
        {
            hDir = -1f;
        }

        if (lure)
        {
            hDir *= -1;
        }
        else
        {
            currentFearRating += scareRating;
        }
        m_lured = lure;
    }

    public Vector3 GetTarget()
    {
        return m_target;
    }

    void FixedUpdate()
    {
        if (m_paused || fadingDown || fadingUp) return;

        m_grounded = Physics2D.OverlapCircle(m_groundCheck.position, m_groundRadius, whatIsGround);

        // set grounded animation
        //anim.SetBool("Grounded", m_grounded);

        if (m_grounded)
        {

            // movement
            //float movement = Input.GetAxis("Horizontal");
            float movement = hDir;

            // flip animation
            if (movement > 0 && !m_facingRight)
            {
                Flip();
            }
            else if (movement < 0 && m_facingRight)
            {
                Flip();
            }

            if (m_reactionTimer == 0)
            {
                m_rigidbody.velocity = new Vector2(movement * maxSpeed, m_rigidbody.velocity.y);
                // set speed animation 
                //anim.SetFloat("Speed", Mathf.Abs(movement)); // horizontal speed
                //anim.SetFloat("vSpeed", m_rigidbody.velocity.y); // vertical speed
            }
            else
            {
                m_rigidbody.velocity = Vector2.zero;
            }

            if (transform.position.x - m_target.x < 1.0f && transform.position.y - m_target.y < 1.0f)
            {
                m_target = transform.position;
                m_rigidbody.velocity = Vector3.zero;
            }
        }
    }

    //Flip the NPC Sprite based on movement direction
    void Flip()
    {
        if (m_paused || fadingUp || fadingDown) return;

        m_facingRight = !m_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Teleport(Vector3 destination, bool doFade = true)
    {
        if (m_paused) return;

        if (doFade)
        {
            fadeTimer = fadeTime;
            fadingDown = true;
            m_destination = destination;
            FreezeVelocity();
        }
        else
        {
            transform.position = destination;
        }
    }

    void OnPause()
    {
        m_paused = true;
        m_playVelocity = m_rigidbody.velocity;
        m_rigidbody.velocity = Vector3.zero;
    }

    void OnResume()
    {
        m_paused = false;
        m_rigidbody.velocity = m_playVelocity;
        m_playVelocity = Vector3.zero;
    }

    void FreezeVelocity()
    {
        m_playVelocity = m_rigidbody.velocity;
        m_rigidbody.velocity = Vector3.zero;
    }

    void UnFreezeVelocity()
    {
        m_rigidbody.velocity = m_playVelocity;
        m_playVelocity = Vector3.zero;
    }
}