using UnityEngine;
using System.Collections;

public class Tile {
	public enum TileType{
		NotDoor, GatorDoor, PossumDoor, FlamingoDoor, GenericDoor, Hazard  
	};

	[SerializeField]
	public bool isWalkable = true;
	[SerializeField]
	private bool isSpawnPoint = false;
	[SerializeField]
	public TileType type = TileType.NotDoor;  

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
