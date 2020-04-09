using UnityEngine;

public class Projectile : MonoBehaviour
{
	private const string k_EnemyTag = "Enemy";

	[SerializeField] private float m_Speed = 50f;
	[SerializeField] private uint m_HitBufferSize = 8;
	[SerializeField] private float m_ExplosionRadius = 0.5f;
	[SerializeField] private int m_Damage = 1;
	[SerializeField] private LayerMask m_CollisionLayer;

	public Vector3 Target { get; set; }
	public int Damage { get => m_Damage; set => m_Damage = value; }

	private Collider[] m_HitBuffer;

	private void Awake()
	{
		m_HitBuffer = new Collider[m_HitBufferSize];
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, Target, m_Speed * Time.deltaTime);
		if (Vector3.Equals(transform.position, Target))
		{
			Explode();
			gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(k_EnemyTag))
		{
			Explode();
		}
	}

	private void Explode()
	{
		int count = Physics.OverlapSphereNonAlloc(transform.position,
			m_ExplosionRadius, m_HitBuffer, m_CollisionLayer);

		for (int i = 0; i < count; i++)
		{
			IEnemy component = m_HitBuffer[i].GetComponentInParent<IEnemy>();
			if(component != null)
			{
				component.Health -= Damage;
			}
		}

		gameObject.SetActive(false);
	}
}
