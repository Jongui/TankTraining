using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoMenuController : MonoBehaviour {

	public Text m_MessageText;      
	public int m_PlayerNumber;

	private TankManager m_TankManager;

	/*private void Start()
	{
		m_TankManager = GameManager.m_Instance.FindTankManager (m_PlayerNumber);
		string coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_TankManager.m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

		m_MessageText.text = coloredPlayerText + m_TankManager.m_PlayerScore;
	}*/

	public void InitCanvas()
	{
		m_TankManager = GameManager.m_Instance.FindTankManager (m_PlayerNumber);
		string coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_TankManager.m_PlayerColor) + ">PLAYER " + m_PlayerNumber + " </color>";

		m_MessageText.text = coloredPlayerText + m_TankManager.m_PlayerScore + " points";
	}

}
