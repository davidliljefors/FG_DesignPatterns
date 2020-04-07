using System;

using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
	[SerializeField] private int m_Health = 10;
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

	private void Awake()
	{

	}

}
