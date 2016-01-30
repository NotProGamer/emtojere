using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HouseGenerator : MonoBehaviour {

	public GameObject room;
	public Material[] wallpapers;
	public Material[] basementWallpapers;
	public Material[] atticWallpapers;
	public int[] numberFloors;
	public GameObject[] collectibles;

	private Vector2[] basementBounds = new Vector2[2]{new Vector2(-5.7f,-3.9f), new Vector2(5.3f,-3.9f)};
	private Vector2[] groundBounds = new Vector2[2]{new Vector2(-6.3f,-1.7f), new Vector2(6f,-1.7f)};
	private Vector2[] firstBounds = new Vector2[2]{new Vector2(-6.3f,0.5f), new Vector2(6f,0.5f)};
	private Vector2[] atticBounds = new Vector2[2]{new Vector2(-2,2.7f), new Vector2(2,2.7f)};
	private int j;
	private List<GameObject> randomiser;

	// Use this for initialization
	void Start () {
		
		// We want the collectibles to not be in the attic
		int roomCount = numberFloors [0] + numberFloors [1] + numberFloors [2];
		randomiser = new List<GameObject> ();
		randomiser.AddRange (collectibles);
		// We add some null dummy entries to the list so that random rooms will get a collectible
		for (int i = collectibles.Length; i < roomCount; i++) {
			randomiser.Add (null);
		}

		j = Random.Range(0, wallpapers.Length);

		// Generate Basement;
		CreateFloor(numberFloors[0], basementBounds[0], basementBounds[1], basementWallpapers);
		// Generate Ground floor;
		CreateFloor(numberFloors[1], groundBounds[0], groundBounds[1], wallpapers);
		// Generate First floor;
		CreateFloor(numberFloors[2], firstBounds[0], firstBounds[1], wallpapers);
		// Generate Attic;
		CreateFloor(numberFloors[3], atticBounds[0], atticBounds[1], atticWallpapers);
	}
		
	
	void CreateFloor(int rooms, Vector2 leftBound, Vector2 rightBound, Material[] wallArray){
		// Make sure rooms are behind the player and objects
		float zRoom = 4.5f;

		// Calculate average width of rooms
		float wRoom = (rightBound.x - leftBound.x)/rooms;
		float xRoom = leftBound.x + wRoom/2;
		float yRoom = leftBound.y;

		for (int i = 0; i < rooms; i++) {
			j = (int)Mathf.Repeat (j + 1, wallArray.Length);
			GameObject instance = (GameObject)Instantiate (room);
			instance.transform.position = new Vector3 (xRoom, yRoom, zRoom);
			instance.transform.localScale = new Vector3 (1f, 0.95f, 1f);
			instance.transform.SetParent (transform);
			instance.GetComponentInChildren<MeshRenderer> ().material = wallArray [Random.Range (j, wallArray.Length)];
			xRoom += wRoom;

			if (randomiser.Count > 0) {
				int c = Random.Range (0, randomiser.Count);
				if (randomiser [c] != null) {
					GameObject obj = Instantiate (randomiser [c]);
					obj.transform.position = instance.transform.position - new Vector3(0f, 1.15f, 4.5f);
				}
				randomiser.RemoveAt (c);
			}
		}
	}
}
