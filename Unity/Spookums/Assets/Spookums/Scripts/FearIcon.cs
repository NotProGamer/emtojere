using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FearIcon : MonoBehaviour {

	public Slider fearMeter;
	public Sprite[] faces;

	private Image face;
	private float fear;

	void Start(){
		face = GetComponent<Image> ();
	}
		
	// Update is called once per frame
	void Update () {
		fear = fearMeter.value;

		face.sprite = faces [Mathf.RoundToInt(fear)];
	
	}
}
