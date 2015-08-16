using UnityEngine;
using System.Collections;

public class AnimalController : MonoBehaviour {

	#region instance variables
	public enum Direction {North, South, East, West};
	const float stepSize = 2f;
	const float attractionRadius = 10.0f;

	// public variables
	[SerializeField]
	public bool attracted;
	[SerializeField]
	public Direction direction;
	[SerializeField]
	public Vector2 velocity;

	// private variables 
	private int randDir;
	private Vector2 tempVector;
	private ArrayList validDirections;
	private Collider2D closestLure;
	private float distToClosestLure;
	private float overlapCheckRadius;
	private float dx, dy;
	private bool respawn;
	#endregion

	#region methods
	// Use this for initialization
	void Start () {
		attracted = false;
		respawn = false;
		direction = Direction.South;
		velocity = new Vector2 (0.0f, -0.03f);
		validDirections = new ArrayList ();
		tempVector = Vector2.zero;
		distToClosestLure = attractionRadius + 50.0f;
	}

	// Update is called once per frame
	// TODO: collision handling for different types of colliders (walls, escape pods, lures)
	void Update () {


		if (GameManager.levelManager.levelLoaded) {

			// respawn if necessary
			if(respawn)
			{
				GameObject respawnPoint = GameObject.FindWithTag("Respawn");
				if(respawnPoint!=null)
				{
					transform.position = (Vector3)(new Vector2(respawnPoint.transform.position.x, respawnPoint.transform.position.y));
					direction = Direction.North;
					velocity = new Vector2 (0.0f, 0.03f);
					respawn = false;
					attracted = false;
				}
			}

			// check to see if the animal is in a door
			Vector2 tileCoords = GameManager.map.GetTileCoordFromWorldPos (transform.position + (Vector3)velocity);
			if (GameManager.map.mapTile [(int)tileCoords.x, (int)tileCoords.y].type == Tile.TileType.GenericDoor)
			{
				velocity = Vector2.zero;
				GameManager.levelManager.numAnimalsRescued++;
				respawn = true;
				if(GameManager.levelManager.numAnimalsRescued >=3)
					GameManager.levelManager.Win ();
			} 
			// Check to see if the animal has hit a hazard
			else if (GameManager.map.mapTile [(int)tileCoords.x, (int)tileCoords.y].type == Tile.TileType.Hazard)
			{
				// player loses a life
				GameManager.levelManager.numLives--;
				if(GameManager.levelManager.numLives == 0)
					GameManager.levelManager.Lose ();
				//else{

				//}
				// animal is removed from the game

				// spawn a new animal
				respawn = true;

			}

			else 
			{
				checkForAttraction (ref attracted, ref closestLure, ref distToClosestLure);

				if (attracted && closestLure !=null) 
				{
					// try to move towards the lure 
					// otherwise move along the opposite axis if there is a dx / dy or just keep moving straight
					dx = transform.position.x - closestLure.transform.position.x;
					dy = transform.position.y - closestLure.transform.position.y;

					if (Mathf.Abs (dx) < 0.5 && Mathf.Abs (dy) < 0.5)
					{
						Destroy (closestLure.gameObject);
						closestLure = null;
						attracted = false;
					}

					// move on the x axis
					if (Mathf.Abs (dx) > Mathf.Abs (dy)) { 
						if (dx > 0) {
							if (!tryMovingWest (ref direction, ref velocity)) {
								if (dy > 0) {
									tryMovingSouth (ref direction, ref velocity);
								} else {
									tryMovingNorth (ref direction, ref velocity);
								}
							}
						} 
						else 
						{
							if (!tryMovingEast (ref direction, ref velocity)) {
								if (dy > 0) {
									tryMovingSouth (ref direction, ref velocity);
								} else {
									tryMovingNorth (ref direction, ref velocity);
								}
							}
						}	
					}

					// move on the y axis
					else {
						if (dy > 0) {
							if (!tryMovingSouth (ref direction, ref velocity)) {
								if (dx > 0) {
									tryMovingWest (ref direction, ref velocity);
								} else {
									tryMovingEast (ref direction, ref velocity);
								}
							}
						} 
						else {
							if (!tryMovingNorth (ref direction, ref velocity)) {
								if (dx > 0) {
									tryMovingWest (ref direction, ref velocity);
								} else {
									tryMovingEast (ref direction, ref velocity);
								}
							}
						}
					}
				} 
				else 
				{
					// If there would be a collision, probabilistically change the animal's direction (can't stay in same direction)
					// For now, all objects are treated as walls
					getAdjacentTileCoords(ref tileCoords, direction);
					if (!GameManager.map.mapTile [(int)tileCoords.x, (int)tileCoords.y].isWalkable)
					{
						// Populate the list of valid directions
						if (direction != Direction.North) {
							getAdjacentTileCoords (ref tileCoords, Direction.North);
							if (GameManager.map.mapTile [(int)tileCoords.x, (int)tileCoords.y].isWalkable)
								validDirections.Add (Direction.North);
						}

						if (direction != Direction.South) {
							getAdjacentTileCoords (ref tileCoords, Direction.South);
							if (GameManager.map.mapTile [(int)tileCoords.x, (int)tileCoords.y].isWalkable)
								validDirections.Add (Direction.South);
						}

						if (direction != Direction.East) {
							getAdjacentTileCoords (ref tileCoords, Direction.East);
							if (GameManager.map.mapTile [(int)tileCoords.x, (int)tileCoords.y].isWalkable)
								validDirections.Add (Direction.East);
						}

						if (direction != Direction.West) {
							getAdjacentTileCoords (ref tileCoords, Direction.West);
							if (GameManager.map.mapTile [(int)tileCoords.x, (int)tileCoords.y].isWalkable)
								validDirections.Add (Direction.West);
						}

						// Randomly select an element from the list of valid directions, then empty the list
						if (validDirections.Count > 0) {
							randDir = Random.Range (0, validDirections.Count);
							validDirections.Clear ();
						} else
						{
							Debug.Log ("There are no valid directions for the animal to turn.");
							randDir = 4;
						}

						// Assign the new velocity and direction based on the randomly generated number
						switch (randDir) {
						case 0:
							direction = Direction.North;
							velocity.x = 0;
							velocity.y = stepSize * Time.deltaTime;
							break;
				
						case 1:
							direction = Direction.South;
							velocity.x = 0;
							velocity.y = -stepSize * Time.deltaTime;
							break;
				
						case 2:
							direction = Direction.East;
							velocity.x = stepSize * Time.deltaTime;
							velocity.y = 0;
							break;
				
						case 3:
							direction = Direction.West;
							velocity.x = -stepSize * Time.deltaTime;
							velocity.y = 0;
							break;

						case 4:
							velocity = Vector2.zero;
							break;
						}
					}
				}
			}

			// Regardless of whether or not the velocity has changed, update the position
			transform.position += (Vector3)velocity;
		}
	}

