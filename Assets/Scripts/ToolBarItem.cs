using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToolBarItem : MonoBehaviour {
	[SerializeField]
	GameObject prefab;
	[SerializeField]
	int count = 10;

	public Vector2 initialPos { get; set;}

	// Use this for initialization
	void Start () {
		Text countText = this.GetComponentInChildren<Text>();
		countText.text = count.ToString();

		initialPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DecrementCount(int countChange){
		Text countText = this.GetComponentInChildren<Text>();
		this.count -= countChange;
		countText.text = count.ToString();
	}


}
