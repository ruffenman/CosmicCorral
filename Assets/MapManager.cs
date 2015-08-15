﻿using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {
	public struct TileCoord
	{
		public int x;
		public int y;
	}

	public TileCoord GetTileCoordFromWorldPos(Vector3 worldPos)
	{
		TileCoord coords = new TileCoord();
		coords.x = (int)Mathf.Floor(worldPos.x);
		float mapBottom = -height;
		coords.y = (int)Mathf.Floor(worldPos.y - mapBottom);
		return coords;
	} 

	public Vector3 GetWorldPositionFromTileCoord(TileCoord coord)
	{		
		float mapBottom = -height;
		Vector3 worldPos = new Vector3(coord.x + 0.5f, mapBottom + coord.y + 0.5f);
		return worldPos;
	}

	public void DropToolbarItemAtPosition(GameObject toolbarItem, Vector3 worldPos)
	{

	}

	public int width {get{return m_sceneTiledMap.NumTilesWide;}}
	public int height {get{return m_sceneTiledMap.NumTilesHigh;}}

	private void Awake()
	{
		m_sceneTiledMap = GameObject.FindObjectOfType<Tiled2Unity.TiledMap>();
		if(m_sceneTiledMap == null) Debug.LogError("No TiledMap was found in the scene");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private Tiled2Unity.TiledMap m_sceneTiledMap;
}
