﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberMovement : MonoBehaviour {

	[HideInInspector]public int m_TargetPlayer = 1;
	[HideInInspector]public TankManager m_TargetTank;
	[HideInInspector]public Vector3 m_Velocity;

	public float m_Altitude;
	public BomberShooting m_BomberShooting;

	private Vector3 m_TempPosition;
	private Vector3 m_StartPosition;
	private Vector3 m_Destination;
	private float m_Speed = 20f;
	private Rigidbody m_Rb;
	private Transform m_FirstTarget;
	private Vector3 m_LastPosition;
	private bool m_TempDest;
	private float m_BombInterval;
	private const float m_MaxLifeTime = 10f;

	// Use this for initialization
	void Start () {
		//target = new Vector3 (0, 20, 0);
		m_Rb = GetComponent<Rigidbody>();
		m_TargetTank = GameManager.m_Instance.FindTankManager (m_TargetPlayer);
		m_StartPosition = transform.position;
	}

	void FixedUpdate()
	{
		MoveBomber ();
	}

	private void OnTriggerEnter(Collider other)
	{
		Rigidbody planeRigidbody = other.GetComponent<Rigidbody> ();
		planeRigidbody.AddForce (new Vector3 (0, 1, 0));
	}

	private void MoveBomber(){
		Vector3 targetPosition = m_TargetTank.m_Instance.transform.position;
		m_StartPosition.y = targetPosition.y = m_Altitude;
		m_Destination = (targetPosition - transform.position).normalized;
		if (!m_BomberShooting.m_Fired) {
			transform.LookAt (targetPosition);
			m_Rb.MovePosition (transform.position + m_Destination * m_Speed * Time.fixedDeltaTime);
			m_Velocity = (transform.position - m_LastPosition) / Time.deltaTime;
		} else {
			if (m_BomberShooting.m_Firing) {
				m_TempDest = true;
				targetPosition = m_BomberShooting.m_LastBombTransform.position;
			}
				else 
			{
				m_TempDest = false;
				targetPosition = m_StartPosition;
				Destroy(gameObject, m_MaxLifeTime);
			}
			targetPosition.y = m_Altitude;
			float distToEnd = Vector3.Distance (targetPosition, transform.position);
			if (distToEnd >= 0.2f) {
				Vector3 direction = (targetPosition - transform.position).normalized;
				targetPosition.y = m_Altitude;
				transform.LookAt (targetPosition);
				m_Rb.MovePosition (transform.position + direction * m_Speed * Time.fixedDeltaTime);
			} else {
				if (m_TempDest) {
					m_TempDest = false;
				} else {
					Destroy (gameObject);
				}
			}

		}
		m_LastPosition = transform.position;
	}

	public float CalcDistance(){
		Vector3 targetPosition = m_TargetTank.m_Instance.transform.position;
		//targetPosition.y = m_Altitude;
		return Vector3.Distance(targetPosition, transform.position);
	}


}
