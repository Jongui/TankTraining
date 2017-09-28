using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TankShooting : NetworkBehaviour
{
    public int m_PlayerNumber = 1;  
	public int m_TargetGuided = 1;
	public GameObject m_Shell;  
	public Rigidbody m_GuidedShell;
	public GameObject m_MachineGunBullet;
	public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
	public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;
	[HideInInspector]public Transform m_PlaneSpawnPoint;
	[HideInInspector]public BomberManager m_BomberManager;
	[HideInInspector]public int m_GuidedShellAmmo = 0;
	[HideInInspector]public int m_ShellAmmo = 0;
	[HideInInspector]public int m_BomberAmmo;
	[HideInInspector]public Text m_AmmoText;
	[SyncVar(hook = "ChangeTankColor")]
	[HideInInspector]public Color m_PlayerColor;
	[HideInInspector]public TankManager m_TankManager;

    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;
	private float m_CmdFireRate = 0.1f;
	private float m_NextFire;

	// Definir qual é o botão do míssel guidado
	private string m_GuidedButton;
	private string m_MachineGunButton;
	private string m_BomberButton;
	private ColorControl[] m_ColorControl;

	private static int m_NumberOfPlayers = 0;
	private static Color[] m_Colors = { Color.red, Color.blue, Color.green, Color.yellow };

    private void OnEnable()
    {
		m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
		//m_PlanesCount = 1;
    }


    private void Start()
    {
		InitColorControl ();
		m_FireButton = "Fire";

		m_GuidedButton = "GuidedFire";

		m_MachineGunButton = "MachineGun";
		m_BomberButton = "Plane";

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

		ColorControl cc = m_ColorControl [m_NumberOfPlayers];
		ChangeTankColor (cc.playerColor);
		m_NumberOfPlayers++;
		if (m_NumberOfPlayers >= m_ColorControl.Length)
			m_NumberOfPlayers = 0;

		//m_PlanesCount = 10;
     }

	public static void ResetPlayers()
	{
		m_NumberOfPlayers = 0;
	}

    private void Update()
    {
		if (!isLocalPlayer)
			return;
		FireButtonAction ();
		FireGuidedButtonAction ();
		MachineGunButtonAction ();
		BomberButtonAction ();
		//string coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";
		//m_AmmoText.text = coloredPlayerText + "\n Score: " + m_TankManager.m_TankObject.m_PlayerScore + "\n Guided shells: " + m_GuidedShellAmmo + "\n Normal shells: " + m_ShellAmmo + "\n Bombers: " + m_BomberAmmo;
    }

	private void MachineGunButtonAction(){
		if(Input.GetButton(m_MachineGunButton) && Time.time >= m_NextFire)
		{
			m_NextFire = Time.time + m_CmdFireRate;
			CmdMachineGunCmdFire ();
		}
	}
	private void FireGuidedButtonAction(){
		if(Input.GetButtonDown(m_GuidedButton))
		{
			GuidedFire ();
		}
	}
	private void FireButtonAction(){
		// Track the current state of the fire button and make decisions based on the current launch force.
		//if(m_ShellAmmo == 0)
		//	return;
		m_AimSlider.value = m_MinLaunchForce;
		if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired) {
			m_Fired = true;
			m_CurrentLaunchForce = m_MaxLaunchForce;
			CmdFire ();

		} else if(Input.GetButtonDown(m_FireButton)){
			m_Fired = false;
			m_CurrentLaunchForce = m_MinLaunchForce;
			m_ShootingAudio.clip = m_ChargingClip;
			m_ShootingAudio.Play ();
		} else if(Input.GetButton(m_FireButton) && !m_Fired){
			m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
			m_AimSlider.value = m_CurrentLaunchForce;
		} else if (Input.GetButtonUp(m_FireButton) && !m_Fired){
			m_Fired = true;
			CmdFire ();
		}
	}

	private void GuidedFire(){
		// Instantiate and launch the shell.
		if(m_GuidedShellAmmo == 0)
			return;
		Rigidbody shellInstance = Instantiate (m_GuidedShell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
		GuidedShellScript shell = shellInstance.GetComponent<GuidedShellScript>();
		shell.m_TargetPlayer = m_TargetGuided;
		ShellExplosion explosion = shell.GetComponent<ShellExplosion> ();
		explosion.m_PlayerNumber = this.m_PlayerNumber;
		//shellInstance.velocity = m_CurrentLaunchForce * m_CmdFireTransform.forward;
		//TankManager targetTank = GameManager.m_Instance.FindTargetTank (m_TargetGuided);
		m_ShootingAudio.clip = m_FireClip;
		m_ShootingAudio.Play ();
		m_CurrentLaunchForce = m_MinLaunchForce;
		m_GuidedShellAmmo--;
	}

	[Command]
    private void CmdFire()
    {
        // Instantiate and launch the shell.
		m_Fired = true;
		GameObject shellInstance = Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as GameObject;
		Rigidbody rigidbody = shellInstance.GetComponent<Rigidbody> ();
		rigidbody.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
		ShellExplosion explosion = shellInstance.GetComponent<ShellExplosion> ();
		explosion.m_PlayerNumber = this.m_PlayerNumber;
		//explosion.m_TankMask = this.gameObject.layer;
		m_ShootingAudio.clip = m_FireClip;
		m_ShootingAudio.Play ();
		m_CurrentLaunchForce = m_MinLaunchForce;
		m_ShellAmmo--;
		NetworkServer.Spawn(shellInstance);

    }

	[Command]
	private void CmdMachineGunCmdFire(){
		GameObject shellInstance = Instantiate (m_MachineGunBullet, m_FireTransform.position, m_FireTransform.rotation) as GameObject;
		Rigidbody rigidbody = shellInstance.GetComponent<Rigidbody> ();
		rigidbody.velocity = m_CurrentLaunchForce * m_FireTransform.forward * 2;
		MachineGunScript fire = shellInstance.GetComponent<MachineGunScript> ();
		fire.m_PlayerNumber = this.m_PlayerNumber;

		//m_ShootingAudio.clip = m_CmdFireClip;
		//m_ShootingAudio.Play ();
		m_CurrentLaunchForce = m_MinLaunchForce;
		NetworkServer.Spawn(shellInstance);
	}

	private void BomberButtonAction(){
		if(Input.GetButtonDown(m_BomberButton))
		{
			if (m_BomberAmmo == 0)
				return;
			
			m_BomberManager.m_Instance = Instantiate (m_BomberManager.m_BomberPrefab, m_PlaneSpawnPoint.position, m_PlaneSpawnPoint.rotation) as GameObject;
			m_BomberManager.m_TargetPlayer = m_TargetGuided;
			m_BomberManager.m_PlayerNumber = this.m_PlayerNumber;
			m_BomberManager.Setup ();
			m_BomberAmmo--;
			GameManager.m_Instance.m_CameraControl.AddCameraTarget (m_BomberManager.m_Instance.transform);
		}
	}

	private void ChangeTankColor(Color newColor)
	{
		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].material.color = newColor;
		}
	}
	private void InitColorControl()
	{
		m_ColorControl = new ColorControl[4];
		for (int i = 0; i < 4; i++) 
		{
			ColorControl cc = new ColorControl();
			cc.playerNumber = i;
			cc.playerColor = m_Colors [i];
			m_ColorControl [i] = cc;
		}
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (isLocalPlayer)
			m_NumberOfPlayers = 0;
	}

}