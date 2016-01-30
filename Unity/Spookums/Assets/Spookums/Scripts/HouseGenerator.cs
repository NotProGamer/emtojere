using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HouseGenerator : MonoBehaviour {

	public GameObject room;
	public GameObject door;
	public GameObject stairUp;
	public GameObject stairDown;
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
	private List<GameObject> interactibleRandomiser;
	private List<GameObject> collAndStairs = new List<GameObject>();
	[SerializeField]private List<GameObject> downStairs = new List<GameObject>();
	[SerializeField]private List<GameObject> upStairs = new List<GameObject>();

	// Use this for initialization
	void Start () {

		// We want an interactible in every room
		interactibleRandomiser = new List<GameObject>();
		interactibleRandomiser.AddRange (interactibles);

		collectibleRandomiser = new List<GameObject> ();
		collectibleRandomiser.AddRange (collectibles);

		j = Random.Range(0, wallpapers.Length);

		// Generate Basement;
		CollectiblesAndStairs(true, false, numberFloors[0]);
		CreateFloor(numberFloors[0], basementBounds[0], basementBounds[1], basementWallpapers);
		// Generate Ground floor;
		CollectiblesAndStairs(true, true, numberFloors[1]);
		CreateFloor(numberFloors[1], groundBounds[0], groundBounds[1], wallpapers);
		// Generate First floor;
		CollectiblesAndStairs(false, true, numberFloors[2]);
		CreateFloor(numberFloors[2], firstBounds[0], firstBounds[1], wallpapers);
		// Generate Attic;
		CollectiblesAndStairs(false, false, 0);
		CreateFloor(numberFloors[3], atticBounds[0], atticBounds[1], atticWallpapers);

		// Link up the stairs
		downStairs[0].GetComponent<Stairs>().destination = upStairs[0].transform;
		upStairs[0].GetComponent<Stairs>().destination = downStairs[0].transform;
		downStairs[1].GetComponent<Stairs>().destination = upStairs[1].transform;
		upStairs[1].GetComponent<Stairs>().destination = downStairs[1].transform;
	}

	void CollectiblesAndStairs(bool up, bool down, int rooms){
		collAndStairs.Clear();
		if (up){
			collAndStairs.Add (stairUp);
		}
		if (down)
			collAndStairs.Add (stairDown);

		int remaining = rooms - collAndStairs.Count;

		if (remaining > 0) {
			for (int i = 0; i < remaining; i++) {
				if (collectibleRandomiser.Count > 0) {
					int j = Random.Range (0, collectibleRandomiser.Count);
					collAndStairs.Add (collectibleRandomiser [j]);
					collectibleRandomiser.RemoveAt (j);
				}
			}
		}
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
			xScale[i] = 1f + Random.Range (-0.15f, 0.15f);
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

			if (interactibleRandomiser.Count == 0) {
				interactibleRandomiser.AddRange (interactibles);
			}
			int r = Random.Range (0, interactibleRandomiser.Count);
			GameObject obj = Instantiate (interactibleRandomiser [r]);
			// Interactibles should go on the left side of the room except for the final room on each floor
			float offset = 1.1f - wRoom * xScale [i] / 2;

			if (i == rooms - 1)
				offset = -offset;

			obj.transform.position = instance.transform.position + new Vector3(offset, -1.15f, -1.5f);
			interactibleRandomiser.RemoveAt (r);

			// Checking if the collectible list still has entries just for safety - we don't really care if we exhaust it.
			if (collAndStairs.Count > 0) {
				r = Random.Range (0, collAndStairs.Count);
				if (collAndStairs [r] != null) {
					obj = Instantiate (collAndStairs [r]);
					// Collectibles should go on the right side except the final room
					obj.transform.position = instance.transform.position + new Vector3(-offset, -1.02f, -3.5f);
					if (obj.name == "StairDown(Clone)")
						downStairs.Add (obj);
					if (obj.name == "StairUp(Clone)")
						upStairs.Add (obj);
				}
				collAndStairs.RemoveAt (r);
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
