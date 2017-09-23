using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankObject : ScriptableObject {
	public Color m_PlayerColor;            
	public BomberManager m_BomberManager;
	public int m_PlayerNumber;             
	public int m_PlayerScore = 1000;
	public int m_GuidedShellAmmo;
	public int m_ShellAmmo;
	public int m_BomberAmmo;
	[HideInInspector] public string m_ColoredPlayerText;
	[HideInInspector] public int m_Wins;  

}
