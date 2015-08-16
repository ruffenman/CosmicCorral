﻿using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour
{
	public void Win()
	{
		m_isCompleted = true;
		if(LevelEnded != null) LevelEnded();
	}
	
	public void Lose()
	{
		if(LevelEnded != null) LevelEnded();
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
			m_isCompleted = false;
			if(LevelStarted != null) LevelStarted();
		};
		GameManager.uiMenus.OnScrimHidden += onHidden;
	}

	private int m_currentLevelIndex;
	private bool m_isCompleted;
}