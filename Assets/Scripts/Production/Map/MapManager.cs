using MapTools;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{
	public MapConfig mapObject;

	public MapInfo MapInfo { get; private set; }
	public IEnumerable<Vector2Int> Path { get; private set; }
	private IPathFinder pathFinder;
	private IList<Vector3> m_WorldPath = null;

	public IList<Vector3> WorldPath
	{
		get
		{
			if (m_WorldPath != null)
			{ return m_WorldPath; }
			else
			{
				IList<Vector3> m_WorldPath = new List<Vector3>();
				foreach (Vector2Int vec in Path)
				{
					m_WorldPath.Add(mapObject.LocalToWorld(vec));
				}
				return m_WorldPath;
			}
		}
	}

	private void Awake()
	{
		MapInfo = MapParser.Parse(mapObject.textFile.text);
		MapBuilder.Build(MapInfo.Tiles, mapObject, transform);

		pathFinder = new Dijkstra(MapInfo.GetWalkable());
		Path = pathFinder.FindPath(MapInfo.Start.Value, MapInfo.End.Value);
	}



}
