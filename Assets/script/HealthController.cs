using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour 
{

	[SerializeField] private float Health = 100f;

		public void ApplyDamage (float damage)
	{
		Health -= damage;

		if (Health <= 0f)
		{

			Destroy (gameObject);
		}
	}
}
