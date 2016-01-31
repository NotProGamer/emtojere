using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public GameObject fearMeter;
    public GameObject timeMeter;
    public GameObject[] collectibles;
    public Game game;
    public GameObject timerText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timeMeter.GetComponent<Slider>().value = (game.maxTimer - game.GetTimer()) / game.maxTimer;
        if((game.maxTimer - game.GetTimer()) / game.maxTimer > .75)
        {
            timerText.GetComponent<Text>().text = game.GetTimeAsString();
        }
    }
}
