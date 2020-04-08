using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float m_Speed = 50f;
	private const string k_EnemyTag = "Enemy";
	[SerializeField] private uint m_HitBufferSize = 8;
	[SerializeField] private float m_ExplosionRadius = 0.5f;



	public Vector3 Target { get; set; }
	public int Damage { get; set; }

	Collider[] m_HitBuffer;

	private void Awake()
	{
		m_HitBuffer = new Collider[m_HitBufferSize];
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, Target, m_Speed * Time.deltaTime);
		if (Vector3.Equals(transform.position, Target))
		{
			Debug.Log("Miss!");
			gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag(k_EnemyTag))
		{
			Explode();
		}
	}

	private void Explode()
	{
		Debug.Log("Boom!");
		int count = Physics.OverlapSphereNonAlloc(transform.position, m_ExplosionRadius, m_HitBuffer);
		if(count > 0)
		{
			foreach(Collider col in m_HitBuffer)
			{

			}
		}
		gameObject.SetActive(false);
	}
}
