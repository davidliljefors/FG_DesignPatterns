using UnityEngine;

public class Projectile : MonoBehaviour
{
	public Vector3 Target { get; set; }
	public Vector3 StartPosition { get; set; }
	public float Speed { get; set; }
	public int Damage { get; set; }


	private void OnEnable()
	{
		transform.position = StartPosition;
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, Target, Speed * Time.deltaTime);
		if(Vector3.Equals(transform.position, Target))
		{
			Debug.Log("Boom!");
			gameObject.SetActive(false);
		}
	}
}
