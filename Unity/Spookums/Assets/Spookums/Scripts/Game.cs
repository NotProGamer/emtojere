using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    enum GameState
    {
        PLAY,
        WIN,
        LOSE,
        PAUSE,
    };

    public float maxTimer = 360;
    float timer;
    float dt;
    bool paused;
    bool[] collectibles;
    GameState currentState;

    public NPCScript npc;

    void Init()
    {
        paused = false;
        timer = maxTimer;

        for (int i = 0; i < collectibles.Length; i++)
            collectibles[i] = false;

        // place NPC at start position
        //npc.Teleport(new Vector3((float)-5.45, (float)-1.8, 0), false);
    }

    // Use this for initialization
    void Start ()
    {
        collectibles = new bool[5];
        dt = Time.deltaTime;

        Init();

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
                Pause();
                currentState = GameState.LOSE;
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
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            UnPause();
        }

        ProcessState();
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
            return "0:00";

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

    void Pause()
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
        Application.LoadLevel(0);
    }

    public void Quit()
    {
        paused = false;
        Application.Quit();
    }

    public void RegisterPickup(int index)
    {
        collectibles[index] = true;
    }

    // any special code behaviours based on game state go here
    void ProcessState()
    {
        switch (currentState)
        {
            case GameState.LOSE:
                break;
            case GameState.PAUSE:
                break;
            case GameState.PLAY:
                break;
            case GameState.WIN:
                break;
        }
    }

    void OnGUI()
    {
        // Game Over screen
        if (currentState == GameState.LOSE)
        {
            int menuWidth = 190;
            int menuHeight = 70;

            // Make a background box
            GUI.Box(new Rect((Screen.width - menuWidth) / 2, (Screen.height - menuHeight) / 2, menuWidth, menuHeight), "GAME OVER!\nTry again?");

            // Resume
            if (GUI.Button(new Rect(((Screen.width - menuWidth) / 2) + 10, ((Screen.height - menuHeight) / 2) + 40, 80, 20), "Yes"))
            {
                paused = false;
                currentState = GameState.PLAY;
                Restart();
            }

            if (GUI.Button(new Rect(((Screen.width - menuWidth) / 2) + 10 + 90, ((Screen.height - menuHeight) / 2) + 40, 80, 20), "No"))
            {
                paused = false;
                Quit();
            }
        }

        if (currentState == GameState.WIN)
        {
            int menuWidth = 190;
            int menuHeight = 70;

            // Make a background box
            GUI.Box(new Rect((Screen.width - menuWidth) / 2, (Screen.height - menuHeight) / 2, menuWidth, menuHeight), "Congratulations, you win!\nPlay again?");

            // Resume
            if (GUI.Button(new Rect(((Screen.width - menuWidth) / 2) + 10, ((Screen.height - menuHeight) / 2) + 40, 80, 20), "Yes"))
            {
                paused = false;
                Restart();
            }

            if (GUI.Button(new Rect(((Screen.width - menuWidth) / 2) + 10 + 90, ((Screen.height - menuHeight) / 2) + 40, 80, 20), "No"))
            {
                paused = false;
                Quit();
            }
        }
    }

    public void AtticCheck()
    {
        if (EverythingCollected())
        {
            currentState = GameState.WIN;
        }
    }

    public bool IsShowingMenu()
    {
        return currentState == GameState.LOSE || currentState == GameState.WIN;
    }
}
