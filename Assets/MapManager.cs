using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {


	public Vector2 GetTileCoordFromWorldPos(Vector3 worldPos)
	{
		Vector2 coords = new Vector2();
		coords.x = Mathf.Floor(worldPos.x);
		float mapBottom = -height;
		coords.y = Mathf.Floor(worldPos.y - mapBottom);
		return coords;
	} 

	public Vector3 GetWorldPositionFromTileCoord(Vector2 coord)
	{		
		float mapBottom = -height;
		Vector3 worldPos = new Vector3(coord.x + 0.5f, mapBottom + coord.y + 0.5f);
		return worldPos;
	}

	public bool DropToolbarItemAtPosition(ToolBarItem toolbarItem, Vector3 worldPos)
	{
		Vector2 tileCoord = GameManager.map.GetTileCoordFromWorldPos(worldPos);
		Vector3 tileWorldPos = GameManager.map.GetWorldPositionFromTileCoord(tileCoord);
		Instantiate(toolbarItem.gameplayPrefab, tileWorldPos, Quaternion.identity);
		Debug.LogFormat("Toolbar item added to map at tile: {0},{1} pos: {2},{3}", tileCoord.x, tileCoord.y, tileWorldPos.x, tileWorldPos.y);
		//TODO Add a check for tile 
		return true;
	}

	public int width {get{return m_sceneTiledMap.NumTilesWide;}}
	public int height {get{return m_sceneTiledMap.NumTilesHigh;}}

	private void Awake()
	{
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		GameManager.levelManager.LevelStarted += OnLevelStarted;
	}

	private void OnLevelStarted()
	{		
		m_sceneTiledMap = GameObject.FindObjectOfType<Tiled2Unity.TiledMap>();
		if(m_sceneTiledMap == null) Debug.LogError("No TiledMap was found in the scene");


		// Initialize tile array
		Vector3 tileCenter;
		mapTile = new Tile[((Tiled2Unity.TiledMap)(m_sceneTiledMap)).NumTilesWide, ((Tiled2Unity.TiledMap)m_sceneTiledMap).NumTilesHigh];
		for (int x = 0; x < ((Tiled2Unity.TiledMap)m_sceneTiledMap).NumTilesWide; x++) {
			for (int y = 0; y < ((Tiled2Unity.TiledMap)m_sceneTiledMap).NumTilesHigh; y++){
				mapTile [x,y] = new Tile();
				mapTile [x,y].type = Tile.TileType.NotDoor;
			
				// configure special tiles
				tileCenter = GetWorldPositionFromTileCoord(new Vector2(x, y));
				Collider2D[] overlapList = Physics2D.OverlapPointAll (tileCenter);
				if (overlapList.Length != 0) {
					foreach (Collider2D collider in overlapList) {

						// if position maps to a door, make it a door
						if (collider.gameObject.GetComponents<Door> ().Length!=0){
							mapTile [x,y].type = Tile.TileType.GenericDoor;
						}
						// if position maps to another collision, make it unwalkable
						else{
							mapTile [x,y].isWalkable = false;
						}
					}
				}
			}
		}
	}

	public Tile[,] mapTile; 
	private Tiled2Unity.TiledMap m_sceneTiledMap;
}
