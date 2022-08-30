using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour {

	private Animator anim;
	private AudioSource _AudioSource;
	
	public float range = 100f; //Maximum range of the weapon
	public int bulletsPerMag = 30; //Bullets per each magazine
	public int bulletsLeft = 200; //Total bullets we have

	public int currentBullets; //The current bullets in our magazine

	public enum ShootMode{ Auto, Semi }
	public ShootMode ShootingMode;

	public Transform shootPoint; //The point from which the bullet leaves the muzzle
	public GameObject hitParticales;
	public GameObject bulletImpact;

	public ParticleSystem muzzleFlash; //Muzzle Flash 
	public AudioClip shootSound; 


	public float fireRate = 0.1f; //The dely between each shot
	public float damage = 20f;

	float fireTimer; //Time counter for the deley

	private bool isReloading;
	private bool isAming;
	private bool ShootInput;

	private Vector3 originalPosition;
	public Vector3 aimPosition;
	public float aodspeed= 8f;

	// Use this for initialization
	void Start () 
	{

		anim = GetComponent<Animator> ();
		_AudioSource = GetComponent<AudioSource> ();

		currentBullets = bulletsPerMag;
		originalPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () { 

		switch (ShootingMode) 
		{
		case ShootMode.Auto:
			ShootInput = Input.GetButton("Fire1");
			break;

		case ShootMode.Semi:
			ShootInput = Input.GetButtonDown("Fire1");
			break;
		}

	
		if (ShootInput)
		{
			if (currentBullets > 0)
				Fire (); //Execute the fire funtion if we press/hold the Left mouse button
			else if(bulletsLeft > 0)
				DoReload();
		}
		if (fireTimer < fireRate)
			fireTimer += Time.deltaTime; //Add into time counter
		
		if(Input.GetKeyDown(KeyCode.R))
		{
			if (currentBullets< bulletsPerMag && bulletsLeft > 0) 
				DoReload ();
		}
		AimDownSights ();

	}
		
	private void FixeUpdate()
	{
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);

		isReloading = info.IsName("Reload");
		anim.SetBool ("Aim", isAming);
	if (info.IsName ("Fire"))
	anim.SetBool ("Fire", false);
	}

	private void AimDownSights()
	{
		if (Input.GetButton ("Fire2") && !isReloading)
		{
			transform.localPosition = Vector3.Lerp (transform.localPosition, aimPosition, Time.deltaTime * aodspeed);
			isAming = true; 
		}
		else 
		{
			transform.localPosition = Vector3.Lerp (transform.localPosition, originalPosition ,Time.deltaTime*aodspeed);
			isAming = false; 
		}
	}


	private void Fire ()
	{
		    //This              Or       this 
		if(fireTimer < fireRate || currentBullets <= 0 || isReloading)
			return;

	RaycastHit hit;

	if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
	{
		Debug.Log(hit.transform.name + " found!");

			GameObject hitParticaleEffect = Instantiate (hitParticales, hit.point, Quaternion.FromToRotation (Vector3.up, hit.normal));
			hitParticaleEffect.transform.SetParent (hit.transform);
			GameObject bulletHole = Instantiate (bulletImpact, hit.point, Quaternion.FromToRotation (Vector3.forward, hit.normal));

			Destroy (hitParticaleEffect, 1f);
			Destroy (bulletHole, 2f);

			if (hit.transform.GetComponent <HealthController> ())
			{
				hit.transform.GetComponent <HealthController>().ApplyDamage(damage);
			} 
	}

		anim.CrossFadeInFixedTime ("Fire",0.01f); //Play the fire animation
		muzzleFlash.Play (); //Show the muzzle Flash
		PlayShootSound(); //Play the shooting sound effect

		currentBullets--; //Deduct one bullet
	fireTimer = 0.0f; //Reset fire timer
	}

	public void Reload()
	{
		if (bulletsLeft <= 0) return;

		int bulletsToload = bulletsPerMag - currentBullets;
		//                               If               then    1st       else    2nd
		int bulletsToDeduct = (bulletsLeft >= bulletsToload) ? bulletsToload : bulletsLeft;

		bulletsLeft -= bulletsToDeduct;
		currentBullets += bulletsToDeduct;
	}

	private void DoReload ()
	{
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);

		if (isReloading) return;
		anim.CrossFadeInFixedTime ("Reload", 0.01f);

	}

	private void PlayShootSound()
	{
		_AudioSource.PlayOneShot (shootSound);
		//_AudioSource.clip = shootSound;
		//_AudioSource.Play();
	}
}
