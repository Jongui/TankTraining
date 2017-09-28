using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using Dictionary;

public class GameNetworkManager : NetworkManager {

	public CameraControl m_CameraControl;

	private int m_NumberOfPlayers = 0;
	private Color[] m_Colors = { Color.red, Color.blue, Color.green, Color.yellow };

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		TankShooting tankShooting = player.GetComponent<TankShooting> ();
		tankShooting.m_PlayerColor = m_Colors [m_NumberOfPlayers];
		//TankShooting.ResetPlayers ();
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		m_NumberOfPlayers++;
	}

	/*public override void OnServerDisconnect(NetworkConnection conn)
	{
		
	}*/
}
