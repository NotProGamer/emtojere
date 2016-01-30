using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour {

    private bool m_paused;

    public bool open = true;
    public Transform destination;
    public float teleportTimer = 0f;
    public float teleportChance = 0.5f;

    public float levelDistance = 0.1f;

    // Use this for initialization
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

        if (m_paused) return;

        if (teleportTimer > 0)
        {
            teleportTimer -= Time.deltaTime;
        }
        else if (teleportTimer < 0)
        {
            teleportTimer = 0;
        }
    }

    public void SetTimer(float timer)
    {
        if (m_paused) return;

        teleportTimer = timer;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision");
        if (m_paused) return;

        if (other.gameObject.tag == "NPC")
        {
            if (teleportTimer == 0)
            {
                //other.GetComponent<NPCScript>().Teleport(destination.position);

                NPCScript npcScript = other.GetComponent<NPCScript>();

                // if npc is lured 
                if (npcScript.IsLured())
                {
                    //if target and destination are on the same level but not this level then teleport

                    Debug.Log(GetDirection(npcScript.GetTarget()) == GetDirection(destination.position)
                        && GetDirection(npcScript.GetTarget()) != LureDirection.Across ? "true" :"false");

                    if (GetDirection(npcScript.GetTarget()) == GetDirection(destination.position)
                        && GetDirection(npcScript.GetTarget()) != LureDirection.Across)
                    {
                        npcScript.Teleport(destination.position);
                        destination.gameObject.GetComponent<Stairs>().SetTimer(2f);
                    }
                    else
                    {
                        Debug.Log("xyz");
                    }
                }
                else
                {
                    if (Random.value >= teleportChance)
                    {
                        npcScript.Teleport(destination.position);
                        destination.gameObject.GetComponent<Stairs>().SetTimer(2f);
                    }
                    else
                    {
                        // keep moving
                    }
                }
            }
        }
    }

    LureDirection GetDirection(Vector3 target)
    {
        if (target.y - transform.position.y >= levelDistance)
        {
            return LureDirection.Up;
        }
        else if (target.y - transform.position.y <= -levelDistance)
        {
            return LureDirection.Down;
        }
        else
        {
            return LureDirection.Across;
        }


    }

    enum LureDirection
    {
        Up,
        Across,
        Down
    }

    void OnPause()
    {
        m_paused = true;
    }

    void OnResume()
    {
        m_paused = false;
    }
}
