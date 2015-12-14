using UnityEngine;
using System.Collections;

public class CloudCrafter : MonoBehaviour {

	//Fields set in the Unity inspector pane
	public int numClouds = 40; //The number of clouds to make
	public GameObject[] cloudPrefabs; //The prefabs for the clouds
	public Vector3 cloudPosMin; // Min Position of each cloud
	public Vector3 cloudPosMax; // Max position of each cloud
	public float cloudScaleMin = 1; //min scale of each cloud
	public float cloudScaleMax = 5; //max scale of each cloud
	public float cloudSpeedMult = 0.5f; //adjusts speed of clouds

	public bool ____________;

	//fields set dynamically
	public GameObject[] cloudInstances;

	void Awake(){
		// Make an array large enough to hold all the Cloud_Instances
		cloudInstances = new GameObject[numClouds];

		//Find the CloudAnchor parent GameObject
		GameObject anchor = GameObject.Find ("CloudAnchor");

		//Iterate through and make Cloud_s
		GameObject cloud;

		for (int i =0; i<numClouds; i++) {
			//Pick an int between 0 and cloudPrefabs.length - 1
			//Random.range will not ever pick as high as the top number
			int prefabNum = Random.Range(0,cloudPrefabs.Length);

			//Make an instance
			cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject;

			//Position cloud
			Vector3 cPos = Vector3.zero;
			cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
			cPos.y = Random.Range(cloudPosMin.x, cloudPosMax.y);

			//Scale cloud
			float scaleU = Random.value;
			float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

			//Smaller clouds with smaller  scaleU should be nearer the ground
			cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);

			//Smaller clouds should be further away
			cPos.z = 100 - 90*scaleU;

			//Appply these transforms to the cloud
			cloud.transform.position = cPos;
			cloud.transform.localScale = Vector3.one * scaleVal;

			//Make cloud a child of the anchor
			cloud.transform.parent = anchor.transform;

			//Add the cloud to cloudInstances
			cloudInstances[i] = cloud;



		}

	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Iterate over each cloud that was created
		foreach (GameObject cloud in cloudInstances) {
			//Get the cloud scale and position
			float scaleVal = cloud.transform.localScale.x;
			Vector3 cPos = cloud.transform.position;

			//Move larger clouds faster
			cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

			//If a cloud has moved too far to the left
			if(cPos.x <= cloudPosMin.x){
				//Move it to the far right
				cPos.x = cloudPosMax.x;
			}

			//Apply the new position to the cloud
			cloud.transform.position = cPos;
		}


	
	}
}
