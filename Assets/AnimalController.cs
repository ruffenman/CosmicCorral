using UnityEngine;
using System.Collections;

public class AnimalController : MonoBehaviour {

	public enum Direction {North, South, East, West};
	const int stepSize = 5;

	public Vector2 velocity;
	public Direction direction;

	private Random rng;

	// Use this for initialization
	void Start () {
		// Set initial direction to north
		direction = Direction.North;

		// Set the animal's speed

		// Initialize the random number generator
		rng = new Random();
	}

	// Update is called once per frame
	// TODO: add lure attraction (later)
	void Update () {

		// Check for collisions (tile-based) 
		// If there is a collision, probabilistically change the animal's direction (can't stay in same direction)
		// (use while loop to check if it's a valid direction - invalid directions are the old direction or a new
		// direction that also causes a collision problem)


		// Regardless of whether or not there are any collisions, move the animal forward according to its speed
		switch (direction) {
			case Direction.North:
				velocity.x = 0;
				velocity.y = -stepSize;
				break;

			case Direction.South:
				velocity.x = 0;
				velocity.y = stepSize;
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

		// Update the animal's position
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
	}


}
