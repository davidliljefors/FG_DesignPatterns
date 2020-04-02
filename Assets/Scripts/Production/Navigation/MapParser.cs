﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MapTools
{
	[System.Serializable]
	public struct MapKeyData
	{
		public TileType type;
		public GameObject prefab;
	}

	[System.Serializable]
	public struct UnitWave
	{
		List<int> units;
	}

	public class MapInfo
	{
		public TileType[,] tiles;
		public IEnumerable<int> unitCount;
		private List<Vector2Int> walkable;
		private bool calculatedWalkable;
		public Vector2Int? Start { get; private set; }
		public Vector2Int? End { get; private set; }
		public MapInfo(TileType[,] tiles, IEnumerable<int> unitCount)
		{
			this.tiles = tiles;
			this.unitCount = unitCount;

			walkable = new List<Vector2Int>();

			for(int i = 0; i< tiles.GetLength(0); ++i)
			{
				for(int j = 0; j< tiles.GetLength(1); ++j)
				{
					if (tiles[i, j] == TileType.Start)
						Start = new Vector2Int(i,j);
					if (tiles[i, j] == TileType.End)
						End = new Vector2Int(i, j);
				}
			}
			Assert.IsTrue(Start.HasValue, "No Start found in map!");
			Assert.IsTrue(End.HasValue, "No Start found in map!");
		}

		public IEnumerable<Vector2Int> GetWalkable()
		{
			if(calculatedWalkable)
			{
				return walkable;
			}
			CalculateWalkable();
			return walkable;
		}


		private void CalculateWalkable()
		{
			for (int i = 0; i < tiles.GetLength(0); ++i)
			{
				for (int j = 0; j < tiles.GetLength(1); ++j)
				{
					if (TileMethods.IsWalkable(tiles[i, j]))
						walkable.Add(new Vector2Int(i, j));
				}
			}
		}
	}

	public static class MapBuilder
	{
		public static void ConstructMap(MapInfo mapInfo, Vector2Int tileSize, Vector2Int offset , IEnumerable<MapKeyData> mapKeyData)
		{
			Dictionary<TileType, GameObject> m_PrefabsById;
			m_PrefabsById = new Dictionary<TileType, GameObject>();
			foreach (MapKeyData data in mapKeyData)
			{
				m_PrefabsById.Add(data.type, data.prefab);
			}
			ConstructMap(mapInfo, tileSize, offset, m_PrefabsById);
		}

		private static void ConstructMap(MapInfo mapInfo, Vector2Int tileSize, Vector2Int offset, Dictionary<TileType, GameObject> mapKeyData)
		{
			//Todo null checking
			for (int i = 0; i < mapInfo.tiles.GetLength(0); ++i)
			{
				for (int j = 0; j < mapInfo.tiles.GetLength(1); ++j)
				{
					GameObject prefab = mapKeyData[mapInfo.tiles[i, j]];
					GameObject.Instantiate(prefab, new Vector3(offset.x + i * tileSize.x,0, offset.y + j * tileSize.y), Quaternion.identity);
				}
			}
		}
	}

	public static class MapParser
	{
		private static readonly char splitCharacter = '#';
		public static MapInfo ParseMap(string text)
		{
			// Split string into necessary parts
			string mapData = text.Split(splitCharacter)[0];

			string enemyData = text.Split(splitCharacter)[1];

			TileType[,] tiles;

			//Loop over the map layout
			{
				string[] rows = mapData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
				Assert.IsNotNull(rows);
				Assert.IsNotNull(rows[0]);

				tiles = new TileType[rows.Length, rows[0].Length];
				for (int i = 0; i < rows.Length; ++i)
				{
					for (int j = 0; j < rows[i].Length; ++j)
					{
						tiles[i, j] = TileMethods.TypeByChar[rows[i][j]];
					}
				}
			}

			List<int> enemySettings = new List<int>();
			// Loop over the enemy counts
			{
				int enemyCount = 0;
				foreach (char c in enemyData)
				{
					if (c == ' ')
					{
						enemySettings.Add(enemyCount);
					}
					enemyCount *= 10;
					enemyCount += (int)char.GetNumericValue(c);
				}
				if(enemyCount != 0)
				{
					enemySettings.Add(enemyCount);
				}
			}

			// Todo check if map settings works
			MapInfo info = new MapInfo(tiles, enemySettings);

			return info;
		}
	}
}