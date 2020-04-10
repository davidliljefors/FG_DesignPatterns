using System;
using UnityEngine;

public class GameOverUI : MonoBehaviour, IDisposable
{
	[SerializeField] GameObject m_GameOverPanel;
	private ICharacter m_Player;

	void Start()
	{
		m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<ICharacter>();
		HealthCheck(m_Player.Health);
		m_Player.OnHealthChanged += HealthCheck;
	}

	void HealthCheck(int value)
	{
		if (value == 0)
		{
			m_GameOverPanel.SetActive(true);
			return;
		}
	}

	public void Dispose()
	{
		m_Player.OnHealthChanged -= HealthCheck;
	}
}
