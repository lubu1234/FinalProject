using UnityEngine;
using System.Collections;

public enum GameMode
{
	idle,
	playing,
	levelEnd

}

public class MissionDemolition : MonoBehaviour
{

	static public MissionDemolition S;

	//Fields set in the Unity Inspector pane
	public GameObject[] castles;
	public GUIText gtLevel;
	public GUIText gtEarth;
	public GUIText gtFire;
	public GUIText gtWater;
	public GUIText gtRestart;
	public Vector3 castlePos;
	public bool ___________;

	//fields set dynamically
	public int level; //current level
	public int levelMax; //number of levels
	public int earthGiven;
	public int fireGiven;
	public int waterGiven;
	public GameObject castle; //current castle
	public GameMode mode = GameMode.idle;
	public string showing = "Slingshot"; //follow cam mode


	// Use this for initialization
	void Start ()
	{

		S = this;

		level = 0;
		levelMax = castles.Length;
		StartLevel ();
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		ShowGT ();
		gtRestart.text = "";

		//Check for level end
		if (mode == GameMode.playing && Goal.goalMet) {
			//change mode to stop checking for level end
			mode = GameMode.levelEnd;

			//zoom out
			SwitchView ("Both");

			//start the next level in 2secs
			Invoke ("NextLevel", 2f);

			
		} else if (MissionDemolition.S.earthGiven == 0 && MissionDemolition.S.fireGiven == 0 && MissionDemolition.S.waterGiven == 0) {
			FollowCam.S.poi = GameObject.Find ("ViewBoth");
			S.showing = "";
			Slingshot.S.projectileName = "";
			gtEarth.text = "";
			gtFire.text = "";
			gtWater.text = "";
			gtRestart.text = "GAME OVER!";


		}

	}

	void StartLevel ()
	{


		//Get rid of the old castle if one exists
		if (castle != null) {
			Destroy (castle);
		}

		//Destroy old projectiles if they exist
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Earth");
		foreach (GameObject pTemp in gos) {
			Destroy (pTemp);
		}

		
		gos = GameObject.FindGameObjectsWithTag ("Water");
		foreach (GameObject pTemp in gos) {
			Destroy (pTemp);
		}

		gos = GameObject.FindGameObjectsWithTag ("Fire");
		foreach (GameObject pTemp in gos) {
			Destroy (pTemp);
		}

		//Instantiate the new castle
		castle = Instantiate (castles [level])as GameObject;
		castle.transform.position = castlePos;

		if (level == 0) {
			earthGiven = 10;
			fireGiven = 5;
			waterGiven = 5;
		}else if (level == 1) {
			earthGiven = 10;
			fireGiven = 5;
			waterGiven = 5;
		}else if (level == 2) {
			earthGiven = 5;
			fireGiven = 6;
			waterGiven = 6;
		}else if (level == 3) {
			earthGiven = 30;
			fireGiven = 6;
			waterGiven = 6;
		}

		//reset camera
		SwitchView ("Both");
		ProjectileLine.S.Clear ();

		//reset goal
		Goal.goalMet = false;

		ShowGT ();

		mode = GameMode.playing;
	}

	void NextLevel ()
	{
		level++;
		if (level == levelMax) {
			level = 0;
		}
		Slingshot.SwitchProjectile ("Earth");	
		StartLevel ();
	}

	void RestartGame(){

		Application.LoadLevel ("_Scene_0");

		
	}

	void ShowGT ()
	{
		//show the data in the gui texts
		gtLevel.text = "Level: " + (level + 1) + " of " + levelMax;
		gtEarth.text = earthGiven + " earth elements left.";
		gtFire.text = fireGiven + " fire elements left.";
		gtWater.text = waterGiven + " water elements left.";

	}

	void OnGUI ()
	{
		//draw the gui button for view switching at the top of the screen
		Rect buttonRect = new Rect ((Screen.width / 2) - 50, 10, 100, 24);

		switch (showing) {
		case "Slingshot":
			if (GUI.Button (buttonRect, "Show Castle")) {
				SwitchView ("Castle");
			}

			break;



		case "Castle":
			if (GUI.Button (buttonRect, "Show Slingshot")) {
				SwitchView ("Slingshot");
			}

			break;

		case "Both":
			if (GUI.Button (buttonRect, "Show Slingshot")) {
				SwitchView ("Slingshot");
			}
			break;

		}

		if (gtRestart.text.Equals ("GAME OVER!")) {

			if (GUI.Button(new Rect ((Screen.width / 2) - 50, 40, 100, 24), "Restart game")){
				RestartGame();				
			}
		}
		
	}
	
	
	//static method that allows code anywhere to request a view change
	static public void SwitchView (string eView)
	{
		S.showing = eView;
		switch (S.showing) {
		case "Slingshot":
			FollowCam.S.poi = null;
			break;


		case "Castle":
			FollowCam.S.poi = S.castle;
			break;


		case "Both":
			FollowCam.S.poi = GameObject.Find ("ViewBoth");
			break;

		}
	}


	//static method that allows code anywhere to increment shotsTaken
	public static void ShotFired (string projName)
	{
		if (projName.Equals ("Earth")) {
			S.earthGiven--;
		}else if (projName.Equals ("Fire")) {
			S.fireGiven--;
		}else if (projName.Equals ("Water")) {
			S.waterGiven--;
		}

	}




	

}
