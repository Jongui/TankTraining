using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunScript : MonoBehaviour {

	public LayerMask m_TankMask;
	public float m_MaxLifeTime = 1500f;                  
	public float m_ExplosionRadius = 0.1f;

	[HideInInspector] public TankManager m_TankManager;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, m_MaxLifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		// Find all the tanks in an area around the shell and damage them.
		Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

		for (int i = 0; i < colliders.Length; i++) {
			Rigidbody targetRigidbody = colliders [i].GetComponent<Rigidbody> ();
			if (!targetRigidbody)
				continue;
			//targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);

			TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();
			if (!targetHealth)
				continue;

			int damage = 5;

			TankShooting targetShooting = targetRigidbody.GetComponent<TankShooting> ();
			if (m_TankManager.m_PlayerNumber == targetShooting.m_PlayerNumber) {
				m_TankManager.SubPoints (damage);
			} else {
				m_TankManager.AddPoints (damage);
			}

			targetHealth.TakeDamage (damage);
		}

		/*m_ExplosionParticles.transform.parent = null;
		m_ExplosionParticles.Play ();

		m_ExplosionAudio.Play ();*/

		Destroy (gameObject);
	}
}
