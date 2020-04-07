using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy, IResettable
{
	public IList<Vector3> Path { get; set; }
	[SerializeField] private float m_MoveSpeed = 1f;
	[SerializeField] private int m_MaxHealth = 10;
	private int m_Health = 10;
	private Vector3 m_MoveTo;
	private Vector3 m_PositionOffset;
	private int m_CurrentPathIndex = 0;
	private bool reachedPlayerBase = false;


	public event Action<int> OnHealthChanged;
	public int Health
	{
		get => m_Health;
		set
		{
			if (m_Health != value)
			{
				m_Health = value; OnHealthChanged?.Invoke(m_Health);
			}
		}
	}

	void Update()
	{
		if(reachedPlayerBase)
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Health -= 1;
			gameObject.SetActive(false);
			// Todo Attack player
			return;
		}

		transform.position = Vector3.MoveTowards(transform.position, m_MoveTo, m_MoveSpeed * Time.deltaTime);

		if(Vector3.Equals(transform.position, m_MoveTo))
		{
			if(m_CurrentPathIndex >= Path.Count-1)
			{
				reachedPlayerBase = true;
				return;
			}
			m_MoveTo = GetNextPathPoint();
			transform.rotation = Quaternion.LookRotation(m_MoveTo - transform.position);
		}
	}

	public void Reset()
	{
		m_CurrentPathIndex = 0;
		m_Health = m_MaxHealth;
		reachedPlayerBase = false;

		var box = GetComponentInChildren<BoxCollider>();
		m_PositionOffset = new Vector3(0, box.size.y / 2f, 0);
		transform.position += m_PositionOffset;
		m_MoveTo = GetNextPathPoint();
		transform.rotation = Quaternion.LookRotation(m_MoveTo - transform.position);
	}

	private Vector3 GetNextPathPoint()
	{
		m_CurrentPathIndex++;
		return Path[m_CurrentPathIndex] + m_PositionOffset;
	}
}
