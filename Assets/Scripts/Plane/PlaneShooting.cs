using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneShooting : MonoBehaviour {

	public Transform m_FireTransform;
	public Rigidbody m_Bombs;
	public int m_BombsCount;
	public float m_Altitude;
	public PlaneMovement m_PlaneMovement;
	public Transform m_LastBombTransform;

	[HideInInspector] public bool m_Fired;
	[HideInInspector] public bool m_Firing;
	[HideInInspector] public TankManager m_TankManager;

	private float m_BombInterval;
	private Rigidbody m_LastBomb;

	// Use this for initialization
	void Start () {
		m_Fired = false;
		m_Firing = false;
	}

	void Update()
	{
		if (m_LastBomb) {
			m_LastBombTransform = m_LastBomb.transform;
			m_Firing = true;
		} else {
			m_Firing = false;
		}
		if (!m_Fired) {
			DropBomb ();
		}
	}

	private void DropBomb(){
		Vector3 targetPosition = m_PlaneMovement.m_TargetTank.m_Instance.transform.position;
		//targetPosition.y = m_Altitude;
		float dist = m_PlaneMovement.CalcDistance();
		print ("Distancia: " + dist);
		if (m_BombsCount == 0) {
			m_Fired = true;
			m_Firing = false;
		}
		if (dist <= 50.0f && !m_Fired && m_BombInterval >= 0.3f){
			Rigidbody bombInstance = Instantiate (m_Bombs, transform.position, transform.rotation) as Rigidbody;
			bombInstance.velocity += m_PlaneMovement.m_Velocity;
			BombExplosion explosion = bombInstance.GetComponent<BombExplosion> ();
			explosion.m_TankManager = m_TankManager;
			m_BombsCount--;
			m_BombInterval = 0;
			m_Firing = true;
			m_LastBomb = bombInstance;
			//bombInstance.MovePosition (transform.position + direction * m_Speed * Time.deltaTime);
			//bombInstance.AddForce(m_Velocity);
		}
		m_BombInterval += Time.deltaTime;
	}
}
