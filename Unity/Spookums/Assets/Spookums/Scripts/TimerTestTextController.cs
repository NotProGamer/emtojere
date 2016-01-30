using UnityEngine;
using System.Collections;

public class TimerTestTextController : MonoBehaviour
{
    public Game game;
    string text;

    // Use this for initialization
    void Start ()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        gameObject.GetComponent<TextMesh>().text = game.GetTimeAsString();
    }
}
