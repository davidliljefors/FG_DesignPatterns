using UnityEngine;
using System.Collections.Generic;
public interface IEnemy : ICharacter
{
	bool Killed { get; set; }
}
		