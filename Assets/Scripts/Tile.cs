using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	enum Door{
		NotDoor, GatorDoor, PossumDoor, FlamingoDoor  
	};

	[SerializeField]
	private bool isWalkable = true;
	[SerializeField]
	private bool isSpawnPoint = false;
	[SerializeField]
	private Door isDoor = Door.NotDoor;  

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
