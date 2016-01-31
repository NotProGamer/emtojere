using UnityEngine;
using System.Collections;

public class CoverController : MonoBehaviour {

    public float coverTimer;
    public float coverTimerMax = 1.5f;

	// Use this for initialization
	void Start () {
        coverTimer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (coverTimer > 0)
        {
            coverTimer -= Time.deltaTime;

            GetComponent<SpriteRenderer>().color = (new Color(255, 255, 255, coverTimer / coverTimerMax));

            if (coverTimer < 0)
            {
                coverTimer = 0;
            }
        }
	}
}
