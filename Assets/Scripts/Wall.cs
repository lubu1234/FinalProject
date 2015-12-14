using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{



	void OnCollisionEnter (Collision coll)
	{

		//set's the string variable wall to affected the wall's tag
		string wall = gameObject.tag;

		//set's the string variable pt to the affected proj.'s tag
		string pt = coll.gameObject.tag;
	
		//if the proj.'s tag is equal to Water and the wall's tag is equal to Wall_Blue
		//Destroy both objects, clear projectile line, and set FollowCam's poi to the castle
		if (pt.Equals ("Water") && wall.Equals ("Wall_Blue")) {
			Destroy (coll.gameObject);
			Destroy (gameObject);
			FollowCam.S.isWaiting = true;
			ProjectileLine.S.Clear ();
			FollowCam.S.poi = MissionDemolition.S.castle;

		}

		if (pt.Equals ("Fire") && wall.Equals ("Wall_Red")) {
			
			//if the proj.'s tag is equal to Fire and the wall's tag is equal to Wall_Red
			//Destroy both objects, clear projectile line, and set FollowCam's poi to the castle

			Destroy (coll.gameObject);
			Destroy (gameObject);
			FollowCam.S.isWaiting = true;
			ProjectileLine.S.Clear ();
			FollowCam.S.poi = MissionDemolition.S.castle;

		}



	

	}

}
