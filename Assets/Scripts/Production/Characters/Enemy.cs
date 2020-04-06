using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
	public MapConfig MapObject { get; set; }
	public IList<Vector2Int> Path { get; set; }
	private int health;

	public event Action<int> OnHealthChanged;
	public int Health
	{
		get => health;
		set
		{
			if (health != value)
			{
				health = value; OnHealthChanged?.Invoke(health);
			}
		}
	}

	public float moveSpeed = 0.3f;
		
	private Vector3 moveTo;
	private int currentPathIndex = 0;
	private bool reachedPlayerBase = false;

	private void Start()
	{
		moveTo = MapObject.LocalToWorld(Path[currentPathIndex]);
	}

	void Update()
	{
		if(reachedPlayerBase)
		{
			// Todo Attack player
			return;
		}

		transform.position = Vector3.MoveTowards(transform.position, moveTo, moveSpeed * Time.deltaTime);

		if(Vector3.Equals(transform.position, moveTo))
		{
			currentPathIndex++;
			if(currentPathIndex == Path.Count)
			{
				reachedPlayerBase = true;
				return;
			}
			moveTo = MapObject.LocalToWorld(Path[currentPathIndex]);
		}
	}
}
