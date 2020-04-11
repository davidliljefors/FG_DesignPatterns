using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System;

namespace MapTools
{
	[System.Serializable]
	public struct MapKeyData
	{
		[SerializeField] private TileType m_Type;
		[SerializeField] private GameObject m_Prefab;

		public TileType Type { get => m_Type; set => m_Type = value; }
		public GameObject Prefab { get => m_Prefab; set => m_Prefab = value; }
	}

	[System.Serializable]
	public class UnitWave
	{
		public UnitWave()
		{
			Units = new Dictionary<UnitType, int>();
		}

		public IDictionary<UnitType, int> Units { get; set; }
	}
	/// <summary>
	/// Contains information of the parsed map
	/// </summary>
	public class MapInfo
	{
		public Vector2Int? Start { get; private set; }
		public Vector2Int? End { get; private set; }
		public TileType[,] Tiles { get; set; }
		public IEnumerable<UnitWave> Units { get; set; }

		private ICollection<Vector2Int> m_Walkable;
		private bool calculatedWalkable = false;

		public MapInfo(TileType[,] tiles, IEnumerable<UnitWave> units)
		{
			Tiles = tiles;
			Units = units;

			m_Walkable = new List<Vector2Int>();

			for (int i = 0; i < tiles.GetLength(0); ++i)
			{
				for (int j = 0; j < tiles.GetLength(1); ++j)
				{
					if (tiles[i, j] == TileType.Start)
					{
						Start = new Vector2Int(i, j);
					}
					if (tiles[i, j] == TileType.End)
					{
						End = new Vector2Int(i, j);
					}
				}
			}
			Assert.IsTrue(Start.HasValue, "No Start found in map!");
			Assert.IsTrue(End.HasValue, "No Start found in map!");
		}

		public IEnumerable<Vector2Int> GetWalkable()
		{
			if (calculatedWalkable)
			{
				return m_Walkable;
			}
			CalculateWalkable();
			return m_Walkable;
		}

		private void CalculateWalkable()
		{
			for (int i = 0; i < Tiles.GetLength(0); ++i)
			{
				for (int j = 0; j < Tiles.GetLength(1); ++j)
				{
					if (TileMethods.IsWalkable(Tiles[i, j]))
					{
						m_Walkable.Add(new Vector2Int(i, j));
					}
				}
			}
		}
	}
}