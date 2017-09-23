using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public int m_NumRoundsToPlay;
	public float m_StartDelay = 3f;         
	public float m_EndDelay = 3f;           
	public CameraControl m_CameraControl;   
	public Text m_MessageText;      
	public GameObject m_TankPrefab;
	public TankManager[] m_Tanks; 
	public Transform m_SpawnPoint;
	public int m_StartScore;
	[HideInInspector] public static GameManager m_Instance;

	private int m_RoundNumber;              
	private WaitForSeconds m_StartWait;     
	private WaitForSeconds m_EndWait;       
	private TankManager m_RoundWinner;
	private TankManager m_GameWinner;



	private void Awake()
	{
		if (m_Instance == null) {
			m_Instance = this;
		}
	}

	private void Start()
	{
		
		SpawnAllTanks ();
		//m_Instance = this;
	}

	public void StartGame()
	{
		m_StartWait = new WaitForSeconds(m_StartDelay);
		m_EndWait = new WaitForSeconds(m_EndDelay);

		SetCameraTargets();

		StartCoroutine(GameLoop());

	}

	private void SpawnAllTanks()
	{
		for (int i = 0; i < m_Tanks.Length; i++)
		{
			m_Tanks[i].m_Instance =
				Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
			m_Tanks[i].m_TankObject.m_PlayerNumber = i + 1;
			//DontDestroyOnLoad (m_Tanks[i].gameObject);
			//m_Tanks [i].m_PlayerScore = 1000;
			m_Tanks[i].Setup();
		}
	}

	private void SetCameraTargets()
	{
		Transform[] targets = new Transform[m_Tanks.Length];

		int i = 0;

		for (i = 0; i < m_Tanks.Length; i++)
		{
			targets[i] = m_Tanks[i].m_Instance.transform;
		}

		m_CameraControl.m_Targets = targets;
	}


	private IEnumerator GameLoop()
	{
		yield return StartCoroutine(RoundStarting());
		yield return StartCoroutine(RoundPlaying());
		yield return StartCoroutine(RoundEnding());

		if (m_GameWinner != null)
		{
			AudioSource gameSong = m_Instance.GetComponent<AudioSource> ();
			gameSong.Stop ();
			SceneManager.LoadScene(0);
		}
		else
		{
			StartCoroutine(GameLoop());
		}
	}


	private IEnumerator RoundStarting()
	{
		ResetAllTanks ();
		DisableTankControl ();
		m_CameraControl.SetStartPositionAndSize ();
		m_RoundNumber++;
		m_MessageText.text = "ROUND " + m_RoundNumber;
		yield return m_StartWait;
	}


	private IEnumerator RoundPlaying()
	{
		EnableTankControl ();
		m_MessageText.text = string.Empty;
		while(!OneTankLeft()){
			yield return null;
		}
	}


	private IEnumerator RoundEnding()
	{
		DestroyAllPlanes ();
		DisableTankControl ();
		m_RoundWinner = null;
		m_RoundWinner = GetRoundWinner ();
		if (m_RoundWinner != null) {
			m_RoundWinner.m_TankObject.m_Wins++;
		}
		if(m_RoundNumber == m_NumRoundsToPlay)
			m_GameWinner = GetGameWinner ();
		string message = EndMessage ();
		m_MessageText.text = message;
		yield return m_EndWait;
	}


	private bool OneTankLeft()
	{
		int numTanksLeft = 0;

		for (int i = 0; i < m_Tanks.Length; i++)
		{
			if (m_Tanks[i].m_Instance.activeSelf)
				numTanksLeft++;
		}

		return numTanksLeft <= 1;
	}

	private TankManager GetRoundWinner()
	{
		for (int i = 0; i < m_Tanks.Length; i++)
		{
			if (m_Tanks[i].m_Instance.activeSelf)
				return m_Tanks[i];
		}

		return null;
	}


	private TankManager GetGameWinner()
	{
		int index = 0;
		int maxScore = 0;
		for (int i = 0; i < m_Tanks.Length; i++)
		{
			if (m_Tanks [i].m_TankObject.m_PlayerScore > maxScore) 
			{
				maxScore = m_Tanks [i].m_TankObject.m_PlayerScore;
				index = i;
			}
		}

		return m_Tanks[index];
	}


	private string EndMessage()
	{
		string message = "DRAW!";

		if (m_RoundWinner != null)
			message = m_RoundWinner.m_TankObject.m_ColoredPlayerText + " WINS THE ROUND!";

		message += "\n\n\n\n";

		for (int i = 0; i < m_Tanks.Length; i++)
		{
			message += m_Tanks[i].m_TankObject.m_ColoredPlayerText + ": " + m_Tanks[i].m_TankObject.m_Wins + " WINS\n";
		}

		if (m_GameWinner != null)
			message = m_GameWinner.m_TankObject.m_ColoredPlayerText + " WINS THE GAME!";

		return message;
	}

	private void ResetAllTanks()
	{
		for (int i = 0; i < m_Tanks.Length; i++)
		{
			m_Tanks[i].Reset();
		}
	}


	private void EnableTankControl()
	{
		for (int i = 0; i < m_Tanks.Length; i++)
		{
			m_Tanks[i].EnableControl();
		}
	}


	private void DisableTankControl()
	{
		for (int i = 0; i < m_Tanks.Length; i++)
		{
			m_Tanks[i].DisableControl();
		}
	}

	private void DestroyAllPlanes(){
		GameObject[] planeObjects =  GameObject.FindGameObjectsWithTag ("Plane");

		for (int i = 0; i < planeObjects.Length; i++) {
			Destroy (planeObjects [i]);
		}
	}

	public TankManager FindTankManager(int tank){
		for (int i = 0; i < m_Tanks.Length; i++) {
			if (m_Tanks [i].m_TankObject.m_PlayerNumber == tank)
				return m_Tanks [i];
		}
		return null;
	}

	public void Quit()
	{
		for (int i = 0; i < m_Tanks.Length; i++)
			m_Tanks [i].m_TankObject.m_PlayerScore = m_StartScore;
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit ();
		#endif
	}
}