using System;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class BomberManager
{	
	public int m_TargetPlayer;
	public GameObject m_BomberPrefab;

	[HideInInspector] public GameObject m_Instance;
	[HideInInspector] public int m_PlayerNumber;
	//[HideInInspector] public TankManager m_TankManager;

	private BomberMovement m_Movement;
	private BomberShooting m_Shooting;

	public void Setup()
	{
		m_Movement = m_Instance.GetComponent<BomberMovement>();
		m_Instance.SetActive (true);
		m_Movement.enabled = true;
		m_Movement.m_TargetPlayer = m_TargetPlayer;

		m_Shooting = m_Instance.GetComponent<BomberShooting> ();
		m_Shooting.m_PlayerNumber = m_PlayerNumber;

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