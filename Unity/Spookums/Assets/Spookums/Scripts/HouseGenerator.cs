using UnityEngine;
using System.Collections;

public class HouseGenerator : MonoBehaviour {

	public GameObject room;
	public Material[] wallpapers;
	public Material[] basementWallpapers;
	public Material[] atticWallpapers;
	public int[] numberFloors;

	private Vector2[] basementBounds = new Vector2[2]{new Vector2(-5.7f,-3.9f), new Vector2(5.3f,-3.9f)};
	private Vector2[] groundBounds = new Vector2[2]{new Vector2(-6.3f,-1.7f), new Vector2(6f,-1.7f)};
	private Vector2[] firstBounds = new Vector2[2]{new Vector2(-6.3f,0.5f), new Vector2(6f,0.5f)};
	private Vector2[] atticBounds = new Vector2[2]{new Vector2(-2,2.7f), new Vector2(2,2.7f)};
	private int j;

	// Use this for initialization
	void Start () {
		// Generate Basement;
		CreateFloor(numberFloors[0], basementBounds[0], basementBounds[1], basementWallpapers);
		// Generate Ground floor;
		CreateFloor(numberFloors[1], groundBounds[0], groundBounds[1], wallpapers);
		// Generate First floor;
		CreateFloor(numberFloors[2], firstBounds[0], firstBounds[1], wallpapers);
		// Generate Attic;
		CreateFloor(numberFloors[3], atticBounds[0], atticBounds[1], atticWallpapers);

		j = Random.Range(0, wallpapers.Length);
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
		}
	}
}
