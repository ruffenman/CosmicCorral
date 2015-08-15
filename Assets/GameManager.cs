using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	public static MapManager map { get { return s_instance.m_map; } }
	public static ToolBarManager toolbar { get { return s_instance.m_toolbar; } }
	public static SoundManager soundManager { get { return s_instance.m_soundManager; } }
	public static UIMenus uiMenus { get { return s_instance.m_uiMenus; } }
	public static EventDispatcher eventDispatcher { get { return s_instance.m_eventDispatcher; } }

	public class EventDispatcher
	{
		public Action GameplayStarted;
	}

	private void Awake()
	{
		if(s_instance == null)
		{
			s_instance = this;
			DontDestroyOnLoad (this.gameObject);

			m_eventDispatcher = new EventDispatcher();
			
			m_map = Instantiate (m_MapManagerPrefab);
			m_toolbar = Instantiate (m_TooBarPrefab);
			m_soundManager = Instantiate<SoundManager>(m_soundManagerPrefab);
			DontDestroyOnLoad(m_soundManager.gameObject);
			m_uiMenus = Instantiate<UIMenus>(m_uiMenusPrefab);
			DontDestroyOnLoad(m_uiMenus.gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(DoStartScreen());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator DoStartScreen()
	{
		m_uiMenus.ShowMenu(UIMenus.MenuScreen.StartScreen, 0);

		bool startGamePlay = false;
		while(!startGamePlay)
		{
			yield return null;

			if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
			{
				startGamePlay = true;
			}
		}

		m_uiMenus.HideMenus(0.25f);
	}

	private void StartGameplay()
	{
		Debug.Log("Gameplay started!");
		if(m_eventDispatcher.GameplayStarted != null) m_eventDispatcher.GameplayStarted();
	}

	[SerializeField]
	private MapManager m_MapManagerPrefab;
	[SerializeField]
	private ToolBarManager m_TooBarPrefab;
	[SerializeField]
	private SoundManager m_soundManagerPrefab;
	[SerializeField]
	private UIMenus m_uiMenusPrefab;

	private MapManager m_map;
	private ToolBarManager m_toolbar;
	private SoundManager m_soundManager;
	private UIMenus m_uiMenus;
	private EventDispatcher m_eventDispatcher;

	private static GameManager s_instance;
}
