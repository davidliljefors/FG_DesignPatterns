using UnityEngine;
using System.Collections.Generic;
public interface IEnemy : ICharacter
{
	IList<Vector2Int> Path { get; set; }
}
		