using UnityEngine;

public enum StatusEffectType
{
	Slow
}

public interface IStatusEffect
{
	StatusEffectType Type { get; }

	float Duration { get; }

	bool ShouldStack { get; }

	Coroutine Routine { get; set; }

	void Enable(GameObject affected);

	// Could implement a way where the effect has a Tick() function applies stuff like a damage-over-time
	// Example burning effect
	// void Tick(float ElapsedTime)

	void Disable(GameObject affected);
	
}
