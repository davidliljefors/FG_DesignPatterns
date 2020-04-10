using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    float Speed { get; set; }
    int Damage { get; set; }
    Vector3 Target { get; set; }
	void Impact(GameObject other);
}
