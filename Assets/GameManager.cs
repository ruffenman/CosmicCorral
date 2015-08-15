using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	private MapManager currentMap;
	private AnimalController animController;
	private ToolBarManager toolbar;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);

		animController = Instantiate (m_AnimalPrefab);
		currentMap = Instantiate (m_MapManagerPrefab);
		toolbar = Instantiate (m_TooBarPrefab);

		graphicRaycaster = new GameObject ("UI_raycaster", typeof(GraphicRaycaster)).GetComponent<GraphicRaycaster>();
		DontDestroyOnLoad (graphicRaycaster.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[SerializeField]
	private MapManager m_MapManagerPrefab;
	[SerializeField]
	private AnimalController m_AnimalPrefab;
	[SerializeField]
	private ToolBarManager m_TooBarPrefab;

	private GraphicRaycaster graphicRaycaster;
}
