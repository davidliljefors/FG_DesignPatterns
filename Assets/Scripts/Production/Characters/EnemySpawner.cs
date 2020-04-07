using UnityEngine;
using System.Collections.Generic;
using MapTools;
using Tools;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private MapManager m_MapManager = default;
	[SerializeField] private GameObject m_EnemyPrefab = default;
	[SerializeField] private uint m_InitialPoolSize = 25;
	private IList<UnitWave> m_SpawnWaves;
	private GameObjectPool m_EnemyPool;

	// Start is called before the first frame update
	void Start()
	{
		m_EnemyPool = new GameObjectPool(m_InitialPoolSize, m_EnemyPrefab, 1, transform);
		m_SpawnWaves = new List<UnitWave>(m_MapManager.MapInfo.Units);
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
		GameObject instance = m_EnemyPool.Rent(true);
		instance.transform.position = m_MapManager.mapObject.LocalToWorld(m_MapManager.MapInfo.Start.Value);
		instance.transform.rotation = Quaternion.identity;

		instance.GetComponent<IEnemy>().Path = m_MapManager.WorldPath;
		instance.GetComponent<IResettable>().Reset();

	}
}
