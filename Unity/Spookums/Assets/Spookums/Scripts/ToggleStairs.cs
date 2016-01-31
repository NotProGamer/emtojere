using UnityEngine;
using System.Collections;

public class ToggleStairs : MonoBehaviour {

    public Transform ladder;
    public Vector3 startPosition;
    public Vector3 stopPosition;
    public float maxSpeed = 1f;
    private bool atticUnlocked = false;
    public GameObject coverSprite;

    public float distance = 4;

    // Use this for initialization
    void Start ()
    {
        stairsScript = gameObject.GetComponent<Stairs>();
        startPosition = transform.position;
        stopPosition = new Vector3(startPosition.x, startPosition.y - distance, startPosition.z);
    }

    public bool enableStairs = false;

	// Update is called once per frame
	void Update ()
    {
        // hack trigger
        if (enableStairs && !atticUnlocked)
        {
            EnableStairs(true);
        }
        else if (!enableStairs && atticUnlocked)
        {
            EnableStairs(false);
        }


        // enable stair timer move
        if (atticUnlocked && ladder.GetComponent<Transform>().position != stopPosition)
        {
            if (ladder.GetComponent<Transform>().position.y <= stopPosition.y)
            {
                // stop moving
                ladder.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                ladder.GetComponent<Transform>().position = stopPosition;
            }
            else
            {
                // move 
                ladder.GetComponent<Rigidbody2D>().velocity = new Vector2(ladder.GetComponent<Rigidbody2D>().velocity.x, - 1 * maxSpeed );
            }
        }
        else if (atticUnlocked && transform.position == stopPosition)
        {
            stairsScript.enabled = true;
        }




    }

    private Stairs stairsScript;

    public void EnableStairs(bool enabled)
    {
        if (stairsScript != null)
        {
            atticUnlocked = enabled;
            stairsScript.enabled = enabled;
            //coverSprite.SetActive(false);
            GameObject.Find("CoverSprite").GetComponent<CoverController>().coverTimer = 1.5f;
            if (enabled == false)
            {
                //stairsScript.enabled = enabled;
                ladder.GetComponent<Transform>().position = startPosition;
            } 
        }
    }

}
