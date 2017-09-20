using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour {

	public int m_TargetPlayer = 1;
	public float m_Altitude;
	public Rigidbody m_Bombs;
	public int m_BombsCount;

	private TankManager m_TargetTank;
	private Vector3 m_StartPosition;
	private Vector3 m_TempPosition;
	private Vector3 m_Destination;
	private float m_Speed = 20f;
	private Rigidbody m_Rb;
	private Transform m_FirstTarget;
	private Vector3 m_LastPosition;
	private Vector3 m_Velocity;
	private bool m_Fired;
	private bool m_TempDest;
	private float bombInterval;
	public float m_MaxLifeTime = 20f;

	// Use this for initialization
	void Start () {
		//target = new Vector3 (0, 20, 0);
		m_Rb = GetComponent<Rigidbody>();
		m_TargetTank = GameManager.m_Instance.FindTargetTank (m_TargetPlayer);
		m_Fired = false;
		m_StartPosition = transform.position;
		Destroy(gameObject, m_MaxLifeTime);
	}

	void FixedUpdate()
	{
		MovePlane ();
		if (!m_Fired) {
			DropBomb ();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Rigidbody planeRigidbody = other.GetComponent<Rigidbody> ();
		planeRigidbody.AddForce (new Vector3 (0, 1, 0));
	}

	private void MovePlane(){
		Vector3 targetPosition = m_TargetTank.m_Instance.transform.position;
		m_StartPosition.y = targetPosition.y = m_Altitude;
		m_Destination = (targetPosition - transform.position).normalized;
		if (!m_Fired) {
			transform.LookAt (targetPosition);
			m_Rb.MovePosition (transform.position + m_Destination * m_Speed * Time.fixedDeltaTime);
			m_Velocity = (transform.position - m_LastPosition) / Time.deltaTime;
		} else {
			if (m_TempDest) {
				targetPosition = m_TempPosition;
			}
			else {
				targetPosition = m_StartPosition;
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

	private void DropBomb(){
		Vector3 targetPosition = m_TargetTank.m_Instance.transform.position;
		//targetPosition.y = m_Altitude;
		float dist = Vector3.Distance(targetPosition, transform.position);
		if (m_BombsCount == 0)
			m_Fired = true;
		if (dist <= 50.0f && !m_Fired && bombInterval >= 0.3f){
			m_TempDest = true;
			m_TempPosition = targetPosition;
			m_TempPosition.y = m_Altitude;
			Rigidbody bombInstance = Instantiate (m_Bombs, transform.position, transform.rotation) as Rigidbody;
			bombInstance.velocity += m_Velocity;
			m_BombsCount--;
			bombInterval = 0;
			//bombInstance.MovePosition (transform.position + direction * m_Speed * Time.deltaTime);
			//bombInstance.AddForce(m_Velocity);
		}
		bombInterval += Time.deltaTime;
	}

	// Update is called once per frame
	void Update () {
		//transform.RotateAround (target, Vector3.up, m_Speed * Time.deltaTime);	

		//m_Rb.AddForce(transform.forward * m_Speed);

		//transform.Rotate(0.0f, m_Speed * Time.deltaTime, -10.0f * Time.deltaTime);
	}
}
