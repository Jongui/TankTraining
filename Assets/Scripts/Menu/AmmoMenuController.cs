using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoMenuController : MonoBehaviour {

	public Text m_ScoreText;
	public Text m_ShellText;
	public Text m_GuidedShellText;
	public Text m_PlaneText;
	public int m_PlayerNumber;

	private TankManager m_TankManager;
	private string m_ColoredPlayerText;

	private void Start()
	{
		/*m_TankManager = GameManager.m_Instance.FindTankManager (m_PlayerNumber);
		string m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_TankManager.m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

		m_ScoreText.text = m_ColoredPlayerText + m_TankManager.m_PlayerScore;
		m_ShellText.text = "Shells: " + m_TankManager.ShellCount ();*/
	}

	public void InitCanvas()
	{
		m_TankManager = GameManager.m_Instance.FindTankManager (m_PlayerNumber);
		m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_TankManager.m_TankObject.m_PlayerColor) + ">PLAYER " + m_PlayerNumber + " </color>";

		m_ScoreText.text = m_ColoredPlayerText + m_TankManager.m_TankObject.m_PlayerScore + " points";

		m_ShellText.text = "Shells: " + m_TankManager.ShellCount ();
		m_GuidedShellText.text = "Guided Shells: " + m_TankManager.GuidedShellCount ();
		m_PlaneText.text = "Bombers: " + m_TankManager.BomberCount ();
	}

	public void Add10Shells()
	{
		m_TankManager.AddShell(10);
		m_ShellText.text = "Shells: " + m_TankManager.ShellCount ();
		m_ScoreText.text = m_ColoredPlayerText + m_TankManager.m_TankObject.m_PlayerScore + " points";

	}

	public void Sub10Shells()
	{
		m_TankManager.SubShell(10);
		m_ShellText.text = "Shells: " + m_TankManager.ShellCount ();
		m_ScoreText.text = m_ColoredPlayerText + m_TankManager.m_TankObject.m_PlayerScore + " points";

	}

	public void AddGuidedShell()
	{
		m_TankManager.AddGuidedShell (1);
		m_GuidedShellText.text = "Guided Shells: " + m_TankManager.GuidedShellCount ();
		m_ScoreText.text = m_ColoredPlayerText + m_TankManager.m_TankObject.m_PlayerScore + " points";

	}

	public void SubGuidedShell()
	{
		m_TankManager.SubGuidedShell (1);
		m_GuidedShellText.text = "Guided Shells: " + m_TankManager.GuidedShellCount ();
		m_ScoreText.text = m_ColoredPlayerText + m_TankManager.m_TankObject.m_PlayerScore + " points";

	}

	public void AddPlane()
	{
		m_TankManager.AddBomber (1);
		m_PlaneText.text = "Bombers: " + m_TankManager.BomberCount ();
		m_ScoreText.text = m_ColoredPlayerText + m_TankManager.m_TankObject.m_PlayerScore + " points";

	}

	public void SubPlane()
	{
		m_TankManager.SubBomber (1);
		m_PlaneText.text = "Bombers: " + m_TankManager.BomberCount ();
		m_ScoreText.text = m_ColoredPlayerText + m_TankManager.m_TankObject.m_PlayerScore + " points";

	}}
