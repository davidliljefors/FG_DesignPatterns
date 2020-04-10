using UnityEngine;
using System.Collections.Generic;
using MapTools;
using Tools;
using System.Collections;

[System.Serializable]
public struct UnitTypeToPrefab
{
	[SerializeField] private UnitType m_Type;
	[SerializeField] private GameObject m_Prefab;

	public UnitType Type { get => m_Type; set => m_Type = value; }
	public GameObject Prefab { get => m_Prefab; set => m_Prefab = value; }
}

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private MapManager m_MapManager = default;
	[SerializeField] private uint m_InitialPoolSize = 25;
	[SerializeField] private float m_SpawnSpeed = 1f;
	[SerializeField] private UnitTypeToPrefab[] m_UnitTypeToPrefab;

	private Queue<UnitWave> m_SpawnWaves;
	private IDictionary<UnitType, GameObjectPool> m_EnemyPools;
	private Coroutine m_SpawnRoutine;

	// Start is called before the first frame update
	void Start()
	{
		m_EnemyPools = new Dictionary<UnitType, GameObjectPool>();
		foreach (UnitTypeToPrefab unit in m_UnitTypeToPrefab)
		{
			m_EnemyPools.Add(unit.Type, new GameObjectPool(m_InitialPoolSize, unit.Prefab, 1, transform));
		}
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
					GameObject instance = m_EnemyPools[pair.Key].Rent(false);
					instance.transform.position = m_MapManager.mapObject.LocalToWorld(m_MapManager.MapInfo.Start.Value);
					instance.transform.rotation = Quaternion.identity;
					instance.GetComponent<IPathAgent>().Path = m_MapManager.WorldPath;
					instance.SetActive(true);
					yield return new WaitForSeconds(m_SpawnSpeed);
				}
			}
		}
	}
}

