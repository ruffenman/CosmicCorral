using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {	
	
	public class EventDispatcher
	{
		public Action GameStarted;
		public Action GameCompleted;
	}

	public static MapManager map { get { return s_instance.m_map; } }
	public static ToolBarManager toolbar { get { return s_instance.m_toolbar; } }
	public static SoundManager soundManager { get { return s_instance.m_soundManager; } }
	public static UIMenus uiMenus { get { return s_instance.m_uiMenus; } }
	public static EventDispatcher eventDispatcher { get { return s_instance.m_eventDispatcher; } }
	public static LevelManager levelManager { get { return s_instance.m_levelManager; } }

	private void Awake()
	{
		if(s_instance == null)
		{
			s_instance = this;
			DontDestroyOnLoad (this.gameObject);

			m_eventDispatcher = new EventDispatcher();
			
			m_map = Instantiate<MapManager>(m_MapManagerPrefab);
			m_map.transform.SetParent(transform, false);
			m_toolbar = Instantiate<ToolBarManager>(m_TooBarPrefab);
			m_toolbar.transform.SetParent(transform, false);
			m_soundManager = Instantiate<SoundManager>(m_soundManagerPrefab);
			m_soundManager.transform.SetParent(transform, false);
			m_uiMenus = Instantiate<UIMenus>(m_uiMenusPrefab);
			m_uiMenus.transform.SetParent(transform, false);
			m_levelManager = Instantiate<LevelManager>(m_levelManagerPrefab);
			m_levelManager.transform.SetParent(transform, false);
			m_levelManager.LevelStarted += OnLevelStarted;
			m_levelManager.LevelEnded += OnLevelEnded;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		// Fade out scrim then do start screen
		m_uiMenus.SetShowScrim(false, 1);
		Action onHidden;
		onHidden = ()=> {
			m_uiMenus.OnScrimHidden -= onHidden;
			StartCoroutine(DoStartScreen());
		};
		m_uiMenus.OnScrimHidden += onHidden;
		// Make sure start screen is shown before scrim fades in
		m_uiMenus.ShowMenu(UIMenus.MenuScreen.StartScreen, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator DoStartScreen()
	{
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

		if(Application.loadedLevel == 0)
		{
			m_levelManager.StartLevel(1);
		}
	}

	private void StartGameplay()
	{
		Debug.Log("Gameplay started!");
		if(m_eventDispatcher.GameStarted != null) m_eventDispatcher.GameStarted();
	}

	private void OnLevelStarted()
	{
		Debug.LogFormat("Level Started!: {0}s", m_levelManager.currentLevelIndex);
	}
	
	private void OnLevelEnded()
	{
		bool wasCompleted = m_levelManager.isCompleted;
		Debug.LogFormat("Level Ended!: {0} success: {1}", m_levelManager.currentLevelIndex, wasCompleted);

		StartCoroutine(StartNextLevel(wasCompleted));
	}

	private IEnumerator StartNextLevel(bool wasCompleted)
	{
		// On next frame, after other event handlers have been called
		yield return null;
		if(wasCompleted)
		{
			m_levelManager.StartLevel(m_levelManager.currentLevelIndex + 1);
		}
		else
		{
			m_levelManager.StartLevel(m_levelManager.currentLevelIndex);
		}
	}

	[SerializeField]
	private MapManager m_MapManagerPrefab;
	[SerializeField]
	private ToolBarManager m_TooBarPrefab;
	[SerializeField]
	private SoundManager m_soundManagerPrefab;
	[SerializeField]
	private UIMenus m_uiMenusPrefab;
	[SerializeField]
	private LevelManager m_levelManagerPrefab;

	private MapManager m_map;
	private ToolBarManager m_toolbar;
	private SoundManager m_soundManager;
	private UIMenus m_uiMenus;
	private EventDispatcher m_eventDispatcher;
	private LevelManager m_levelManager;

	private static GameManager s_instance;
}
