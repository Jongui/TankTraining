using System;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class PlaneManager
{	
	public int m_TargetPlayer;
	public GameObject m_PlanePrefab;
	[HideInInspector] public GameObject m_Instance;          

	private PlaneMovement m_Movement;

	public void Setup()
	{
		
	}

	public void SpawnPlane(Transform spawnPoint)
	{
		//m_Instance =
		//	Instantiate(m_PlanePrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
		
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