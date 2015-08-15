using UnityEngine;
using System.Collections;

public class AnimalController : MonoBehaviour {

	#region instance variables
	public enum Direction {North, South, East, West};
	const float stepSize = 0.1f;

	// public variables
	public Direction direction;
	public Vector2 velocity;

	// private variables 
	// (reusable temp variables; saving memory)
	private Collider2D[] collisionResults;
	private int randDir;
	private Vector2 tempVector;
	private ArrayList validDirections;
	#endregion

	#region methods
	// Use this for initialization
	void Start () {
		//transform.position = new Vector2 (50.0f, 50.0f);
		direction = Direction.North;
		velocity = new Vector2 (0.0f, stepSize);
		collisionResults = new Collider2D[50];
		validDirections = new ArrayList ();
		tempVector = Vector2.zero;
	}

	// Update is called once per frame
	// TODO: add lure attraction (later)
	// TODO: collision handling for different types of colliders (walls, escape pods, lures)
	void Update () {

		// If there would be a collision, probabilistically change the animal's direction (can't stay in same direction)
		// For now, all objects are treated as walls
		if (Physics2D.OverlapPointNonAlloc (transform.position + (Vector3)velocity, collisionResults) != 0) {

			// Populate the list of valid directions
			if(direction != Direction.North){
				tempVector.x = 0.0f;
				tempVector.y = stepSize * Time.deltaTime;
				if (Physics2D.OverlapPointNonAlloc (transform.position + (Vector3)tempVector, collisionResults) != 0)
					validDirections.Add (Direction.North);
			}

			if(direction != Direction.South){
				tempVector.x = 0;
				tempVector.y = -stepSize * Time.deltaTime;
				if (Physics2D.OverlapPointNonAlloc (transform.position + (Vector3)tempVector, collisionResults) != 0)
					validDirections.Add (Direction.South);
			}

			if(direction != Direction.East){
				tempVector.x = stepSize * Time.deltaTime;
				tempVector.y = 0;
				if (Physics2D.OverlapPointNonAlloc (transform.position + (Vector3)tempVector, collisionResults) != 0)
					validDirections.Add (Direction.East);
			}

			if(direction != Direction.West){
				tempVector.x = -stepSize * Time.deltaTime;
				tempVector.y = 0;
				if (Physics2D.OverlapPointNonAlloc (transform.position + (Vector3)tempVector, collisionResults) != 0)
					validDirections.Add (Direction.West);
			}

			// Randomly select an element from the list of valid directions, then empty the list
			if(validDirections.Count > 0){
				randDir = Random.Range (0, validDirections.Count);
				validDirections.Clear();
			}
			else
				Debug.Log("There are no valid directions for the animal to turn.");

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

		// Regardless of whether or not the velocity has changed, update the position
		transform.position += (Vector3)velocity;
	}
	#endregion
}
