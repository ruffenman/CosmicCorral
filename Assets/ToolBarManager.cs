using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ToolBarManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
							  IDropHandler, IPointerDownHandler, IPointerEnterHandler {
	#region instance variables
	GraphicRaycaster graphicRaycaster;
	#endregion

	// Use this for initialization
	void Start () {
		graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
		DontDestroyOnLoad (graphicRaycaster.gameObject);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnPointerEnter(PointerEventData eventData){
		Debug.Log ("Mouse is over something");
	}

	public void OnPointerDown(PointerEventData eventData){
		Debug.Log("Clicked on something");
	}

	public void OnBeginDrag(PointerEventData eventData){
		Debug.Log("Dragging");
	}

	public void OnEndDrag(PointerEventData eventData){
		Debug.Log("Dragging has stopped");
	}

	public void OnDrag(PointerEventData eventData){
		Debug.Log("Dragging Has started");
	}

	public void OnDrop(PointerEventData eventData){
		Debug.Log ("Dropped drug thing");
	}
}
