using UnityEngine;
using MapTools;

/// <summary>
/// Contains data for spawning map to world
/// </summary>
[CreateAssetMenu(fileName = "MapConfig", menuName = "ScriptableObjects/MapSettingsScriptableObject", order = 1)]
public class MapConfig : ScriptableObject, ITileConfig
{
	public TextAsset textFile;
	[SerializeField] private Vector2Int m_TileSize;
	[SerializeField] private Vector2Int m_SpawnOffset;
	[SerializeField] private MapKeyData[] m_TileToPrefab;

	public MapKeyData[] TileToPrefab { get => m_TileToPrefab; set => m_TileToPrefab = value; }
	public Vector2Int SpawnOffset { get => m_SpawnOffset; set => m_SpawnOffset = value; }
	public Vector2Int TileSize { get => m_TileSize; set => m_TileSize = value; }

	public Vector3 LocalToWorld(Vector2Int local)
	{
		return new Vector3(SpawnOffset.x + TileSize.x * local.x, 0, SpawnOffset.y + TileSize.y * local.y);
	}

	public Vector2Int WorldToLocal(Vector3 world)
	{
		return new Vector2Int(Mathf.RoundToInt(world.x), Mathf.RoundToInt(world.y));
	}
}
