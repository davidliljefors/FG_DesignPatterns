using UnityEngine;
using Tools;

[RequireComponent(typeof(SphereCollider))]
[SelectionBase]
public class Tower : MonoBehaviour
{
	[SerializeField] private float m_Range = 3f;
	[SerializeField] private float m_AttackDelay = 1f;
	[SerializeField] private GameObject m_Projectile = default;
	private const string k_EnemyTag = "Enemy";

	[SerializeField] private GameObjectPool m_ProjectilePool;

	private float m_NextAttackTime = 0;

	private void Start()
	{
		SphereCollider sphere = GetComponent<SphereCollider>();
		sphere.radius = m_Range;
		m_ProjectilePool = new GameObjectPool(5, m_Projectile, 1, transform);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag(k_EnemyTag) && Time.time >= m_NextAttackTime)
		{
			Debug.Log("Firing");
			Fire(other.transform.position);
			m_NextAttackTime = Time.time + m_AttackDelay;
		}
	}

	private void Fire(Vector3 target)
	{
		var go = m_ProjectilePool.Rent(true);
		var proj = go.GetComponent<Projectile>();
		go.transform.position = transform.position;
		proj.Target = target;
	}
}
