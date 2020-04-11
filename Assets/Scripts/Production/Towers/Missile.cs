using UnityEngine;

public class Missile : ProjectileBase
{
	[SerializeField] private uint m_HitBufferSize = 8;
	[SerializeField] private float m_ExplosionRadius = 0.5f;
	[SerializeField] private LayerMask m_CollisionLayer = default;

	private Collider[] m_HitBuffer;

	private void Awake()
	{
		m_HitBuffer = new Collider[m_HitBufferSize];
	}

	public override void Impact(GameObject other)
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
