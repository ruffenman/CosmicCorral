using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMenus : MonoBehaviour {

	public enum MenuScreen
	{
		StartScreen,
		WinScreen,
		LoseScreen,
	}

	public void ShowMenu(MenuScreen menuScreen, float transitionTime = 0.5f)
	{
		if(m_currentMenu != null)
		{
			CanvasGroup screenToFadeOut = m_currentMenu;
			LeanTween.value(m_currentMenu.gameObject, m_currentMenu.alpha, 0, transitionTime)
				.setOnUpdate((float newVal)=>{screenToFadeOut.alpha = newVal;})
				.setOnComplete(()=>{screenToFadeOut.alpha = 0;});
		}

		m_currentMenu = m_menuScreens[(int)menuScreen];
		
		CanvasGroup screenToFadeIn = m_currentMenu;
		LeanTween.value(m_currentMenu.gameObject, m_currentMenu.alpha, 1, transitionTime)
			.setOnUpdate((float newVal)=>{screenToFadeIn.alpha = newVal;})
			.setOnComplete(()=>{screenToFadeIn.alpha = 1;});
	}

	public void HideMenus(float transitionTime = 0.5f)
	{
		for(int i = 0; i < m_menuScreens.Count; ++i)
		{
			CanvasGroup menuScreen = m_menuScreens[i];
			LeanTween.value(menuScreen.gameObject, menuScreen.alpha, 0, transitionTime)
				.setOnUpdate((float newVal)=>{menuScreen.alpha = newVal;})
				.setOnComplete(()=>{menuScreen.alpha = 0;});
		}
		m_currentMenu = null;
	}

	private CanvasGroup m_currentMenu;

	[SerializeField]
	private List<CanvasGroup> m_menuScreens;
}
