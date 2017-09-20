﻿using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce;            
    public float m_MaxLifeTime = 3f;                  
    public float m_ExplosionRadius;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
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

			float damage = CalculateDamage (targetRigidbody.position);

			targetHealth.TakeDamage (damage);
		}

		Rigidbody bombRigidbody = other.GetComponent<Rigidbody> ();
		if (bombRigidbody)
			return;
		
		//if (tankFound) {
			m_ExplosionParticles.transform.parent = null;
			m_ExplosionParticles.Play ();
			m_ExplosionAudio.Play ();
		

			Destroy (m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
			Destroy (gameObject);
		//}
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
		Vector3 explosionToVector = targetPosition - transform.position;
		float explosionDistance = explosionToVector.magnitude;
		float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
		float damage = relativeDistance * m_MaxDamage;
		damage = Mathf.Max (0f, damage);
		return damage;
    }
}