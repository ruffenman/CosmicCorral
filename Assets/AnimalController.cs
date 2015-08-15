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
	#endregion

	#region methods
	// Use this for initialization
	void Start () {
		transform.position = new Vector2 (50.0f, 50.0f);
		direction = Direction.North;
		velocity = new Vector2 (0.0f, stepSize);
		collisionResults = new Collider2D[50];
	}

	// Update is called once per frame
	// TODO: add lure attraction (later)
	// TODO: collision handling for different types of colliders (walls, escape pods, lures)
	void Update () {

		// Check for collisions
		// For now, all objects are treated as walls
		while (Physics2D.OverlapPointNonAlloc (transform.position + (Vector3)velocity, collisionResults) != 0) {

			// If there would be a collision, probabilistically change the animal's direction (can't stay in same direction)
			randDir = Random.Range (0, 3);
			while (randDir==(int)direction)
				randDir = Random.Range (0, 3);
			
			switch (randDir) {
			case 0:
				direction = Direction.North;
				break;
				
			case 1:
				direction = Direction.South;
				break;
				
			case 2:
				direction = Direction.East;
				break;
				
			case 3:
				direction = Direction.West;
				break;
			}
		
			// Set the velocity according to the new direction
			switch (direction) {
				case Direction.North:
					velocity.x = 0;
					velocity.y = stepSize;
					break;

				case Direction.South:
					velocity.x = 0;
					velocity.y = -stepSize;
					break;

				case Direction.East:
					velocity.x = stepSize;
					velocity.y = 0;
					break;

				case Direction.West:
					velocity.x = -stepSize;
					velocity.y = 0;
					break;
			}
		}

		// Regardless of whether or not the velocity has changed, update the position
		transform.position += (Vector3)velocity;
	}
	#endregion
}