	// Checks to see if there are any lures within the attraction radius.
	// If the animal is already attracted by a lure, we only need to check
	// for new lures that are closer (for efficiency)
	void checkForAttraction(ref bool attracted, ref Collider2D closestLure, ref float distToClosestLure){

		//if (!attracted)
			overlapCheckRadius = attractionRadius;
		//else
			//overlapCheckRadius = distToClosestLure;
		
		attracted = false;
		distToClosestLure = attractionRadius + 50.0f;
		Collider2D[] overlapList = Physics2D.OverlapCircleAll (transform.position, overlapCheckRadius);
		if (overlapList.Length != 0) {
			foreach (Collider2D collider in overlapList) {
				if (collider.gameObject.GetComponent<LureController> ()) {

					// set the animal's attraction
					if (!attracted)
						attracted = true;
				
					// if the attraction radius is nonempty, find the closest lure
					if (Vector2.Distance (transform.position, collider.transform.position) < distToClosestLure) {
						closestLure = collider;
						distToClosestLure = Vector2.Distance (transform.position, collider.transform.position);
					
					
					}

				}
			}
		}
	}

	// if there is no collision, set the new velocity and direction
	bool tryMovingWest(ref Direction direction, ref Vector2 velocity){

		tempVector.x = -stepSize * Time.deltaTime;
		tempVector.y = 0;
		if (!Physics2D.OverlapPoint (transform.position + (Vector3)tempVector)){
			direction = Direction.West;
			velocity = tempVector;
			return true;
		}
		return false;
		// TODO: (maybe trigger frustration graphic here)
	}

	// if there is no collision, set the new velocity and direction
	bool tryMovingEast(ref Direction direction, ref Vector2 velocity){

		tempVector.x = stepSize * Time.deltaTime;
		tempVector.y = 0;
		if (!Physics2D.OverlapPoint (transform.position + (Vector3)tempVector)){
			direction = Direction.East;
			velocity = tempVector;
			return true;
		}
		return false;
		// TODO: (maybe trigger frustration graphic here)
	}

	// if there is no collision, set the new velocity and direction
	bool tryMovingSouth(ref Direction direction, ref Vector2 velocity){

		tempVector.x = 0.0f;
		tempVector.y = -stepSize * Time.deltaTime;
		if (!Physics2D.OverlapPoint (transform.position + (Vector3)tempVector)){
			direction = Direction.South;
			velocity = tempVector;
			return true;
		}
		return false;
		// TODO: (maybe trigger frustration graphic here)
	}

	// if there is no collision, set the new velocity and direction
	bool tryMovingNorth(ref Direction direction, ref Vector2 velocity){

		tempVector.x = 0.0f;
		tempVector.y = stepSize * Time.deltaTime;
		if (!Physics2D.OverlapPoint (transform.position + (Vector3)tempVector)){
			direction = Direction.North;
			velocity = tempVector;
			return true;
		}
		return false;
		// TODO: (maybe trigger frustration graphic here)
	}

	void getAdjacentTileCoords(ref Vector2 tileCoords, Direction direction )
	{
		tileCoords = GameManager.map.GetTileCoordFromWorldPos (transform.position);
		switch(direction)
		{
		case Direction.North:
			tileCoords.y++;
			break;
		case Direction.South:
			tileCoords.y--;
			break;
		case Direction.East:
			tileCoords.x++;
			break;
		case Direction.West:
			tileCoords.x--;
			break;
		}
	}

	#endregion
}
