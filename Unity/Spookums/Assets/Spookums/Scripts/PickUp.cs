using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    private bool m_paused;

    public Transform collectedLocation;
    public bool destroyOnPickUp = true;

    public enum Items
    {
        UrnOfAshes,
        SacrificialKnife,
        Amulet,
        BlackCandleWithPentagram,
        SpellBook
    }

    public Items inventoryIndex;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_paused) return;

        if (other.gameObject.tag == "NPC")
        {
            if (destroyOnPickUp)
            {
                Destroy(gameObject);
                RegisterPickup();
            }
            else
            {
                //Teleport to inventory location
                gameObject.transform.position = collectedLocation.position;
            }
        }
    }

    void RegisterPickup()
    {
        if (m_paused) return;

        //gameObject.transform.Find("GameManager").GetComponent<Game>().RegisterPickup(inventoryIndex);
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
