using MapTools;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{
	public MapConfig mapObject;

	public MapInfo MapInfo { get; private set; }
	public IEnumerable<Vector2Int> Path { get; private set; }
	
	private IPathFinder pathFinder;

	private void Awake()
	{
		MapInfo = MapParser.Parse(mapObject.textFile.text);	
		MapBuilder.Build(MapInfo.tiles, mapObject);

		pathFinder = new Dijkstra(MapInfo.GetWalkable());
		Path = pathFinder.FindPath(MapInfo.Start.Value, MapInfo.End.Value);
	}

}
