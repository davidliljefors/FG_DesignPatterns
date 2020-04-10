using UnityEngine;
public interface ITower
{
	float Range { get; set; }
	float AttackDelay { get; set; }
	GameObject Projectile { get; set; }
	void Fire(Vector3 target);
}
