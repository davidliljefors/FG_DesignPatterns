using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusAffectable
{
	/// <summary>
	/// Should Launch a Coroutine
	/// </summary>
	void StartEffect(IStatusEffect effect);
	IEnumerator ApplyEffect(IStatusEffect effect);
	List<IStatusEffect> ActiveStatusEffects { get; }
}
