using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public GameObject fearMeter;
    public GameObject timeMeter;
    public GameObject[] collectibles;
    public Game game;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timeMeter.GetComponent<Slider>().value = (360 - game.GetTimer()) / 360;
//        fearMeter.GetComponent<Slider>().value = npc.GetFear();
        foreach(GameObject c in collectibles)
        {
            c.GetComponent<Image>().color = Color.gray;
        }
    }
}
