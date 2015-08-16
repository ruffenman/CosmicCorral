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
	#endregion

	#region methods
	// Use this for initialization
	void Start () {
		attracted = false;
		direction = Direction.North;
		velocity = new Vector2 (0.0f, 0.03f);
		validDirections = new ArrayList ();
		tempVector = Vector2.zero;
		distToClosestLure = attractionRadius + 50.0f;
	}

	// Update is called once per frame
	// TODO: collision handling for different types of colliders (walls, escape pods, lures)
	void Update () {


		if (GameManager.levelManager.levelLoaded) {
			// check to see if the animal is in a door
			Vector2 tileCoords = GameManager.map.GetTileCoordFromWorldPos (transform.position + (Vector3)velocity);
			if (GameManager.map.mapTile [(int)tileCoords.x, (int)tileCoords.y].type == Tile.TileType.GenericDoor)
			{
				velocity = Vector2.zero;
			} 
			// Check to see if the animal has hit a hazard
			else if (GameManager.map.mapTile [(int)tileCoords.x, (int)tileCoords.y].type == Tile.TileType.Hazard)
			{
				// player loses a life
				GameManager.levelManager.numLives--;
				if(GameManager.levelManager.numLives == 0)
					GameManager.levelManager.Lose ();

				// animal is removed from the game

				// spawn a new animal

			}

			else {
				checkForAttraction (ref attracted, ref closestLure, ref distToClosestLure);

				if (attracted) {

					// try to move towards the lure 
					// otherwise move along the opposite axis if there is a dx / dy or just keep moving straight
					dx = transform.position.x - closestLure.transform.position.x;
					dy = transform.position.y - closestLure.transform.position.y;

					if (Mathf.Abs (dx) < 0.5 && Mathf.Abs (dy) < 0.5)
						Destroy (closestLure.gameObject);

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
						} else {
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
						} else {
							if (!tryMovingNorth (ref direction, ref velocity)) {
								if (dx > 0) {
									tryMovingWest (ref direction, ref velocity);
								} else {
									tryMovingEast (ref direction, ref velocity);
								}
							}
						}
					}
				} else {
	
					// If there would be a collision, probabilistically change the animal's direction (can't stay in same direction)
					// For now, all objects are treated as walls

					Collider2D[] overlapList = Physics2D.OverlapPointAll (transform.position + (Vector3)velocity);
					if (overlapList.Length != 0) {
						foreach (Collider2D collider in overlapList) {
							if (collider.gameObject.GetComponents<Door> ().Length!=0){
					

								// Populate the list of valid directions
								if (direction != Direction.North) {
									tempVector.x = 0.0f;
									tempVector.y = stepSize * Time.deltaTime;
									if (!Physics2D.OverlapPoint (transform.position + (Vector3)tempVector))
										validDirections.Add (Direction.North);
								}

								if (direction != Direction.South) {
									tempVector.x = 0;
									tempVector.y = -stepSize * Time.deltaTime;
									if (!Physics2D.OverlapPoint (transform.position + (Vector3)tempVector))
										validDirections.Add (Direction.South);
								}

								if (direction != Direction.East) {
									tempVector.x = stepSize * Time.deltaTime;
									tempVector.y = 0;
									if (!Physics2D.OverlapPoint (transform.position + (Vector3)tempVector))
										validDirections.Add (Direction.East);
								}

								if (direction != Direction.West) {
									tempVector.x = -stepSize * Time.deltaTime;
									tempVector.y = 0;
									if (!Physics2D.OverlapPoint (transform.position + (Vector3)tempVector))
										validDirections.Add (Direction.West);
								}

								// Randomly select an element from the list of valid directions, then empty the list
								if (validDirections.Count > 0) {
									randDir = Random.Range (0, validDirections.Count);
									validDirections.Clear ();
								} else
									Debug.Log ("There are no valid directions for the animal to turn.");

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
								}
							}
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

		if (!attracted)
			overlapCheckRadius = attractionRadius;
		else
			overlapCheckRadius = distToClosestLure;
		
		attracted = false;
		Collider2D[] overlapList = Physics2D.OverlapCircleAll (transform.position, overlapCheckRadius);
		if (overlapList.Length != 0) {
			foreach (Collider2D collider in overlapList) {
				if (collider.gameObject.GetComponents<LureController> ().Length!=0) {

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

	#endregion
}
