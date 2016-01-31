using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickUp : MonoBehaviour {

    private bool m_paused;

	public GameObject inventory;
	public Sprite itemSprite;
    public bool destroyOnPickUp = false;
    public Game gameManager;

	private AudioSource audioClip;

    public enum Items
    {
		Amulet,
		SpellBook,
        BlackCandleWithPentagram,
		SacrificialKnife,
		UrnOfAshes
    }

    public Items inventoryIndex;

	void Start(){
		audioClip = GetComponent<AudioSource> ();
		inventory = GameObject.Find ("Inventory");
        gameManager = GameObject.Find("GameManager").GetComponent<Game>();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_paused) return;

		audioClip.Play();

        if (other.gameObject.tag == "NPC")
        {
            if (destroyOnPickUp)
            {
                Destroy(gameObject);
            }
            else
            {
				transform.position = new Vector2 (-100, -100);
				Debug.Log ((int)inventoryIndex);
				inventory.transform.GetChild((int)inventoryIndex).GetComponent<Image>().sprite = itemSprite;
                RegisterPickup();
            }
        }
    }

    void RegisterPickup()
    {
        if (m_paused) return;

        gameManager.Pickup();
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
