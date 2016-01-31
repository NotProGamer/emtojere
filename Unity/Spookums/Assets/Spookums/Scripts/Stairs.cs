using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour
{
    private bool m_paused;

    public bool open = true;
    public Transform destination;
    public float teleportTimer = 0f;
    public float teleportChance = 0.5f;

    public float levelDistance = 0.1f;

    int floor;

    // Use this for initialization
    void Start()
    {
        // determine what floor we are on using elevation markers.
        if      (transform.position.y > GameObject.Find("2").transform.position.y) floor = 2;
        else if (transform.position.y > GameObject.Find("1").transform.position.y) floor = 1;
        else if (transform.position.y > GameObject.Find("G").transform.position.y) floor = 0;
        else floor = -1;
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
            NPCScript npcScript = other.GetComponent<NPCScript>();

            if (teleportTimer == 0 && npcScript.GetCurrentFloor() != GetDestinationFloor())
            {
                // if npc is lured or evacuating house
                if (npcScript.IsLured() || npcScript.IsFleeing())
                {
					int df =  Mathf.Abs(npcScript.GetAlertFloor() - npcScript.GetCurrentFloor());
                    int ndf = Mathf.Abs(npcScript.GetAlertFloor() - GetDestinationFloor());
					
                    if (npcScript.GetAlertFloor() == GetDestinationFloor() ||
                         ndf < df)
                    {
                        npcScript.Teleport(destination.position, GetDestinationFloor());
                    }
                }
            }
        }
    }

    public int GetFloor()
    {
        return floor;
    }

    public int GetDestinationFloor()
    {
        // if the destination is above us, we go to the floor above, and vice versa
        return (destination.transform.position.y > transform.position.y) ? (floor + 1) : (floor - 1);
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
