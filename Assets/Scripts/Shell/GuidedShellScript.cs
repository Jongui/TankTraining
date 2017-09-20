using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedShellScript : MonoBehaviour {

	public LayerMask m_TankMask;
	public int m_TargetPlayer = 1;
	public float m_Speed;

	private TankManager m_TargetTank;
	private Vector3 m_StartPosition;
	//private float m_StartTime;
	//private bool check;
	//private GameObject m_Target;
	private float moveTime;

	// Use this for initialization
	void Start () {
		m_TargetTank = GameManager.m_Instance.FindTargetTank (m_TargetPlayer);
		//m_Target = m_TargetTank.m_Instance;
		//m_StartTime = Time.deltaTime;
		//check = false;
	}

	// Update is called once per frame
	void Update () {
		/*Vector3 dif = m_Target.transform.position - transform.position;
		dif.Normalize ();
		print ("Coordenadas diferença: x: " + dif.x
			+ ", y: " + dif.y
			+ ", z: " + dif.z);
		transform.Translate (dif * m_Speed);*/
		moveTime += Time.deltaTime * m_Speed;
		transform.LookAt(m_TargetTank.m_Instance.transform.position);
		transform.position = Vector3.Lerp (transform.position, m_TargetTank.m_Instance.transform.position, moveTime);// 
	}
}
