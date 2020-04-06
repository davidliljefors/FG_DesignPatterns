using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private MapManager mapManager;
	[SerializeField] private GameObject enemyPrefab = default;

	// Start is called before the first frame update
	void Start()
	{
		Spawn();
	}

	private void Update()
	{
		if(Time.frameCount % 100 == 0)
		{
			Spawn();
		}
	}

	void Spawn()
	{
		GameObject instance = Instantiate(enemyPrefab,
		mapManager.mapObject.LocalToWorld(mapManager.MapInfo.Start.Value),
		Quaternion.identity);
		instance.GetComponent<IEnemy>().Path = new List<Vector2Int>(mapManager.Path);
		instance.GetComponent<Enemy>().MapObject = mapManager.mapObject;
	}
}
