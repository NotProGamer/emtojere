using UnityEngine;
using System.Collections;

public class HouseGenerator : MonoBehaviour {

	public GameObject room;
	public Material[] wallpapers;
	public Vector2[] basementBounds;
	public Vector2[] groundBounds;
	public Vector2[] firstBounds;
	public Vector2[] atticBounds;
	public int[] numberFloors;
	public float hRoom;

	// Use this for initialization
	void Start () {
		// Generate Basement;
		CreateFloor(numberFloors[0], basementBounds[0], basementBounds[1]);
		// Generate Ground floor;
		CreateFloor(numberFloors[1], groundBounds[0], groundBounds[1]);
		// Generate First floor;
		CreateFloor(numberFloors[2], firstBounds[0], firstBounds[1]);
		// Generate Attic;
		CreateFloor(numberFloors[3], atticBounds[0], atticBounds[1]);
	}
	
	void CreateFloor(int rooms, Vector2 leftBound, Vector2 rightBound){
		// Make sure rooms are behind the player and objects
		float zRoom = 3f;

		// Calculate average width of rooms
		float wRoom = (rightBound.x - leftBound.x)/rooms;

		float xRoom = leftBound.x + wRoom/2;
		float yRoom = leftBound.y;

		for (int i = 0; i < rooms; i++) {
			GameObject instance = (GameObject)Instantiate (room, new Vector3 (xRoom, yRoom, zRoom), Quaternion.identity);
			instance.transform.localScale = new Vector3 (1f, 1f, 1f);
			instance.transform.SetParent (transform);
			instance.GetComponentInChildren<MeshRenderer> ().material = wallpapers [Random.Range (0, wallpapers.Length)];
			xRoom += wRoom;
		}
	}
}
