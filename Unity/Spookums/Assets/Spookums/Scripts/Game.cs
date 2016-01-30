using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    enum GameState
    {
        WIN,
        LOSE,
        PAUSE,
        PLAY
    };

    public float maxTimer = 360;
    float timer;
    float dt;
    bool paused;
    bool[] collectibles;
    GameState currentState;

    // Use this for initialization
    void Start ()
    {
        paused = false;
        timer = maxTimer;
        dt = Time.deltaTime;
        collectibles = new bool[5];

        for (int i = 0; i < collectibles.Length; i++)
            collectibles[i] = false;

        currentState = GameState.PLAY;
    }

    // Update is called once per frame
    void Update ()
    {
        // update delta time
        dt = Time.deltaTime;

        if (!paused)
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

        // handle inputs
        // ESC : Pause Game or Resume Game
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            // tell all gameobjects to pause
            Object[] objects = FindObjectsOfType(typeof(GameObject));

            foreach (GameObject g in objects)
            {
                g.SendMessage("OnPause", SendMessageOptions.DontRequireReceiver);
            }

            paused = true;
            currentState = GameState.PAUSE;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            UnPause();
        }
    }

    bool EverythingCollected()
    {
        foreach (bool c in collectibles)
        {
            if (!c)
                return false;
        }

        return false;
    }

    public string GetTimeAsString()
    {
        if (timer <= 0)
            return "00:00";

        int minutes = (int)(timer / 60);
        int seconds = (int)(timer % 60);

        string result = minutes + ":";

        if (seconds < 10)
            result += "0";

        return result + seconds;
    }

    public float GetTimer()
    {
        return timer;
    }

    public void UnPause()
    {
        // tell all gameobjects to resume
        Object[] objects = FindObjectsOfType(typeof(GameObject));

        foreach (GameObject g in objects)
        {
            g.SendMessage("OnResume", SendMessageOptions.DontRequireReceiver);
        }

        paused = false;
        currentState = GameState.PLAY;
    }

    public void Restart()
    {
        paused = false;
        currentState = GameState.PLAY;
    }

    public void Quit()
    {
        paused = false;
        Application.Quit();
    }
}
