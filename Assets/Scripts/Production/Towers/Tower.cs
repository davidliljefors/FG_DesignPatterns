using UnityEngine;
using Tools;

public class Tower : MonoBehaviour
{
	[SerializeField] private float m_Range = 3f;
	[SerializeField] private float m_AttackDelay = 1f;
	[SerializeField] private GameObject m_Projectile = default;

	[SerializeField] private GameObjectPool m_ProjectilePool;

	private float m_NextAttackTime = 0;

	private void Start()
	{
		var sphere = GameObjectExtensions.ForceComponent<SphereCollider>(gameObject);
		sphere.radius = m_Range;
		m_ProjectilePool = new GameObjectPool(5, m_Projectile, 1, transform);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Enemy") && Time.time >= m_NextAttackTime)
		{
			Fire(other.transform.position);
			m_NextAttackTime = Time.time + m_AttackDelay;
		}
	}

	private void Fire(Vector3 target)
	{
		var proj = m_ProjectilePool.Rent(true).GetComponent<Projectile>();
		proj.StartPosition = transform.position;
		proj.Target = target;
	}
}
