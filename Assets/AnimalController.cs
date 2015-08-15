using UnityEngine;
using System.Collections;

public class AnimalController : MonoBehaviour {

	#region instance variables
	public enum Direction {North, South, East, West};
	const float stepSize = 0.1f;
	const float attractionRadius = 200;

	// public variables
	[SerializeField]
	private bool attracted;
	[SerializeField]
	private Direction direction;
	[SerializeField]
	private Vector2 velocity;

	// private variables 
	// (reusable temp variables; saving memory)
	private Collider2D[] collisionResults;
	private int randDir;
	private Vector2 tempVector;
	private ArrayList validDirections;
	private Collider2D closestLure;
	private float distToClosestLure;
	private float overlapCheckRadius;
	#endregion

	#region methods
	// Use this for initialization
	void Start () {
		//transform.position = new Vector2 (50.0f, 50.0f);
		attracted = false;
		direction = Direction.North;
		velocity = new Vector2 (0.0f, stepSize);
		collisionResults = new Collider2D[50];
		validDirections = new ArrayList ();
		tempVector = Vector2.zero;
		distToClosestLure = attractionRadius + 50.0f;
	}

	// Update is called once per frame
	// TODO: add lure attraction (later)
	// TODO: collision handling for different types of colliders (walls, escape pods, lures)
	void Update () {

		#region attraction handling

		// check to see if there are any lures within the attraction radius
		// if the animal is already attracted by a lure, we only need to check
		// for new lures that are closer (for efficiency)
		if (!attracted)
			overlapCheckRadius = attractionRadius;
		else
			overlapCheckRadius = distToClosestLure;

		attracted = false;
		collisionResults = new Collider2D[50];
		if(Physics2D.OverlapCircleNonAlloc(transform.position, overlapCheckRadius, collisionResults)!=0){
			foreach (Collider2D collider in collisionResults){
				if (collider.GetComponents<LureController>().Length != 0){

					// set the animal's attraction
					if(!attracted)
						attracted = true;

					// if the attraction radius is nonempty, find the closest lure
					if (Vector2.Distance(transform.position, collider.transform.position) < distToClosestLure){
						closestLure = collider;
						distToClosestLure = Vector2.Distance(transform.position, collider.transform.position);
					}
				}
			}
		}
		#endregion

		#region collision handling
		// If there would be a collision, probabilistically change the animal's direction (can't stay in same direction)
		// For now, all objects are treated as walls
		collisionResults = new Collider2D[50];
		if (Physics2D.OverlapPointNonAlloc (transform.position + (Vector3)velocity, collisionResults) != 0) {

			// Populate the list of valid directions
			if(direction != Direction.North){
				tempVector.x = 0.0f;
				tempVector.y = stepSize * Time.deltaTime;
				collisionResults = new Collider2D[50];
				if (Physics2D.OverlapPointNonAlloc (transform.position + (Vector3)tempVector, collisionResults) != 0)
					validDirections.Add (Direction.North);
			}

			if(direction != Direction.South){
				tempVector.x = 0;
				tempVector.y = -stepSize * Time.deltaTime;
				collisionResults = new Collider2D[50];
				if (Physics2D.OverlapPointNonAlloc (transform.position + (Vector3)tempVector, collisionResults) != 0)
					validDirections.Add (Direction.South);
			}

			if(direction != Direction.East){
				tempVector.x = stepSize * Time.deltaTime;
				tempVector.y = 0;
				collisionResults = new Collider2D[50];
				if (Physics2D.OverlapPointNonAlloc (transform.position + (Vector3)tempVector, collisionResults) != 0)
					validDirections.Add (Direction.East);
			}

			if(direction != Direction.West){
				tempVector.x = -stepSize * Time.deltaTime;
				tempVector.y = 0;
				collisionResults = new Collider2D[50];
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
		#endregion

		// Regardless of whether or not the velocity has changed, update the position
		transform.position += (Vector3)velocity;
	}

	#endregion
}
