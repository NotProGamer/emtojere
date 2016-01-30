using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    enum GameState
    {
        WIN = 0,
        LOSE,
        PAUSE,
        PLAY
    };

    [SerializeField]private float timer = 300;
    float dt;
    bool isPaused;
    bool[] collectibles;
    int currentState;

    // Use this for initialization
    void Start ()
    {
        dt = Time.deltaTime;
        isPaused = false;
        collectibles = new bool[5];

        for (int i = 0; i < collectibles.Length; i++)
            collectibles[i] = false;

        //currentState = GameState.PLAY;
    }

    // Update is called once per frame
    void Update ()
    {
        // update delta time
        dt = Time.deltaTime;

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
}
