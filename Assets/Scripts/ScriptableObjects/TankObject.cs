using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankObject : ScriptableObject {
	public Color m_PlayerColor;            
	public BomberManager m_BomberManager;
	public int m_PlayerNumber;             
	[HideInInspector] public int m_PlayerScore = 1000;
	[HideInInspector] public string m_ColoredPlayerText;
	[HideInInspector] public int m_Wins;  
	[HideInInspector] public int m_GuidedShellAmmo = 0;
	[HideInInspector] public int m_ShellAmmo = 0;
	[HideInInspector] public int m_BomberAmmo;

}
