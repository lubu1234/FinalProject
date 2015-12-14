using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCam : MonoBehaviour
{

	static public FollowCam S; //a FollowCam singleton

	//fields set in the Unity Inspector pane
	public float easing = 0.05f;
	public Vector2 minXY;
	public bool __________;

	//fields set dynamically
	public GameObject poi; //The point of interest
	public float camZ; //The desired Z pos of the camera
	public List<GameObject> wallCount = new List<GameObject> ();
	public int isSleeping;
	public Vector3 destroyedPosition;

	[SerializeField]
	public bool isWaiting;

	void Awake ()
	{
		S = this;
		camZ = this.transform.position.z;
	}


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 destination;

		//If there is no poi, return P:[0,0,0]
		if (poi == null) {

			destination = Vector3.zero;


		} else {
			//Get the position of the poi
			destination = poi.transform.position;
			//If poi is a Projectile, check to see if it's at rest
			if (poi.tag == "Earth") {
				//If it is sleeping 

				if (poi.rigidbody.IsSleeping ()) {
					//return to default view
					poi = null;
					//Slingshot.S.projectileName = "Earth";
					//in the next update
					return;
				}

			} else if (poi.tag == "Water") {
				if (poi.rigidbody.IsSleeping ()) {
					poi = null;
					return;
				}
			} else if (poi.tag == "Fire") {
				if (poi.rigidbody.IsSleeping ()) {
					poi = null;
					return;
				}

			} else if (poi.tag == "Castle") {

				//Gathers all game objects with wall tags
				if(isWaiting) {


					GameObject[] gos = GameObject.FindGameObjectsWithTag ("Wall");
					foreach (GameObject pTemp in gos) {
						wallCount.Add (pTemp);
					}
				
				
					gos = GameObject.FindGameObjectsWithTag ("Wall_Red");
					foreach (GameObject pTemp in gos) {
						wallCount.Add (pTemp);
					}
				
					gos = GameObject.FindGameObjectsWithTag ("Wall_Blue");
					foreach (GameObject pTemp in gos) {
						wallCount.Add (pTemp);
					}

					try{
					//Checks to see if every game object in wallCount list is sleeping
					for (int i = 0; i < wallCount.Count; i++) {
						if (wallCount [i].rigidbody.IsSleeping ()) {
							isSleeping++;
						}

					}
					}catch(MissingReferenceException e){
						wallCount.Clear ();
						
						gos = GameObject.FindGameObjectsWithTag ("Wall");
						foreach (GameObject pTemp in gos) {
							wallCount.Add (pTemp);
						}
						
						
						gos = GameObject.FindGameObjectsWithTag ("Wall_Red");
						foreach (GameObject pTemp in gos) {
							wallCount.Add (pTemp);
						}
						
						gos = GameObject.FindGameObjectsWithTag ("Wall_Blue");
						foreach (GameObject pTemp in gos) {
							wallCount.Add (pTemp);
						}

					}

					//If all of them are sleeping then set poi to null
					if (isSleeping >= wallCount.Count) {
						wallCount.Clear ();
						isSleeping = 0;
						isWaiting = false;
						poi = null;
						return;
					}


				}else{

					destination = poi.transform.position;

				}


				
				
			}

		}


		//Limit the X & Y values
		destination.x = Mathf.Max (minXY.x, destination.x);
		destination.y = Mathf.Max (minXY.y, destination.y);

		//Interpolate from the current Camera position toward destination
		destination = Vector3.Lerp (transform.position, destination, easing);

		//Debug.Log (destination);
			
		//retain a destination.z of camZ
		destination.z = camZ;

		//Set the camera to the destination
		transform.position = destination;

		//Set the orthographicSize of the Camera to keep Ground in view
		this.camera.orthographicSize = destination.y + 10;


	


		
	}
}
