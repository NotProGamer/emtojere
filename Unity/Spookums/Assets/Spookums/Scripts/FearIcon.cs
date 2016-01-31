using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FearIcon : MonoBehaviour {

	public Slider fearMeter;
	public Sprite[] faces;
	public AudioSource heartbeat;
	public AudioSource music;
	public AudioSource background;
    public NPCScript playerScript;

	private Image face;
	private float fear;

	void Start(){
		face = GetComponent<Image> ();
	}
		
	// Update is called once per frame
	void Update () {
		fear = fearMeter.value;

        playerScript.maxSpeed = Mathf.Lerp(2.5f, 5f, fear / fearMeter.maxValue);

		face.sprite = faces [Mathf.RoundToInt(fear)];

		heartbeat.volume = Mathf.RoundToInt (fear) / fearMeter.maxValue;

		music.volume = 0.3f * (fearMeter.maxValue) / (5f * fear + fearMeter.maxValue);
	
		background.volume = 0.8f * (fearMeter.maxValue) / (5f * fear + fearMeter.maxValue);
	}
}
