using MapTools;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapTest : MonoBehaviour
{
	[SerializeField] private MapKeyData[] prefabToTile;
	[SerializeField] private TextAsset mapFile;

	private MapInfo mapInfo;
	private Dijkstra dijkstra;

	private Vector2Int offset;
	private Vector2Int tileSize;
	private IEnumerable<Vector2Int> path;

	void Start()
	{
		offset = new Vector2Int(10, 10);
		tileSize = new Vector2Int(2, 2);
		mapInfo = MapParser.ParseMap(mapFile.text);
		MapBuilder.ConstructMap(mapInfo, tileSize, offset, prefabToTile);

		dijkstra = new Dijkstra(mapInfo.GetWalkable());
		path = dijkstra.FindPath(mapInfo.Start.Value, mapInfo.End.Value);
	}

	private void OnDrawGizmosSelected()
	{
		if (mapInfo != null)
		{
			Gizmos.color = Color.red;
			foreach (Vector2Int item in mapInfo.GetWalkable())
			{
				Gizmos.DrawWireSphere(new Vector3(item.x * tileSize.x + offset.x, 0, item.y * tileSize.y + offset.y), 0.5f);
			}

			foreach (var item in path)
			{
				Debug.Log(mapInfo.Start);
				Debug.Log(mapInfo.End);
				Gizmos.color = Color.green;
				Gizmos.DrawCube(new Vector3(item.x * tileSize.x + offset.x, 0, item.y * tileSize.y + offset.y), Vector3.one);
			}
		}

	}
}
