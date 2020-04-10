using UnityEngine;

public class SlowShot : ProjectileBase
{
	private IStatusEffect m_SlowEffect;

	private void Awake()
	{
		m_SlowEffect = new SlowEffect(StatusEffectType.Slow, 3f, 0.3f);
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
			affectable.StartEffect(m_SlowEffect);
		}
	}
}
