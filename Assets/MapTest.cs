using MapTools;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{
	[SerializeField] private GameObject[] prefabs = default;
	[SerializeField] private TileType[] types = default;

	MapInfo mapInfo;
	Dijkstra dijkstra;

	private Vector2Int offset;
	private Vector2Int tileSize;
	private IEnumerable<Vector2Int> path;

	void Start()
	{
		TextAsset txt = Resources.Load("MapSettings/map_2") as TextAsset;
		List<MapKeyData> mapKeyData = new List<MapKeyData>();
		for (int i = 0; i < prefabs.Length; ++i)
		{
			mapKeyData.Add(new MapKeyData(types[i], prefabs[i]));
		}
		offset = new Vector2Int(10, 10);
		tileSize = new Vector2Int(2, 2);
		mapInfo = MapParser.ParseMap(txt.text);
		MapBuilder.ConstructMap(mapInfo, tileSize, offset, mapKeyData);


		dijkstra = new Dijkstra(mapInfo.GetWalkable());
		path = dijkstra.FindPath(mapInfo.Start.Value, mapInfo.End.Value);
	}

	private void OnDrawGizmosSelected()
	{

		if (mapInfo != null)
		{
			Gizmos.color = Color.red;
			foreach (var item in mapInfo.GetWalkable())
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
