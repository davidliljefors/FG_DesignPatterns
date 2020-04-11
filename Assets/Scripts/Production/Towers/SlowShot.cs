using UnityEngine;

public class SlowShot : ProjectileBase
{
	private void Awake()
	{
		
	}

	public override void Impact(GameObject other)
	{
		IEnemy enemy = other.GetComponentInParent<IEnemy>();
		if (enemy != null)
		{
			enemy.Health -= Damage;
		}

		IStatusAffectable affectable = other.GetComponentInParent<IStatusAffectable>();
		if (affectable != null)
		{
			IStatusEffect SlowEffect = new SlowEffect(StatusEffectType.Slow, 0.5f, 0.3f);
			affectable.StartEffect(SlowEffect);
		}
	}
}
