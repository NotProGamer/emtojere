using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour
{
    bool paused = false;
    public Game game;

    // Use this for initialization
    void Start ()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (paused)
        {
            int menuWidth = 100;
            int menuHeight = 120;

            // Make a background box
            GUI.Box(new Rect((Screen.width - menuWidth)/2, (Screen.height - menuHeight)/2, menuWidth, menuHeight), "PAUSED");

            // Resume
            if (GUI.Button(new Rect(((Screen.width - menuWidth) / 2) + 10, ((Screen.height - menuHeight) / 2) + (1*30), 80, 20), "Resume"))
            {
                paused = false;
                game.UnPause();
            }

            // Restart
            if (GUI.Button(new Rect(((Screen.width - menuWidth) / 2) + 10, ((Screen.height - menuHeight) / 2) + (2*30), 80, 20), "Restart"))
            {
                paused = false;
                game.Restart();
            }

            if (GUI.Button(new Rect(((Screen.width - menuWidth) / 2) + 10, ((Screen.height - menuHeight) / 2) + (3*30), 80, 20), "Quit"))
            {
                paused = false;
                game.Quit();
            }
        }
    }

    void OnPause()
    {
        paused = true;
    }

    void OnResume()
    {
        paused = false;
    }
}
