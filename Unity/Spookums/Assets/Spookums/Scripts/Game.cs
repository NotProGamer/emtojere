using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    float timer;
    float dt;
    bool isPaused;
    bool[] collectibles;

	// Use this for initialization
	void Start ()
    {
        timer = 300;
        dt = 1/30;
        isPaused = false;
        collectibles = new bool[5];

        for (int i = 0; i < collectibles.Length; i++)
            collectibles[i] = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (!isPaused)
        {
            timer -= dt;

            if (timer <= 0)
            {
                // end game with dawn sequence
            }

            if (EverythingCollected())
            {
                // unlock attic
            }
        }
	}

    bool EverythingCollected()
    {
        foreach (bool c in collectibles)
        {
            if (!c)
                return false;
        }

        return true;
    }
}
