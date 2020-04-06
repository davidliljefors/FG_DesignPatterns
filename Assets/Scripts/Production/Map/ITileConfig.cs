using UnityEngine;

namespace MapTools
{
	public interface ITileConfig
	{
		Vector2Int SpawnOffset { get; set; }
		Vector2Int TileSize { get; set; }
		MapKeyData[] TileToPrefab { get; set; }
	}
}

