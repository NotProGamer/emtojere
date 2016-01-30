using UnityEngine;
using System.Collections;

public class CandleSpawner : MonoBehaviour {

	public GameObject candle;
	public int minCandles;
	public int maxCandles;
	public float yCandle;
	public float xMin;
	public float xMax;
	public Vector3 scale = new Vector3 (0.8f, 0.8f, 1f);


	void Start(){
		int numCandles = Random.Range (minCandles, maxCandles + 1);

		for (int i = 0; i < numCandles; i++) {
			GameObject instance = (GameObject)Instantiate (candle);
			instance.transform.position = new Vector3 (transform.position.x + Random.Range (xMin, xMax), transform.position.y + yCandle, transform.position.z);
			instance.transform.SetParent (transform);
			instance.transform.localScale = scale;
		}
	}

}
