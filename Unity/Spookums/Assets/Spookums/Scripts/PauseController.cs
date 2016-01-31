using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour
{
    bool paused = false;
    public Game game;
    public Texture2D menuTexture;
    public Texture2D resumeButtonTexture;
    public Texture2D restartButtonTexture;
    public Texture2D quitButtonTexture;

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
        if (paused && !game.IsShowingMenu())
        {
            int menuWidth = 450;
            int menuHeight = 225;
            int buttonWidth = 204;
            int buttonHeight = 54;

            // Make a background box
            GUIStyle currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.0f));
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            GUI.Box(new Rect((Screen.width - menuWidth)/2, (Screen.height - menuHeight)/2, menuWidth, menuHeight), menuTexture, currentStyle);

            /*
             *  Sam's note: I am *so* sorry about the position calculation
             */

            // Resume
            if (GUI.Button(new Rect(((Screen.width - (menuWidth / 2)) / 2) + 10, ((Screen.height - menuHeight) / 2) + ((buttonHeight / 2)), buttonWidth, buttonHeight), resumeButtonTexture, currentStyle))
            {
                paused = false;
                game.UnPause();
            }

            // Restart
            if (GUI.Button(new Rect(((Screen.width - (menuWidth/2)) / 2) + 10, ((Screen.height - menuHeight) / 2) + (1.5f* (buttonHeight)), buttonWidth, buttonHeight), restartButtonTexture, currentStyle))
            {
                paused = false;
                game.Restart();
            }

            if (GUI.Button(new Rect(((Screen.width - (menuWidth / 2)) / 2) + 10, ((Screen.height - menuHeight) / 2) + (2.5f* (buttonHeight)), buttonWidth, buttonHeight), quitButtonTexture, currentStyle))
            {
                paused = false;
                game.Quit();
            }
        }
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

    void OnPause()
    {
        paused = true;
    }

    void OnResume()
    {
        paused = false;
    }
}
