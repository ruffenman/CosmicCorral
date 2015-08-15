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
		eventData.selectedObject = eventData.pointerCurrentRaycast.gameObject;
		Debug.Log("Clicked on something");
	}

	public void OnBeginDrag(PointerEventData eventData){
		Debug.Log("Dragging");
	}

	public void OnEndDrag(PointerEventData eventData){
		Debug.Log("Dragging has stopped");
	}

	public void OnDrag(PointerEventData eventData){
		eventData.selectedObject.transform.position = new Vector3 (eventData.position.x, eventData.position.y);
		Debug.Log("Dragging Has started");
	}

	public void OnDrop(PointerEventData eventData){
		Vector3 droppedAt = Camera.main.ScreenToWorldPoint (eventData.selectedObject.transform.position);
		GameManager.map.DropToolbarItemAtPosition(eventData.selectedObject.gameObject, droppedAt); 
		Debug.Log ("Dropped drug thing");
	}
}
