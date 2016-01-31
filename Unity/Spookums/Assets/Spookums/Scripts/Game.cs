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
    public bool goalCollected;
    GameState currentState;
    int collectibleCount;


    public Texture2D loseMenuTexture;
    public Texture2D winMenuTexture;
    public Texture2D replayButtonTexture;
    public Texture2D exitButtonTexture;

    public GameObject ladderExit;

    public NPCScript npc;

    public void Pickup()
    {
        collectibleCount++;
    }

    void Init()
    {
        paused = false;
        timer = maxTimer; // set game timer

        // set collectables as not found
        for (int i = 0; i < collectibles.Length; i++)
            collectibles[i] = false;

        // place NPC at start position
        //npc.Teleport(new Vector3((float)-5.45, (float)-1.8, 0), false); // depreciated
        collectibleCount = 0;
    }

    // Use this for initialization
    void Start ()
    {
        collectibles = new bool[5]; // create collectibles array
        dt = Time.deltaTime;

        Init(); // reset level 

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

            // end the game if timer runs out
            if (goalCollected)
            {
                //Pause();
                currentState = GameState.WIN;
            }

            if (timer <= 0)
            {
                // end game with dawn sequence
                Pause();
                currentState = GameState.LOSE;
            }

            if (EverythingCollected())
            {
                // unlock attic
                if (ladderExit != null)
                {
                    ladderExit.GetComponent<ToggleStairs>().enableStairs = true;
                }
                else
                {
                    Debug.Log("LadderExit is null");
                }
                
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

        if (npc.IsFleeing())
        {
            currentState = GameState.LOSE;
        }

        ProcessState();
    }

    bool EverythingCollected()
    {
        //foreach (bool c in collectibles)
        //{
        //    if (!c)
        //        return false;
        //}

        return collectibleCount >= 5;//false;
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
            int menuWidth = 450;
            int menuHeight = 225;
            int buttonWidth = 204;
            int buttonHeight = 54;

            // Make a background box
            GUIStyle currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.0f));
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            GUI.Box(new Rect((Screen.width - menuWidth) / 2, (Screen.height - menuHeight) / 2, menuWidth, menuHeight), loseMenuTexture, currentStyle);

            /*
             *  Sam's note: I am *so* sorry about the position calculation
             */

            // Resume
            if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth, Screen.height / 2 + buttonHeight / 2, buttonWidth, buttonHeight), replayButtonTexture, currentStyle))
            {
                paused = false;
                Restart();
            }

            // Restart
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + buttonHeight / 2, buttonWidth, buttonHeight), exitButtonTexture, currentStyle))
            {
                paused = false;
                Quit();
            }
        }

        if (currentState == GameState.WIN)
        {
            int menuWidth = 450;
            int menuHeight = 225;
            int buttonWidth = 204;
            int buttonHeight = 54;

            // Make a background box
            GUIStyle currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.0f));
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            GUI.Box(new Rect((Screen.width - menuWidth) / 2, (Screen.height - menuHeight) / 2, menuWidth, menuHeight), winMenuTexture, currentStyle);

            /*
             *  Sam's note: I am *so* sorry about the position calculation
             */

            // Resume
            if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth, Screen.height / 2 + buttonHeight / 2, buttonWidth, buttonHeight), replayButtonTexture, currentStyle))
            {
                    paused = false;
                    Restart();
            }

            // Restart
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + buttonHeight / 2, buttonWidth, buttonHeight), exitButtonTexture, currentStyle))
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

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
