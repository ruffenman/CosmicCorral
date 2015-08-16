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
		if(Input.GetMouseButtonDown(0))
		{
			RectTransform panelRect = transform.FindChild("Panel").GetComponent<RectTransform>();
			if(!RectTransformUtility.RectangleContainsScreenPoint(panelRect, (Vector2)Input.mousePosition, null) && m_selectedItem != null && m_selectedItem.count > 0)
			{
				bool success = GameManager.map.DropToolbarItemAtPosition(m_selectedItem, Camera.main.ScreenToWorldPoint(Input.mousePosition));
				if(success)
				{
					m_selectedItem.DecrementCount(1);
					GameManager.soundManager.PlaySfx(SoundManager.SFX_PLACE_ITEM);
				}
				else
				{
					GameManager.soundManager.PlaySfx(SoundManager.SFX_CANCEL_ITEM);
				}
			}
			else
			{
				bool onItem = false;
				foreach(ToolBarItem item in m_items)
				{
					onItem = onItem || RectTransformUtility.RectangleContainsScreenPoint(item.GetComponent<RectTransform>(), (Vector2)Input.mousePosition, null);
				}
				if(!onItem && GameManager.levelManager.levelLoaded && m_selectedItem != null)
				{
					GameManager.soundManager.PlaySfx(SoundManager.SFX_CANCEL_ITEM);
				}
			}
		}
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
