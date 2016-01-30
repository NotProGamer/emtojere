using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HouseGenerator : MonoBehaviour {

	public GameObject room;
	public GameObject door;
	public Material[] wallpapers;
	public Material[] basementWallpapers;
	public Material[] atticWallpapers;
	public int[] numberFloors;
	public GameObject[] collectibles;
	public GameObject[] interactibles;

	private Vector2[] basementBounds = new Vector2[2]{new Vector2(-5.7f,-3.9f), new Vector2(5.3f,-3.9f)};
	private Vector2[] groundBounds = new Vector2[2]{new Vector2(-6.3f,-1.7f), new Vector2(6f,-1.7f)};
	private Vector2[] firstBounds = new Vector2[2]{new Vector2(-6.3f,0.5f), new Vector2(6f,0.5f)};
	private Vector2[] atticBounds = new Vector2[2]{new Vector2(-2.15f,2.7f), new Vector2(1.85f,2.7f)};
	private int j;
	private List<GameObject> collectibleRandomiser;
	private List<GameObject> randomiser;

	// Use this for initialization
	void Start () {

		// We want an interactible in every room
		randomiser = new List<GameObject>();
		randomiser.AddRange (interactibles);

		// We want the collectibles to not be in the attic
		int roomCount = numberFloors [0] + numberFloors [1] + numberFloors [2];
		collectibleRandomiser = new List<GameObject> ();
		collectibleRandomiser.AddRange (collectibles);
		// We add some null dummy entries to the list so that random rooms will get a collectible
		for (int i = collectibles.Length; i < roomCount; i++) {
			collectibleRandomiser.Add (null);
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
		float yRoom = leftBound.y;
		float[] xScale = new float[rooms];
		xScale[rooms-1] = rooms;

		for (int i = 0; i < rooms-1; i++) {
			xScale[i] = 1f + Random.Range (-0.3f, 0.3f);
			xScale[rooms-1] -=xScale[i];
		}

		float xRoom = leftBound.x + wRoom*xScale[0]/2;

		for (int i = 0; i < rooms; i++) {

			j = (int)Mathf.Repeat (j + 1, wallArray.Length);
			GameObject instance = (GameObject)Instantiate (room);
			instance.transform.position = new Vector3 (xRoom, yRoom, zRoom);
			instance.transform.localScale = new Vector3 (xScale[i], 0.95f, 1f);
			instance.transform.SetParent (transform);
			instance.GetComponentInChildren<MeshRenderer> ().material = wallArray [Random.Range (j, wallArray.Length)];

			// We need to refill the interactibles list if we've exhausted it, and then grab one at random
			if (randomiser.Count == 0) {
				randomiser.AddRange (interactibles);
			}
			int r = Random.Range (0, randomiser.Count);
			GameObject obj = Instantiate (randomiser [r]);
			obj.transform.position = instance.transform.position - new Vector3(0f, 1.15f, 1.5f);
			randomiser.RemoveAt (r);

			// Checking if the collectible list still has entries just for safety - we don't really care if we exhaust it.
			if (collectibleRandomiser.Count > 0) {
				r = Random.Range (0, collectibleRandomiser.Count);
				if (collectibleRandomiser [r] != null) {
					obj = Instantiate (collectibleRandomiser [r]);
					obj.transform.position = instance.transform.position - new Vector3(0f, 1.15f, 4.5f);
				}
				collectibleRandomiser.RemoveAt (r);
			}

			// Add a door to the next room!
			if (i < rooms - 1) {
				GameObject newDoor = (GameObject)Instantiate (door);
				newDoor.transform.position = new Vector3 (xRoom + xScale [i] * wRoom / 2, yRoom, zRoom - 1);
				xRoom += wRoom * (xScale [i] + xScale [i + 1]) / 2;
			}
		}
	}
}
