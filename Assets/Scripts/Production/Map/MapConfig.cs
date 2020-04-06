using UnityEngine;
using MapTools;

/// <summary>
/// Contains data for spawning map to world
/// </summary>
[CreateAssetMenu(fileName = "MapConfig", menuName = "ScriptableObjects/MapSettingsScriptableObject", order = 1)]
public class MapConfig : ScriptableObject
{
	public TextAsset textFile;
	public Vector2Int spawnOffset;
	public Vector2Int tileSize;
	public MapKeyData[] tileToPrefab = default;

	public Vector3 LocalToWorld(Vector2Int local)
	{
		return new Vector3(spawnOffset.x + tileSize.x * local.x, 0, spawnOffset.y + tileSize.y * local.y);
	}
	public Vector2Int WorldToLocal(Vector3 world)
	{
		return new Vector2Int(Mathf.RoundToInt(world.x), Mathf.RoundToInt(world.y));
	}
}
