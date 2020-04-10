using System;
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
	private int m_Health = 10;
	private Vector3 m_PositionOffset;
	private bool m_ReachedPlayerBase = false;
	private Animator m_Anim;
	private BoxCollider m_Collider;

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

	private void OnEnable()
	{
		CurrentPathIndex = 0;
		m_Health = m_MaxHealth;
		m_ReachedPlayerBase = false;
		Killed = false;
		OnHealthChanged += DeathCheck;
		OnHealthChanged += TakeDamageAnim;

		m_Collider = GetComponentInChildren<BoxCollider>();
		m_Collider.enabled = true;
		m_Anim = GetComponent<Animator>();
		m_PositionOffset = new Vector3(0, m_Collider.size.y / 2f, 0);
		transform.position += m_PositionOffset;
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
			// Todo: Fix! Only using string to set for testing purposes
			const string Name = "Killed";
			m_Anim.SetTrigger(Name);
			Killed = true;
			StartCoroutine(Die());
		}
	}

	private void TakeDamageAnim(int value)
	{
		if (!Killed)
		{
			// Todo: Fix! Only using string to set for testing purposes
			const string Name = "Damaged";
			m_Anim.SetTrigger(Name);
		}
	}

	void Update()
	{
		if (!Killed)
		{
			if (m_ReachedPlayerBase)
			{
				// Todo: fix string 
				const string Tag = "Player";
				GameObject.FindGameObjectWithTag(Tag).GetComponent<Player>().Health -= 1;
				gameObject.SetActive(false);
				// Todo Attack player
				return;
			}
			Move(GetNextPosition());
		}
	}

	public void Move(Vector3 nextPosition)
	{
		// Todo: Fix! Only using string to set for testing purposes
		const string Name = "isWalking";
		m_Anim.SetBool(Name, true);
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
			for (int i = ActiveStatusEffects.Count - 1; i >= 0; i--)
			{
				if (ActiveStatusEffects[i].Type == effect.Type)
				{
					StopCoroutine(ActiveStatusEffects[i].Routine);
					ActiveStatusEffects[i].Disable(gameObject);
					ActiveStatusEffects.RemoveAt(i);
				}
			}
		}

		effect.Routine = StartCoroutine(ApplyEffect(effect));
		ActiveStatusEffects.Add(effect);
	}

	public IEnumerator ApplyEffect(IStatusEffect effect)
	{
		effect.Enable(gameObject);

		yield return new WaitForSeconds(effect.Duration);

		effect.Disable(gameObject);
		ActiveStatusEffects.Remove(effect);
	}
}
