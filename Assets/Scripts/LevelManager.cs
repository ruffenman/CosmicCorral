using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour
{
	public int numLives = 3;
	//public int numPossumsRescued = 0;
	//public int numFlamingoesRescued = 0;
	//public int numGatorsRescued = 0;
	public int numAnimalsRescued = 0;
	public bool levelLoaded = false;

	public void Win()
	{
		m_isCompleted = true;
		if(LevelEnded != null) LevelEnded();
		levelLoaded = false;
	}
	
	public void Lose()
	{
		if(LevelEnded != null) LevelEnded();
		levelLoaded = false;
	}
	
	public void StartLevel(int levelIndex)
	{
		GameManager.uiMenus.SetShowScrim(true);
		Action onShown;
		onShown = ()=>{
			GameManager.uiMenus.OnScrimShown -= onShown;
			Application.LoadLevel(levelIndex);
		};
		GameManager.uiMenus.OnScrimShown += onShown;
	}
	
	public int currentLevelIndex {get{return m_currentLevelIndex;}}
	public bool isCompleted {get{return m_isCompleted;}}

	public Action LevelStarted;
	public Action LevelEnded;

	private void Awake()
	{		
		DontDestroyOnLoad(gameObject);
	}

	private void OnLevelWasLoaded(int levelIndex)
	{

		GameManager.uiMenus.SetShowScrim(false);
		Action onHidden;
		onHidden = ()=>{
			GameManager.uiMenus.OnScrimHidden -= onHidden;
			m_currentLevelIndex = levelIndex;
			m_isCompleted = false;
			levelLoaded = true;
			if(LevelStarted != null) LevelStarted();
		};
		GameManager.uiMenus.OnScrimHidden += onHidden;
	}



	private int m_currentLevelIndex;
	private bool m_isCompleted;
}
