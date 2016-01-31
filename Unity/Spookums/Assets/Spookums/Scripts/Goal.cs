using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public GameObject gameManager;
    private Game gameScript;
	// Use this for initialization
	void Start () {
        gameScript = gameManager.GetComponent<Game>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        if (gameScript != null)
        {
            gameScript.goalCollected = true;
        }
    }
}
