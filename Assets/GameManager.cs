using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static MapManager map { get { return s_instance.m_map; } }
	public static ToolBarManager toolbar { get { return s_instance.m_toolbar; } }
	public static SoundManager soundManager { get { return s_instance.m_soundManager; } }

	private void Awake()
	{
		if(s_instance == null)
		{
			s_instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);

		m_map = Instantiate (m_MapManagerPrefab);
		m_toolbar = Instantiate (m_TooBarPrefab);
		m_soundManager = Instantiate<SoundManager>(m_soundManagerPrefab);
		DontDestroyOnLoad(m_soundManager.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[SerializeField]
	private MapManager m_MapManagerPrefab;
	[SerializeField]
	private ToolBarManager m_TooBarPrefab;
	[SerializeField]
	private SoundManager m_soundManagerPrefab;

	private MapManager m_map;
	private ToolBarManager m_toolbar;
	private SoundManager m_soundManager;

	private static GameManager s_instance;
}
