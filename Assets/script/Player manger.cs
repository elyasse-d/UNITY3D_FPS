using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Playermanger : MonoBehaviour 
{
	public static int Health = 100;
	public GameObject FPSController;
	public Slider Healthbar;

	void Start () 
	{
		
	}

	void ReuceHealth()
	{
		Health = Health - 20;
		Healthbar.value = Health;
		if (Health <= 0) 
		{
			Destroy (gameObject);
		}
	}

	void Update ()
	{
		
	}

}
