using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

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
	/// <summary>
	/// Contains information of the parsed map
	/// </summary>
	public class MapInfo
	{
		public TileType[,] tiles;
		public IEnumerable<UnitWave> units;
		public Vector2Int? Start { get; private set; }
		public Vector2Int? End { get; private set; }

		private ICollection<Vector2Int> walkable;
		private bool calculatedWalkable = false;

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
}