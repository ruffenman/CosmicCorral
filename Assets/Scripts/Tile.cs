using UnityEngine;
using System.Collections;

public class Tile {
	public enum Door{
		NotDoor, GatorDoor, PossumDoor, FlamingoDoor, GenericDoor  
	};

	[SerializeField]
	private bool isWalkable = true;
	[SerializeField]
	private bool isSpawnPoint = false;
	[SerializeField]
	public Door isDoor = Door.NotDoor;  

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
