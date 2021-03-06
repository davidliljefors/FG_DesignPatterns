﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Enemy : MonoBehaviour, IEnemy, IPathAgent, IStatusAffectable
{
	public IList<Vector3> Path { get; set; }
	[SerializeField] private float m_MoveSpeed = 1f;
	[SerializeField] private int m_MaxHealth = 10;
	[SerializeField] private float m_DeathTimer = 1f;
	[SerializeField] private int m_Damage = 1;
	private int m_Health = 10;
	private Vector3 m_PositionOffset;
	private bool m_ReachedPlayerBase = false;
	private Animator m_Anim;
	private BoxCollider m_Collider;

	// Animation hash values
	int m_KilledHash = Animator.StringToHash("Killed");
	int m_DamagedHash = Animator.StringToHash("Damaged");
	int m_WalkingHash = Animator.StringToHash("isWalking");
	int m_WalkStateHash = Animator.StringToHash("Walk");

	const string m_PlayerTag = "Player";

	public List<IStatusEffect> ActiveStatusEffects { get; private set; }

	public bool Killed { get; set; } = false;
	public event Action<int> OnHealthChanged;
	public int Health
	{
		get => m_Health;
		set
		{
			if (m_Health != value)
			{
				m_Health = value;
				OnHealthChanged?.Invoke(m_Health);
			}
		}
	}

	public int CurrentPathIndex { get; set; } = 0;
	public float MoveSpeed { get => m_MoveSpeed; set => m_MoveSpeed = value; }

	private void Awake()
	{
		ActiveStatusEffects = new List<IStatusEffect>();
	}

	public void Init()
	{
		OnHealthChanged += DeathCheck;
		OnHealthChanged += TakeDamageAnim;
		CurrentPathIndex = 0;
		m_Health = m_MaxHealth;
		m_ReachedPlayerBase = false;
		Killed = false;
		m_Collider = GetComponentInChildren<BoxCollider>();
		m_Collider.enabled = true;

		m_Anim = GetComponent<Animator>();
		m_Anim.ResetTrigger(m_KilledHash);
		// Todo: fix resetting animation 
		m_Anim.Play(m_WalkStateHash, -1, UnityEngine.Random.value);

		m_PositionOffset = new Vector3(0, m_Collider.size.y / 2f, 0);
		transform.position += m_PositionOffset;
	}
	public void OnEnable()
	{
		Init();
	}

	private void OnDisable()
	{
		OnHealthChanged -= DeathCheck;
		OnHealthChanged -= TakeDamageAnim;
	}

	private void DeathCheck(int newHealth)
	{
		if (newHealth <= 0 && !Killed)
		{
			m_Anim.SetTrigger(m_KilledHash);
			Killed = true;
			StartCoroutine(Die());
		}
	}

	private void TakeDamageAnim(int value)
	{
		if (!Killed)
		{
			m_Anim.SetTrigger(m_DamagedHash);
		}
	}

	void Update()
	{
		if (!Killed)
		{
			if (m_ReachedPlayerBase)
			{		
				GameObject.FindGameObjectWithTag(m_PlayerTag).GetComponent<ICharacter>().Health -= m_Damage;
				gameObject.SetActive(false);
				// Todo Attack player
				return;
			}
			Move(GetNextPosition());
		}
	}

	public void Move(Vector3 nextPosition)
	{
		m_Anim.SetBool(m_WalkingHash, true);
		transform.position = Vector3.MoveTowards(transform.position, nextPosition, MoveSpeed * Time.deltaTime);
	}

	public Vector3 GetNextPosition()
	{
		Vector3 target = Path[CurrentPathIndex] + m_PositionOffset;
		if (Vector3.Equals(transform.position, target))
		{
			if (CurrentPathIndex == Path.Count - 1)
			{
				m_ReachedPlayerBase = true;
				return Path[CurrentPathIndex];
			}
			CurrentPathIndex++;
			target = Path[CurrentPathIndex] + m_PositionOffset;

			// Ugly to set rotation in get next path point. Question to Ederic, what is a good way to do it without breaking single responsibility
			transform.rotation = Quaternion.LookRotation(target - transform.position);

			return target;
		}
		return target;
	}

	private IEnumerator Die()
	{
		m_Collider.enabled = false;
		yield return new WaitForSeconds(m_DeathTimer);
		gameObject.SetActive(false);
	}

	
	public void StartEffect(IStatusEffect effect)
	{
		if (effect.ShouldStack == false)
		{
			for (int i = 0; i < ActiveStatusEffects.Count; ++i)
			{
				if (ActiveStatusEffects[i].Type == effect.Type)
				{
					StopCoroutine(ActiveStatusEffects[i].Routine);
					ActiveStatusEffects[i].Disable(gameObject);
					ActiveStatusEffects.RemoveAt(i);
					break;
				}
			}
		}

		effect.Routine = StartCoroutine(ApplyEffect(effect));
	}

	public IEnumerator ApplyEffect(IStatusEffect effect)
	{
		ActiveStatusEffects.Add(effect);
		effect.Enable(gameObject);
		yield return new WaitForSeconds(effect.Duration);
		effect.Disable(gameObject);
		ActiveStatusEffects.Remove(effect);
	}
}
