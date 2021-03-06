using UnityEngine;
using System.Collections;

public class NPCScript : MonoBehaviour
{
    private bool m_paused;
    private Vector3 m_playVelocity;

    public float maxSpeed;
    private bool m_facingRight = true;
    int currentFloor;

    private bool m_lured;
    private bool m_scared;
    private bool m_cornered;
    float fearImmunityTimer;
    public float fearImmunityMax = 2.0f;
    bool fleeing;

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

    public Animator animator;

    // source-seeking data
    Vector3 alertLocation;
    int alertFloor;
    ArrayList stairs;

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

    public bool IsScared()
    {
        return m_scared;
    }

    public void EvacuateHouse()
    {
        GetComponent<PlayerAudio>().Fleeing();
        fleeing = true;
    }

    public bool IsFleeing()
    {
        return fleeing;
    }

    // Use this for initialization
    void Start()
    {
        m_target = transform.position;
        m_destination = transform.position;
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_rigidbody.velocity = Vector3.zero;
        //m_anim = GetComponent<Animator>();
        currentFearRating = 0f;
        fadeTimer = fadeTime;
        fadingDown = false;
        fadingUp = false;
        currentFloor = 0;
        m_lured = false;
        m_scared = false;
        m_cornered = false;
        fearImmunityTimer = 0.0f;
        fleeing = false;

        // populate stairs list
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        stairs = new ArrayList();

        foreach (GameObject o in objects)
        {
            if (o.name.Contains("Stair"))
                stairs.Add(o);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(m_rigidbody.velocity.x) > 1)
        {
            animator.SetBool("Walk", true);
        }
        else {
            animator.SetBool("Walk", false);
        }

        if (Mathf.Abs(m_rigidbody.velocity.x) > 3)
            animator.SetBool("Run", true);
        else {
            animator.SetBool("Run", false);
        }

        if (m_paused) return;

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
                transform.position = new Vector3(m_destination.x, m_destination.y, transform.position.z);

                // we've arrived at our destination. Clear target in order to select a new one.
                m_target = Vector3.zero;
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

            return;
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    SetDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition), true);
        //}

        if (m_reactionTimer > 0)
        {
            m_reactionTimer -= Time.deltaTime;
        }
        else if (m_reactionTimer < 0)
        {
            m_reactionTimer = 0;
        }

        if (fearImmunityTimer > 0)
        {
            fearImmunityTimer -= Time.deltaTime;
        }
        else if (fearImmunityTimer < 0)
        {
            fearImmunityTimer = 0.0f;
        }
    }

    public void Alert(Vector3 source, bool lure, int floor, bool useDelay = true)
    {
        if (m_paused || fadingDown || fadingUp || (m_scared && !fleeing) || fearImmunityTimer > 0) return;

        if (useDelay)
            m_reactionTimer = reactionDelay;
        else
            m_reactionTimer = 0.0f;

        alertLocation = source;
        alertFloor = floor;
        m_target = Vector3.zero;

        if (lure || m_cornered) m_lured = true;
        else m_scared = true;

        if (m_cornered)
        {
            // fear immunity timer stops us from fleeing to corner immediately after leaving it.
            fearImmunityTimer = fearImmunityMax;
            //m_cornered = false;
            //m_target = Vector3.zero; // clear target so it can be calculated correctly during navigation
        }
    }

    public int GetAlertFloor()
    {
        return alertFloor;
    }

    public int GetCurrentFloor()
    {
        return currentFloor;
    }

    public void SetDirection()
    {
        if (m_paused || fadingDown || fadingUp) return;

        float horizontalDirection = 0.0f;

        if (m_lured)        horizontalDirection = transform.position.x - m_target.x;
        else if (m_scared)  horizontalDirection = transform.position.x - alertLocation.x;

        if (horizontalDirection > 0)
        {
            hDir = 1f;
        }
        else if (horizontalDirection < 0)
        {
            hDir = -1f;
        }

        if (m_lured)
        {
            hDir *= -1;
        }
        else
        {
            currentFearRating += scareRating;
        }
    }

    public Vector3 GetTarget()
    {
        return m_target;
    }

    void FixedUpdate()
    {
        if (m_paused || fadingDown || fadingUp) return;

        //m_grounded = Physics2D.OverlapCircle(m_groundCheck.position, m_groundRadius, whatIsGround);

        // set grounded animation
        //anim.SetBool("Grounded", m_grounded);

        //if (m_grounded)
        {
            // if we are currently being lured, navigate to the source
            if (m_lured && m_target == Vector3.zero)
            {
                switch (alertFloor - currentFloor)
                {
                    case -3:
                    case -2:
                    case -1:
                        // below us. Find stairs on our floor that go down and target them
                        foreach (GameObject s in stairs)
                        {
                            Stairs stair = s.GetComponent<Stairs>();

                            if (stair.GetFloor() == currentFloor && stair.GetDestinationFloor() < currentFloor)
                            {
                                m_target = s.transform.position;
                                break;
                            }
                        }

                        break;
                    case 0:
                        // we're on the same floor. Set target directly
                        m_target = alertLocation;
                        break;
                    case 1:
                    case 2:
                    case 3:
                        // above us. Find stairs on our floor that go up and target them
                        foreach (GameObject s in stairs)
                        {
                            Stairs stair = s.GetComponent<Stairs>();

                            if (stair.GetFloor() == currentFloor && stair.GetDestinationFloor() > currentFloor)
                            {
                                m_target = s.transform.position;
                                break;
                            }
                        }

                        break;
                }
            }

            if (m_lured || m_scared)
            {
                SetDirection();

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
                    float speedFactor = m_lured ? 0.5f : 1.0f;

                    if (fleeing) speedFactor = 1.5f;

                    m_rigidbody.velocity = new Vector2(movement * (maxSpeed * speedFactor), m_rigidbody.velocity.y);
                    // set speed animation 
                    //anim.SetFloat("Speed", Mathf.Abs(movement)); // horizontal speed
                    //anim.SetFloat("vSpeed", m_rigidbody.velocity.y); // vertical speed
                }
                else
                {
                    m_rigidbody.velocity = Vector2.zero;
                }

                if (m_lured)
                {
                    float dx = Mathf.Abs(transform.position.x - m_target.x);
                    float dy = Mathf.Abs(transform.position.y - m_target.y);

                    if (dx < 1.0f && dy < 1.0f)
                    {
                        if (m_target == alertLocation)
                        {
                            if (m_lured) m_lured = false; // we've reached the source of our alert, return to neutral disposition
                        }

                        m_target = Vector3.zero;
                        m_rigidbody.velocity = Vector3.zero;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_paused) return;

            Debug.Log("NPC collided with " + other.gameObject.name + ".");
        if (other.gameObject.name.Contains("Wall") && m_scared)
        {

            // turn around and quail in terror
            Flip();
            m_rigidbody.velocity = Vector3.zero;

            m_scared = false; // whilst we're technically still -scared-, we're open to other stimuli now.
            m_cornered = true; // once cornered, we will no longer flee until we are lured first.
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

    public void Teleport(Vector3 destination, int newFloor)
    {
        if (m_paused) return;

		GetComponent<PlayerAudio> ().Stairs ();

        //if (doFade)
        {
            currentFloor = newFloor;
            fadeTimer = fadeTime;
            fadingDown = true;
            m_destination = destination;
            FreezeVelocity();
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