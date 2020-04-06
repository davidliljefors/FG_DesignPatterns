using System.Collections.Generic;
using UnityEngine;

namespace MapTools
{
	public static class MapBuilder
	{
		public static void Build(TileType[,] tiles, ITileConfig config)
		{
			Dictionary<TileType, GameObject> m_PrefabsById;
			m_PrefabsById = new Dictionary<TileType, GameObject>();
			foreach (MapKeyData data in config.TileToPrefab)
			{
				m_PrefabsById.Add(data.type, data.prefab);
			}
			Build(tiles, config.TileSize, config.SpawnOffset, m_PrefabsById);
		}

		private static void Build(TileType[,] tiles, Vector2Int tileSize, Vector2Int offset, Dictionary<TileType, GameObject> mapKeyData)
		{
			//Todo null checking
			for (int i = 0; i < tiles.GetLength(0); ++i)
			{
				for (int j = 0; j < tiles.GetLength(1); ++j)
				{
					GameObject prefab = mapKeyData[tiles[i, j]];
					GameObject.Instantiate(prefab, new Vector3(offset.x + i * tileSize.x, 0, offset.y + j * tileSize.y), Quaternion.identity);
				}
			}
		}
	}
}

