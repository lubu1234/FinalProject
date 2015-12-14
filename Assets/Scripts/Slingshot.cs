using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Slingshot : MonoBehaviour
{
	
	public GameObject launchPoint;
	static public Slingshot S;

	//fields set in the Unity Inspector pane
	public static GameObject prefabProjectile;
	public float velocityMult = 4f;
	public bool _________;
	public List<GameObject> prefabProjectileList = new List<GameObject> ();
	public static List<GameObject> projectileList = new List<GameObject> ();

	//fields set dynamically
	public Vector3 launchPos;
	public bool aimingMode;
	public string projectileName = "Earth";


	public static GameObject projectile;

	void Awake()
	{
		S = this;
		Transform launchPointTrans = transform.Find ("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive (false);
		launchPos = launchPointTrans.position;

		
		for (int i = 0; i < prefabProjectileList.Count; i ++) {

			projectileList.Add (prefabProjectileList [i]);
			
		}

		prefabProjectile = projectileList [0];



	}

	void OnMouseEnter ()
	{
		//print ("Slingshot:OnMouseEnter()");
		launchPoint.SetActive (true);
	}

	void OnMouseExit ()
	{
		//print ("Slingshot:OnMouseExit()");
		launchPoint.SetActive (false);
	}

	void OnMouseDown ()
	{
		//The player has pressed the button while over Slingshot
		aimingMode = true;

		//Instantiate a projectile
		
		if (prefabProjectile.tag.Equals ("Earth") && MissionDemolition.S.earthGiven > 0) {
			projectile = Instantiate (prefabProjectile) as GameObject;
		}else if (prefabProjectile.tag.Equals ("Fire") && MissionDemolition.S.fireGiven > 0) {
			projectile = Instantiate (prefabProjectile) as GameObject;
		}else if (prefabProjectile.tag.Equals ("Water") && MissionDemolition.S.waterGiven > 0) {
			projectile = Instantiate (prefabProjectile) as GameObject;
		}


		if (projectile) {

			//Start it at the LaunchPoint
			projectile.transform.position = launchPos;

			//Set it to is Kinematic now
			projectile.rigidbody.isKinematic = true;
		}
	}

	// Use this for initialization	
	void Start ()
	{

	
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (projectile) {

			//If the Slingshot is not in the aimingMode don't run this
			if (!aimingMode)
				return;

			//Get the current mouse position in the 2D screen coordinates
			Vector3 mousePos2D = Input.mousePosition;

			//Convert the mouse position to 3D world coordinates
			mousePos2D.z = -Camera.main.transform.position.z;

			Vector3 mousePos3D = Camera.main.ScreenToWorldPoint (mousePos2D);

			//Find the delta from the launchPos to the mousePos3D
			Vector3 mouseDelta = mousePos3D - launchPos;

			//Limit mouseDelta to the radius of the Slignshot sphereCollider
			float maxMagnitude = this.GetComponent<SphereCollider> ().radius;

			if (mouseDelta.magnitude > maxMagnitude) {
				mouseDelta.Normalize ();
				mouseDelta *= maxMagnitude;
			}


			//Move the projectile to this new position
			Vector3 projPos = launchPos + mouseDelta;
			projectile.transform.position = projPos;



			if (Input.GetMouseButtonUp (0)) {
				//The mouse has been released
				aimingMode = false;
				projectile.rigidbody.isKinematic = false;
				projectile.rigidbody.velocity = -mouseDelta * velocityMult;
				FollowCam.S.poi = projectile;
				MissionDemolition.ShotFired (projectile.tag);
				projectile = null;


			}


		}

	
	}

	void OnGUI ()
	{
		//draw the gui button for projectile switching at the top of the screen
		Rect buttonRect = new Rect ((Screen.width / 2) - 100, 50, 200, 24);


		switch (projectileName) {
	
		case "Earth":
	
				if (GUI.Button (buttonRect, "Select Fire")) {
					SwitchProjectile ("Fire");
				}

			
			break;

		case "Water":
			if (GUI.Button (buttonRect, "Select Earth")) {
				SwitchProjectile ("Earth");
			}
			
			break;

			
		case "Fire":
			if (GUI.Button (buttonRect, "Select Water")) {
				SwitchProjectile ("Water");
			}
			break;
			
		}
	}


	
	//static method that allows code anywhere to request a projectile change
	static public void SwitchProjectile (string eProjectile)
	{
		S.projectileName = eProjectile;

		switch (S.projectileName) {
		case "Earth":
			prefabProjectile = projectileList [0];
			break;
			
			
		case "Water":
			prefabProjectile = projectileList [1];
			break;
			
			
		case "Fire":
			prefabProjectile = projectileList [2];
			break;
			
		}
	}




}
