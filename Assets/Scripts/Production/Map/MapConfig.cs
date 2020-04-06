using UnityEngine;
using MapTools;

/// <summary>
/// Contains data for spawning map to world
/// </summary>
[CreateAssetMenu(fileName = "MapConfig", menuName = "ScriptableObjects/MapSettingsScriptableObject", order = 1)]
public class MapConfig : ScriptableObject, ITileConfig
{
	public TextAsset textFile;
	[SerializeField] private Vector2Int tileSize;
	[SerializeField] private Vector2Int spawnOffset;
	[SerializeField] private MapKeyData[] tileToPrefab;

	public MapKeyData[] TileToPrefab { get => tileToPrefab; set => tileToPrefab = value; }
	public Vector2Int SpawnOffset { get => spawnOffset; set => spawnOffset = value; }
	public Vector2Int TileSize { get => tileSize; set => tileSize = value; }



	public Vector3 LocalToWorld(Vector2Int local)
	{
		return new Vector3(SpawnOffset.x + TileSize.x * local.x, 0, SpawnOffset.y + TileSize.y * local.y);
	}
	public Vector2Int WorldToLocal(Vector3 world)
	{
		return new Vector2Int(Mathf.RoundToInt(world.x), Mathf.RoundToInt(world.y));
	}
}
