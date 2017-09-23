using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TankManager
{
    /*public Color m_PlayerColor;            
    public Transform m_SpawnPoint;
	public Transform m_PlaneSpawnPoint;
	public Text m_AmmoText;
	public PlaneManager m_PlaneManager;*/

    /*[HideInInspector] public int m_PlayerNumber;             
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public int m_Wins;  
	[HideInInspector] public int m_PlayerScore = 1000;*/

	public TankObject m_TankObject;

	public Transform m_SpawnPoint;
	public Transform m_PlaneSpawnPoint;
	public Text m_AmmoText;

	[HideInInspector] public GameObject m_Instance;        


    private TankMovement m_Movement;       
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;
	private const int m_ShellValue = 10;
	private const int m_BomberValue = 100;
	private const int m_GuidedShellValue = 50;



    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

		m_Shooting.m_AmmoText = m_AmmoText;
		m_Shooting.m_PlayerColor = m_TankObject.m_PlayerColor;
		m_Shooting.m_PlaneSpawnPoint = m_PlaneSpawnPoint;
		m_Shooting.m_BomberAmmo = m_TankObject.m_BomberAmmo;
		m_Shooting.m_ShellAmmo = m_TankObject.m_ShellAmmo;
		m_Shooting.m_GuidedShellAmmo = m_TankObject.m_GuidedShellAmmo;
		m_Shooting.m_TankManager = this;

		m_Movement.m_PlayerNumber = m_TankObject.m_PlayerNumber;
		m_Shooting.m_PlayerNumber = m_TankObject.m_PlayerNumber;
		m_Shooting.m_BomberManager = m_TankObject.m_BomberManager;

		if (m_TankObject.m_PlayerNumber == 1) {
			m_Shooting.m_TargetGuided = 2;
		} else {
			m_Shooting.m_TargetGuided = 1;
		}

		m_TankObject.m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_TankObject.m_PlayerColor) + ">PLAYER " + m_TankObject.m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
			renderers[i].material.color = m_TankObject.m_PlayerColor;
        }
    }

	public int ShellCount()
	{
		return m_Shooting.m_ShellAmmo;
	}

	public int GuidedShellCount()
	{
		return m_Shooting.m_GuidedShellAmmo;
	}

	public int BomberCount()
	{
		return m_Shooting.m_BomberAmmo;
	}

	public void AddPoints(int points){
		this.m_TankObject.m_PlayerScore += points;
	}

	public void SubPoints(int points){
		this.m_TankObject.m_PlayerScore -= points;
	}

	public void AddShell(int shells)
	{
		int newScore = m_TankObject.m_PlayerScore - shells * m_ShellValue;
		if (newScore < 0)
			return;
		m_TankObject.m_PlayerScore = newScore;
		this.m_Shooting.m_ShellAmmo += shells;
	}

	public void SubShell(int shells)
	{
		m_TankObject.m_PlayerScore += shells * m_ShellValue;
		this.m_Shooting.m_ShellAmmo -= shells;
	}

	public void AddGuidedShell(int shells)
	{
		int newScore = m_TankObject.m_PlayerScore - shells * m_GuidedShellValue;
		if (newScore < 0)
			return;
		m_TankObject.m_PlayerScore = newScore;
		this.m_Shooting.m_GuidedShellAmmo += shells;
	}

	public void SubGuidedShell(int shells)
	{
		m_TankObject.m_PlayerScore += shells * m_GuidedShellValue;
		this.m_Shooting.m_GuidedShellAmmo -= shells;
	}

	public void AddBomber(int bombers)
	{
		int newScore = m_TankObject.m_PlayerScore - bombers * m_BomberValue;
		if (newScore < 0)
			return;
		m_TankObject.m_PlayerScore = newScore;
		this.m_Shooting.m_BomberAmmo += bombers;
	}

	public void SubBomber(int bombers)
	{
		m_TankObject.m_PlayerScore += bombers * m_BomberValue;
		this.m_Shooting.m_BomberAmmo -= bombers;
	}
    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
		m_Instance.transform.position = m_SpawnPoint.position;
		m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);


    }
}
