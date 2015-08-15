using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {
	#region instance variables
	public int columns = 20;
	public int rows = 20;
	
	//Goal objects
	[SerializeField]
	private GameObject gatorPen;
	[SerializeField]
	private GameObject flamingoPen;
	[SerializeField]
	private GameObject possumPen;
	
	//Obsticles 
	[SerializeField]
	private GameObject[] bigAssPuddleOfAntifreeze;
	[SerializeField]
	private GameObject[] openFlame;
	[SerializeField]
	private GameObject[] barrels;
	
	/*public struct TileProperty {
		int movement;

	};*/
	#endregion

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
