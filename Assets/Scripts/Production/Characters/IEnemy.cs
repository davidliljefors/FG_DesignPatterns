using UnityEngine;
using System.Collections.Generic;
public interface IEnemy : ICharacter
{
	IList<Vector3> Path { get; set; }
}
		