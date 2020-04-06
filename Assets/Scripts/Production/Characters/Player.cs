using System;

using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
	private int m_Health;
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

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
