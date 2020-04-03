using System.Collections.Generic;
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
	public class UnitWave
	{
		public List<int> units;
		public UnitWave()
		{
			units = new List<int>();
		}
	}

	public class MapInfo
	{
		public TileType[,] tiles;
		public IEnumerable<UnitWave> units;
		private List<Vector2Int> walkable;
		private bool calculatedWalkable;
		public Vector2Int? Start { get; private set; }
		public Vector2Int? End { get; private set; }
		public MapInfo(TileType[,] tiles, IEnumerable<UnitWave> units)
		{
			this.tiles = tiles;
			this.units = units;

			walkable = new List<Vector2Int>();

			for (int i = 0; i < tiles.GetLength(0); ++i)
			{
				for (int j = 0; j < tiles.GetLength(1); ++j)
				{
					if (tiles[i, j] == TileType.Start)
						Start = new Vector2Int(i, j);
					if (tiles[i, j] == TileType.End)
						End = new Vector2Int(i, j);
				}
			}
			Assert.IsTrue(Start.HasValue, "No Start found in map!");
			Assert.IsTrue(End.HasValue, "No Start found in map!");
		}

		public IEnumerable<Vector2Int> GetWalkable()
		{
			if (calculatedWalkable)
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
		public static void ConstructMap(MapInfo mapInfo, Vector2Int tileSize, Vector2Int offset, IEnumerable<MapKeyData> mapKeyData)
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
					GameObject.Instantiate(prefab, new Vector3(offset.x + i * tileSize.x, 0, offset.y + j * tileSize.y), Quaternion.identity);
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
				string[] rows = mapData.Split(new char[] { '\n', (char)13 }, System.StringSplitOptions.RemoveEmptyEntries);
				Assert.IsNotNull(rows);
				Assert.IsNotNull(rows[0]);

				tiles = new TileType[rows.Length, rows[0].Length];

				for (int i = 0; i < rows.Length; ++i)
				{
					for (int j = 0; j < rows[i].Length; ++j)
					{
						if(TileMethods.TypeByChar.TryGetValue(rows[i][j], out TileType tile))
						{
							tiles[i, j] = tile;
						}
						else
						{
							// Question to Ederic, thorw Exception or Debug.LogError?
							Debug.LogError("Unexpected Map format!");
							throw new System.InvalidOperationException("Cannot parse map data");
						}
					}
				}
			}

			List<UnitWave> unitWaves = new List<UnitWave>();
			// Loop over the enemy counts
			{
				string[] rows = enemyData.Split(new char[] { '\n', (char)13 }, System.StringSplitOptions.RemoveEmptyEntries);

				// One Wave per line
				for (int i = 0; i < rows.Length; ++i)
				{
					string[] unitCounts = rows[i].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
					UnitWave wave = new UnitWave();

					foreach (string unitCount in unitCounts)
					{
						if (int.TryParse(unitCount, out int result))
						{
							wave.units.Add(int.Parse(unitCount));
						}
						else
						{
							// Question to Ederic, thorw Exception or Debug.LogError?
							Debug.LogError("Unexpected Map format!");
							throw new System.InvalidOperationException("Cannot parse map data");
						}
					}
					unitWaves.Add(wave);
				}
			}

			// Todo check if map settings works
			MapInfo info = new MapInfo(tiles, unitWaves);

			return info;
		}
	}
}