using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ToolBarItem : MonoBehaviour {
	[SerializeField]
	public GameObject gameplayPrefab;
	[SerializeField]
	public int count = 10;
	
	public void DecrementCount(int countChange){
		this.count -= countChange;
		m_countText.text = count.ToString();
	}

	public Action<ToolBarItem> Selected;
	public Action<ToolBarItem> Deselected;

	private void Awake()
	{
		m_countText = GetComponentInChildren<Text>();
		m_toggle = GetComponent<Toggle>();
		m_toggle.onValueChanged.AddListener(OnToggled);
	}

	// Use this for initialization
	void Start () {
		m_countText.text = count.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnToggled(bool isOn)
	{
		if(isOn)
		{
			Debug.LogFormat("Selected {0}", gameplayPrefab);
			if(Selected != null) Selected(this);
		}
		else
		{
			Debug.LogFormat("Deselected {0}", gameplayPrefab);
			if(Deselected != null) Deselected(this);
		}
	}

	private Text m_countText;
	private Toggle m_toggle;
}
