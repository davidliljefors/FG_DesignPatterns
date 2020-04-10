using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathAgent
{
	float MoveSpeed { get; set; }
	IList<Vector3> Path { get; set; }
	int CurrentPathIndex { get; set; }
	void Move(Vector3 nextPosition);
	Vector3 GetNextPosition();
}
