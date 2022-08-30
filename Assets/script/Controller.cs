using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {


	static Animator anim;
	public float speed = 10.0f;
	public float rotationSpeed = 100.0f;

	void start ()
	{
		anim = GetComponent<Animator> ();
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update ()
	{
		float tranlation = Input.GetAxis ("Vertical") * speed;
		float straffe = Input.GetAxis ("Horizontal") * speed;
		tranlation *= Time.deltaTime;
		straffe *= Time.deltaTime;

		transform.Translate (straffe, 0, tranlation);

		if (Input.GetButton ("Fire1")) {
			anim.SetBool ("isAttaking", true);
		} 
		else
			anim.SetBool ("isAttaking", false);

		if (tranlation != 0) {
			anim.SetBool ("isWallking", true);
			anim.SetBool ("isIdle", false);
		} 
		else 
		{
			anim.SetBool ("isWallking", false);
			anim.SetBool ("isIdle", true);
		}

		if (Input.GetKeyDown ("escape"))
			Cursor.lockState = CursorLockMode.None;

	}
}