using System;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class PlaneManager
{	
	public int m_TargetPlayer;
	public GameObject m_PlanePrefab;

	[HideInInspector] public GameObject m_Instance;
	[HideInInspector] public TankManager m_TankManager;

	private PlaneMovement m_Movement;
	private PlaneShooting m_Shooting;

	public void Setup()
	{
		m_Movement = m_Instance.GetComponent<PlaneMovement>();
		m_Instance.SetActive (true);
		m_Movement.enabled = true;
		m_Movement.m_TargetPlayer = m_TargetPlayer;

		m_Shooting = m_Instance.GetComponent<PlaneShooting> ();
		m_Shooting.m_TankManager = m_TankManager;

	}
		
	public void DisableControl()
	{
		m_Movement.enabled = false;
	}


	public void EnableControl()
	{
		m_Movement.enabled = true;
	}

}