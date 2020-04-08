using UnityEngine;
using System.Collections.Generic;
using MapTools;
using Tools;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private MapManager m_MapManager = default;
	[SerializeField] private GameObject m_EnemyPrefab = default;
	[SerializeField] private uint m_InitialPoolSize = 25;
	[SerializeField] private float m_SpawnSpeed = 3f;
	private IList<UnitWave> m_SpawnWaves;
	private GameObjectPool m_EnemyPool;

	private Coroutine m_SpawnRoutine;
	// Start is called before the first frame update
	void Start()
	{
		m_EnemyPool = new GameObjectPool(m_InitialPoolSize, m_EnemyPrefab, 1, transform);
		m_SpawnWaves = new List<UnitWave>(m_MapManager.MapInfo.Units);
		m_SpawnRoutine = StartCoroutine(Spawn());
	}

	private IEnumerator Spawn()
	{
		while (true)
		{
			GameObject instance = m_EnemyPool.Rent(false);
			instance.transform.position = m_MapManager.mapObject.LocalToWorld(m_MapManager.MapInfo.Start.Value);
			instance.transform.rotation = Quaternion.identity;
			instance.GetComponent<IEnemy>().Path = m_MapManager.WorldPath;
			instance.SetActive(true);
			yield return new WaitForSeconds(m_SpawnSpeed);
		}
	}
}
