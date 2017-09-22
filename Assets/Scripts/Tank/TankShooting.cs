using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;  
	public int m_TargetGuided = 1;
	public int m_PlanesCount = 1;
    public Rigidbody m_Shell;  
	public Rigidbody m_GuidedShell;
	public Rigidbody m_MachineGunBullet;
	public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
	public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;
	[HideInInspector]public Transform m_PlaneSpawnPoint;
	[HideInInspector]public PlaneManager m_PlaneManager;
	[HideInInspector]public int m_GuidedShellAmmo;
	[HideInInspector]public int m_ShellAmmo;
	[HideInInspector]public Text m_AmmoText;
	[HideInInspector]public Color m_PlayerColor;
	[HideInInspector]public TankManager m_TankManager;

    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;
	private float m_FireRate = 0.1f;
	private float m_NextFire;

	// Definir qual é o botão do míssel guidado
	private string m_GuidedButton;
	private string m_MachineGunButton;
	private string m_PlaneButton;

    private void OnEnable()
    {
		m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
		m_GuidedShellAmmo++;
		m_ShellAmmo = 100;
		m_PlanesCount = 1;
    }


    private void Start()
    {
		m_FireButton = "Fire" + m_PlayerNumber;

		m_GuidedButton = "GuidedFire" + m_PlayerNumber;

		m_MachineGunButton = "MachineGun" + m_PlayerNumber;
		m_PlaneButton = "Plane" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

		m_GuidedShellAmmo = 10;
		m_PlanesCount = 10;
		m_ShellAmmo = 100;
     }

    private void Update()
    {
		FireButtonAction ();
		FireGuidedButtonAction ();
		MachineGunButtonAction ();
		PlaneButtonAction ();
		string coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";
		m_AmmoText.text = coloredPlayerText + "\n Score: " + m_TankManager.m_PlayerScore + "\n Guided shells: " + m_GuidedShellAmmo + "\n Normal shells: " + m_ShellAmmo + "\n Bombers: " + m_PlanesCount;
    }

	private void MachineGunButtonAction(){
		if(Input.GetButton(m_MachineGunButton) && Time.time >= m_NextFire)
		{
			m_NextFire = Time.time + m_FireRate;
			MachineGunFire ();
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
		if(m_ShellAmmo == 0)
			return;
		m_AimSlider.value = m_MinLaunchForce;
		if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired) {
			m_CurrentLaunchForce = m_MaxLaunchForce;
			Fire ();

		} else if(Input.GetButtonDown(m_FireButton)){
			m_Fired = false;
			m_CurrentLaunchForce = m_MinLaunchForce;
			m_ShootingAudio.clip = m_ChargingClip;
			m_ShootingAudio.Play ();
		} else if(Input.GetButton(m_FireButton) && !m_Fired){
			m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
			m_AimSlider.value = m_CurrentLaunchForce;
		} else if (Input.GetButtonUp(m_FireButton) && !m_Fired){
			Fire ();
		}
	}

	private void GuidedFire(){
		// Instantiate and launch the shell.
		if(m_GuidedShellAmmo == 0)
			return;
		m_Fired = true;
		Rigidbody shellInstance = Instantiate (m_GuidedShell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
		GuidedShellScript shell = shellInstance.GetComponent<GuidedShellScript>();
		shell.m_TargetPlayer = m_TargetGuided;
		ShellExplosion explosion = shell.GetComponent<ShellExplosion> ();
		explosion.m_PlayerNumber = this.m_PlayerNumber;
		//shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
		//TankManager targetTank = GameManager.m_Instance.FindTargetTank (m_TargetGuided);
		m_ShootingAudio.clip = m_FireClip;
		m_ShootingAudio.Play ();
		m_CurrentLaunchForce = m_MinLaunchForce;
		m_GuidedShellAmmo--;
	}

    private void Fire()
    {
        // Instantiate and launch the shell.
		m_Fired = true;
		Rigidbody shellInstance = Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
		shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
		ShellExplosion explosion = shellInstance.GetComponent<ShellExplosion> ();
		explosion.m_PlayerNumber = this.m_PlayerNumber;
		m_ShootingAudio.clip = m_FireClip;
		m_ShootingAudio.Play ();
		m_CurrentLaunchForce = m_MinLaunchForce;
		m_ShellAmmo--;

    }

	private void MachineGunFire(){
		Rigidbody shellInstance = Instantiate (m_MachineGunBullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
		shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward * 2;
		MachineGunScript fire = shellInstance.GetComponent<MachineGunScript> ();
		fire.m_TankManager = this.m_TankManager;

		//m_ShootingAudio.clip = m_FireClip;
		//m_ShootingAudio.Play ();
		m_CurrentLaunchForce = m_MinLaunchForce;
	}

	private void PlaneButtonAction(){
		if(Input.GetButtonDown(m_PlaneButton))
		{
			if (m_PlanesCount == 0)
				return;
			
			m_PlaneManager.m_Instance = Instantiate (m_PlaneManager.m_PlanePrefab, m_PlaneSpawnPoint.position, m_PlaneSpawnPoint.rotation) as GameObject;
			m_PlaneManager.m_TargetPlayer = m_TargetGuided;
			m_PlaneManager.m_PlayerNumber = this.m_PlayerNumber;
			m_PlaneManager.Setup ();
			m_PlanesCount--;
			GameManager.m_Instance.m_CameraControl.AddCameraTarget (m_PlaneManager.m_Instance.transform);
		}
	}
}