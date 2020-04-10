using System;
using UnityEngine;

public class SlowEffect : IStatusEffect
{
	public float Duration { get; }
	public bool ShouldStack { get; }
	public StatusEffectType Type { get; }
	public Coroutine Routine { get; set; }

	private float m_OriginalSpeed;
	private float m_SpeedMultiplier;

	public SlowEffect(StatusEffectType type, float duration, float speedMultiplier, bool shouldStack = false)
	{
		Type = type;
		Duration = duration;
		m_SpeedMultiplier = speedMultiplier;
		ShouldStack = shouldStack;
	}

	public void Enable(GameObject affected)
	{
		IPathAgent agent = affected.GetComponent<IPathAgent>();
		m_OriginalSpeed = agent.MoveSpeed;
		agent.MoveSpeed = agent.MoveSpeed * m_SpeedMultiplier;
	}

	public void Disable(GameObject affected)
	{
		IPathAgent agent = affected.GetComponent<IPathAgent>();
		agent.MoveSpeed = m_OriginalSpeed;
	}
}
