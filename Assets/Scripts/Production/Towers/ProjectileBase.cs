using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour, IProjectile
{
	private const string k_EnemyTag = "Enemy";

	[SerializeField] private float m_Speed = 50f;
	[SerializeField] private int m_Damage = 1;
	[SerializeField] private float m_Lifespan = 3f;

	private float m_Age = 0f;
	private Vector3 m_Velocity;
	
	public Vector3 Target { get; set; }
	public int Damage { get => m_Damage; set => m_Damage = value; }
	public float Speed { get => m_Speed; set => m_Speed = value; }


	// Target should be set before enabling the GO unless you set velocity yourself
	private void OnEnable()
	{
		m_Age = 0;
		m_Velocity = (Target - transform.position).normalized * m_Speed;
	}

	private void Update()
	{
		transform.position += m_Velocity * Time.deltaTime;
		m_Age += Time.deltaTime;

		if (m_Age >= m_Lifespan)
		{
			gameObject.SetActive(false);
		}
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(k_EnemyTag))
		{
			Impact(other.gameObject);
			gameObject.SetActive(false);
		}
	}
	
	public abstract void Impact(GameObject other);

}
