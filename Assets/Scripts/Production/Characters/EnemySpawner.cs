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
	[SerializeField] private float m_SpawnSpeed = 1f;
	private Queue<UnitWave> m_SpawnWaves;
	private GameObjectPool m_EnemyPool;
	private Coroutine m_SpawnRoutine;

	// Start is called before the first frame update
	void Start()
	{
		m_EnemyPool = new GameObjectPool(m_InitialPoolSize, m_EnemyPrefab, 1, transform);
		m_SpawnWaves = new Queue<UnitWave>(m_MapManager.MapInfo.Units);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("Spawning");
			StartCoroutine(SpawnWave());
		}
	}

	private IEnumerator SpawnWave()
	{
		if (m_SpawnWaves.Count > 0)
		{
			UnitWave wave = m_SpawnWaves.Dequeue();
			foreach (KeyValuePair<UnitType, int> pair in wave.Units)
			{
				for (int i = 0; i < pair.Value; i++)
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
	}
}

