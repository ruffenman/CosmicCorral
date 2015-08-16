using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ToolBarManager : MonoBehaviour 
{
	#region instance variables
	GraphicRaycaster graphicRaycaster;
	#endregion

	private void Awake()
	{
		m_items = new List<ToolBarItem>(GetComponentsInChildren<ToolBarItem>());
		for(int i = 0; i < m_items.Count; ++i)
		{
			ToolBarItem item = m_items[i];
			item.Selected += OnItemSelected;
			item.Selected += OnItemDeselected;
		}
	}

	// Use this for initialization
	private void Start () {
		DontDestroyOnLoad (gameObject);
		graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
	}
	
	// Update is called once per frame
	private void Update () {

	}

	private void OnItemSelected(ToolBarItem item)
	{
		m_selectedItem = item;
		GameManager.soundManager.PlaySfx(SoundManager.SFX_SELECT_ITEM);
	}
	
	private void OnItemDeselected(ToolBarItem item)
	{

	}

	private List<ToolBarItem> m_items;
	private ToolBarItem m_selectedItem;
}
