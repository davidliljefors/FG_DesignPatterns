using UnityEngine;
using Tools;

[RequireComponent(typeof(SphereCollider))]
[SelectionBase]
public class Tower : MonoBehaviour, ITower
{
	[SerializeField] private float m_Range = 3f;
	[SerializeField] private float m_AttackDelay = 1f;
	[SerializeField] private GameObject m_Projectile = default;
	[SerializeField] private GameObjectPool m_ProjectilePool;
	[SerializeField] private int m_Damage = 1;

	private const string k_EnemyTag = "Enemy";

	private float m_NextAttackTime = 0;

	public float Range { get => m_Range; set => m_Range = value; }
	public float AttackDelay { get => m_AttackDelay; set => m_AttackDelay = value; }
	public GameObject Projectile { get => m_Projectile; set => m_Projectile = value; }
	
	private void Start()
	{
		SphereCollider sphere = GetComponent<SphereCollider>();
		sphere.radius = Range;
		m_ProjectilePool = new GameObjectPool(5, Projectile, 1, transform);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag(k_EnemyTag) && Time.time >= m_NextAttackTime)
		{
			Fire(other.transform.position);
			m_NextAttackTime = Time.time + AttackDelay;
		}
	}

	public void Fire(Vector3 target)
	{
		var go = m_ProjectilePool.Rent(false);
		var proj = go.GetComponent<IProjectile>();
		go.transform.position = transform.position;
		proj.Target = target;
		proj.Damage = m_Damage;

		go.SetActive(true);
	}
}
