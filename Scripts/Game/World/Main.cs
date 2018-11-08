using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	public MainMenu mainMenu;
	public MainGeneration mainGeneration;
	public AudioControl audioControl;
	public GameObject gameCanvas;
	public GameObject shopUI;
	public GameObject researchOutpostUI;
	public GameObject clueUI;
	public GameObject shipNavigationUI;
	public GameObject SystemInfoUI;
	public GameObject gameMenuUI;
	public GameObject quitConfimationBoxMenu;
	public GameObject quitConfimationBoxDesktop;
	public GameObject gameSavedMessage;
	public GameObject systemJumpConfimationBox;
	public GameObject driveJumpConfimationBox;
	public GameObject autoPilotConfimationBox;
	public GameObject finalEventBox;
	public GameObject shipDestroyedBox;

	public Text interactionText;
	public GameObject interactionUI;

	public InputField inputField;

	public Text[] text;
	public Text[] shipText;

	//bool jumping;

	public EventSystem eventSystem;
	public int eventNumber;


	public Ship ship;

	public bool shipOnOuterEdge;

	public CameraControl[] cameras;

	public int mainEventProgress;

	public Text gameViewButtonText;

	public bool mainGame;
	public bool systemGenerator;
	//public bool inEvent;

	public Clue clue;
	public bool[] clueKnown;
	public Text[] clueText;

	public GameObject navigationLinePoint;
	GameObject currentSystemRing;

	public int targetSystem;

	bool systemJumping;
	int nextSystemJump;
	float systemJumpProgress;
	float systemJumpSpeed;

	bool driveJumping;
	int nextjumpDrive;
	float jumpDriveProgress;
	float jumpDriveSpeed;

	//bool autoPilotActive;

	public Slider driveModeSlider;
	public Toggle autoPilotToggle;

	public Canvas blockingCanvas;

	public void StartMain (GameData gameData){

		mainGeneration.GenerateGalaxy (gameData);

		if (gameData.savedGame == false) {
			NewGame (gameData);
		} else {
			LoadGame (gameData);
		}

		mainEventProgress = gameData.mainEventProgress;
	
		OpenGameUI ();

		mainGame = true;
		navigationLinePoint = DrawNavigationLine (new Vector3[]{mainGeneration.systemPoint[0].position});
		navigationLinePoint.transform.parent = transform.GetChild (0);

		SetupNewTargetSystem (gameData.targetSystem);
	

		GameObject ob = Instantiate(mainGeneration.shipGameObject [(int)gameData.shipType],ship.transform.position,mainGeneration.shipGameObject [(int)gameData.shipType].transform.rotation,ship.transform);



		clueText [0].text = "Number of Planets:";
		clueText [1].text = "Has Asteroid Belt:";
		clueText [2].text = "Star Class:";
		clueText [3].text = "Distance:";
		clueText [4].text = "System Jumps Away:";


		currentSystemRing = CreateCurrentSystemRing (5);
		currentSystemRing.transform.parent = transform.GetChild (0);
		currentSystemRing.transform.position = mainGeneration.systemPoint [gameData.startingSystem].position;
	}
	public void LoadGame (GameData gameData){

		Vector3 newPos = new Vector3 (gameData.x, 25, gameData.z);
		ship.SetUpShip (newPos,gameData.shipType,gameData.commander, gameData.strength, gameData.money, gameData.jumpCharges, gameData.research);

		CalculateNumberOfSystemJumpsBetweenSystems (mainGeneration.currentSystem,mainGeneration.currentSystem, 0);


		clue = gameData.clue;
		clueKnown = gameData.clueKnown;

		targetSystem = gameData.targetSystem;

		UpdateClueDisplay ();


	}
	public void NewGame (GameData gameData){

		ship.SetUpShip (gameData.shipType,gameData.commander);

		foreach (Transform child in transform.GetChild(1)) {
			if (child.tag == "MainEventPoint") {
				ship.transform.position = new Vector3 (child.position.x, 25, child.position.z);
			}
		}


		OpenEvent (eventSystem.mainEventList[0]);

	}
	public void DisplayGameSavedMessage (){

		gameSavedMessage.SetActive (true);
		StartCoroutine (GameSavedTime(2));

	}
	IEnumerator GameSavedTime (int _time){

		yield return new WaitForSeconds (_time);
		gameSavedMessage.SetActive (false);
	}
	public void SaveGame (){
		GameData newGameData = new GameData ();
		newGameData.clue = clue;
		newGameData.clueKnown = clueKnown;
		newGameData.galaxyType = mainGeneration.galaxyType;
		newGameData.numberOfSystems = mainGeneration.starSystem.Length;
		newGameData.seed = mainGeneration.seed;
		newGameData.startingSystem = mainGeneration.currentSystem;
		newGameData.mainEventProgress = mainEventProgress;
		newGameData.targetSystem = targetSystem;

		bool[,] anomaliCompleted = new bool[newGameData.numberOfSystems, 2];

		for (int i = 0; i < newGameData.numberOfSystems; i++) {
			for (int j = 0; j < mainGeneration.starSystem [i].anomaliEventCompleted.Length; j++) {
				anomaliCompleted [i, j] = mainGeneration.starSystem [i].anomaliEventCompleted [j];
			}
		}

		newGameData.anomaliCompleted = anomaliCompleted;


		newGameData.shipType = ship.shipType;
		newGameData.strength = ship.strength;
		newGameData.commander = ship.commander;
		newGameData.money = ship.money;
		newGameData.jumpCharges = ship.jumpCharges;
		newGameData.research = ship.research;
		newGameData.x = ship.transform.position.x;
		newGameData.z = ship.transform.position.z;

		newGameData.savedGame = true;



		SaveLoadSystem.Save (newGameData);

		DisplayGameSavedMessage ();
	}

	void Update (){

	//	if (Input.GetMouseButtonDown (0)) {
		//	audioControl.PlaySoundEffect (0);
		//}

		if (audioControl.source [0].isPlaying == false) {
			audioControl.PlayAmbience (Random.Range (0, audioControl.ambience.Length));

		}

		if (systemGenerator) {
			mainMenu.systemGenerationModeMain.UpdateSystemGenerator ();
			cameras[1].UpdateCamera (false);
			return;
		}
		if (mainGame == false)
			return;
		
		//if (inEvent == true)
		//	return;

		UpdateGame ();


	}
	void UpdateGame (){

		if (finalEventBox.activeSelf) {
			return;
		}
		if (shipDestroyedBox.activeSelf) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (eventSystem.eventObject.activeSelf) {
				return;
			} else if (researchOutpostUI.activeSelf) {
				return;
			} else if (shopUI.activeSelf) {
				return;
			} else if (autoPilotToggle.isOn) {
				CancelAutoPilotConfimation ();
				autoPilotToggle.isOn = false;
			} else if (blockingCanvas.sortingOrder == 3) {
				OpenQuitConfimationBoxMenu (false);
				OpenQuitConfimationBoxDesktop (false);

			} else if (gameMenuUI.activeSelf) {
				OpenGameMenu (false);
			} else if (blockingCanvas.sortingOrder == 1 && blockingCanvas.gameObject.activeSelf) {
				CancelDriveJumpConfimation ();
				CancelSystemJumpConfimation ();

			} else {
				OpenGameMenu (true);
			}
		}

		if (blockingCanvas.gameObject.activeSelf) {
			return;
		}

		if (autoPilotToggle.isOn) {
			UpdateAutoPilot ();
		}
		if (systemJumping) {
			UpdateSystemJump ();
			return;
		}
		if (driveJumping) {
			UpdateJumpDrive ();
			return;
		}

		if (mainEventProgress == 10) {

			FinalEvent ();
			SetBlockingCanvas (true, 1);
		}

		for (int i = 0; i < 2; i++) {
			cameras[i].UpdateCamera (mainMenu.cameraFollowShip.isOn);
		}

		if (ship.gameObject.activeSelf) {
			if (ship.UpdateShip (mainMenu.holdToMove.isOn) == false) {
				ShipDestroyed ();
				SetBlockingCanvas (true, 1);
			}

			Collider[] overlaps;
			overlaps = Physics.OverlapSphere (ship.transform.position, 5);

			if (overlaps.Length > 0) {
				interactionUI.SetActive (true);
				interactionText.text = overlaps [0].tag;
			} else {
				interactionUI.SetActive (false);
			}

			if (Input.GetKeyDown (KeyCode.E)) {

				InteractionCheck ();

			}
		}

		if (ship.money < 0) {
			ship.money = 0;
		}
		if (ship.research < 0) {
			ship.research = 0;
		}
		if (ship.jumpCharges < 0) {
			ship.jumpCharges = 0;
		}

		shipText [0].text = ship.strength.ToString () + "/" + ship.maxStrength.ToString ();
		shipText [1].text = ship.money.ToString ();
		shipText [2].text = ship.research.ToString ();
		shipText [3].text = ship.jumpCharges.ToString () + "/" + ship.maxJumpCharges.ToString ();

			

		if (Vector3.Distance (transform.position, ship.transform.position) > StarSystemGenerator.GetStarSystemEdgeDistance(mainGeneration.starSystem[mainGeneration.currentSystem].seed)) {
			shipOnOuterEdge = true;
		} else {
			shipOnOuterEdge = false;
		}

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) {

			if (hit.transform.tag == "SystemPoint") {


				mainGeneration.currentHoverSystem = hit.transform.gameObject.GetComponent<SystemPoint> ().GetStarSystem ();

				SystemInfoUI.SetActive (true);

//				if (Input.mousePosition.y > (Screen.height / 10) *9) {
//
//
//				}

				SystemInfoUI.transform.position = Input.mousePosition;

				StarSystemGenerator.StarSystemInfo ssi = StarSystemGenerator.GetStarSystemInfo (mainGeneration.starSystem [mainGeneration.currentHoverSystem].seed);
			
				text [0].text = "Number of Planets: " + ssi.numberOfPlanets.ToString ();
				text [1].text = "Has Asteroid Belt: " + ssi.asteroidBelt.ToString ();
				text [2].text = "Star Class: " + ssi.starType.ToString ();
				text [3].text = "Distance: " + (int)GetSystemDistance (mainGeneration.currentSystem, mainGeneration.currentHoverSystem);
				text [4].text = "System Jumps Away: " + mainGeneration.systemPoint [mainGeneration.currentHoverSystem].numberOfJumpsAway;


				if (Input.GetMouseButtonDown (0)) {

					//if (jumping) {
						if(driveModeSlider.value == 0){
						if (CheckSystemDistance (mainGeneration.currentSystem, mainGeneration.currentHoverSystem, ship.maxJumpDistance)) {
							driveJumpConfimationBox.SetActive (true);
							driveJumpConfimationBox.transform.position = Input.mousePosition;
							audioControl.PlaySoundEffect (2);
							SetBlockingCanvas (true, 1);
						}


					} else {
						if (CheckSystemConnection (mainGeneration.currentSystem, mainGeneration.currentHoverSystem)) {
							if (shipOnOuterEdge) {
								systemJumpConfimationBox.SetActive (true);
								systemJumpConfimationBox.transform.position = Input.mousePosition;
								audioControl.PlaySoundEffect (2);
								SetBlockingCanvas (true, 1);
							}
						}
					}



				} else if (Input.GetMouseButtonDown (1)) {

					SetupNewTargetSystem (mainGeneration.currentHoverSystem);
					UpdateClueDisplay ();
				}



			}
		} else {
			SystemInfoUI.SetActive (false);
		}			

	}
	public void ConfirmSystemJump(){
		StartSystemJumpProgress (mainGeneration.currentHoverSystem);
		systemJumpConfimationBox.SetActive (false);
		SetBlockingCanvas (false, 1);
		//audioControl.PlaySoundEffect (0);
	}
	public void ConfirmDriveJump(){
		StartJumpDriveProgress (mainGeneration.currentHoverSystem);
		driveJumpConfimationBox.SetActive (false);
		SetBlockingCanvas (false, 1);
		//audioControl.PlaySoundEffect (0);
	}
	public void CancelSystemJumpConfimation(){
		systemJumpConfimationBox.SetActive (false);
		SetBlockingCanvas (false, 1);
		//audioControl.PlaySoundEffect (1);
	}
	public void CancelDriveJumpConfimation(){
		driveJumpConfimationBox.SetActive (false);
		SetBlockingCanvas (false, 1);
		//audioControl.PlaySoundEffect (1);
	}
	public void OpenAutoPilotConfimationBox(){
		autoPilotConfimationBox.SetActive (true);
		SetBlockingCanvas (true, 3);
		audioControl.PlaySoundEffect (2);
	}
	public void AutoPilotToggleSwitch(){

		if (autoPilotToggle.isOn) {
			if (shipOnOuterEdge) {
				OpenAutoPilotConfimationBox ();			
			} else {
				autoPilotToggle.isOn = false;
			}

		} else {
			autoPilotToggle.isOn = false;
		}
	}
	public void ConfirmAutoPilot(){		
		StartAutoPilot ();
		autoPilotConfimationBox.SetActive (false);
		SetBlockingCanvas (false, 1);
		//audioControl.PlaySoundEffect (0);
	}
	public void CancelAutoPilotConfimation(){
		autoPilotToggle.isOn = false;
		autoPilotConfimationBox.SetActive (false);
		SetBlockingCanvas (false, 1);
		//audioControl.PlaySoundEffect (1);
	}
	public void SetupNewTargetSystem(int target){

		targetSystem = target;

		//ClearNumberOfSystemJumps ();
		//CalculateNumberOfSystemJumpsBetweenSystems (mainGeneration.currentSystem, mainGeneration.currentSystem, 0);
		UpdateNavigationLinePoints (mainGeneration.currentSystem, targetSystem, mainGeneration.systemPoint [targetSystem].numberOfJumpsAway);
		ClearNumberOfSystemJumps ();
		CalculateNumberOfSystemJumpsBetweenSystems (mainGeneration.currentSystem, mainGeneration.currentSystem, 0);

	}
	public void InteractionCheck (){


		Collider[] overlaps;
		overlaps = Physics.OverlapSphere (ship.transform.position, 5);

		for (int i = 0; i < overlaps.Length; i++) {

			switch (overlaps [i].tag) {

			case "EventPoint":
				if (mainGeneration.starSystem [mainGeneration.currentSystem].anomaliEventCompleted [overlaps [i].GetComponent<EventPoint> ().GetEventNumber ()] == false) {
					OpenEvent (eventSystem.eventList[overlaps [i].GetComponent<EventPoint> ().GetEventListNumber ()]);
					mainGeneration.starSystem [mainGeneration.currentSystem].anomaliEventCompleted [overlaps [i].GetComponent<EventPoint> ().GetEventNumber ()] = true;
				}
				break;
			case "MainEventPoint":
				if (mainGeneration.currentSystem == mainEventProgress) {
					OpenEvent (eventSystem.mainEventList[mainEventProgress]);
					//mainGeneration.starSystem [mainGeneration.currentSystem].mainEventCompleted = true;
				}
				break;
			case "Trader":
				OpenTrader (true);
				break;
			case "ResearchOutpost":
				OpenResearchOutpost (true);
				break;

			}
				
		}

	}
	public void OpenGameMenu(bool _active){
		gameMenuUI.SetActive (_active);
		SetBlockingCanvas (_active, 1);
		//blockingCanvas.gameObject.SetActive(_active);
	}
	public void QuitToMenu() {
		SceneManager.LoadScene (0);
	}
	public void QuitToDesktop() {
		Application.Quit ();
	}
	public void ResetGame() {
		SceneManager.LoadScene (0);
	}
	public void OpenQuitConfimationBoxMenu(bool _active){

		quitConfimationBoxMenu.SetActive (_active);

		if (_active) {
			SetBlockingCanvas (true, 3);
		} else {
			SetBlockingCanvas (true, 1);
		}
		audioControl.PlaySoundEffect (2);
	}
	public void OpenQuitConfimationBoxDesktop(bool _active){

		quitConfimationBoxDesktop.SetActive (_active);
		if (_active) {
			SetBlockingCanvas (true, 3);
		} else {
			SetBlockingCanvas (true, 1);
		}
		audioControl.PlaySoundEffect (2);
	}
	public void SetBlockingCanvas(bool _active,int _order){
		
		blockingCanvas.gameObject.SetActive(_active);
		blockingCanvas.sortingOrder = _order;
	}
	public void SaveAndQuitToMenu (){
		SaveGame ();
		QuitToMenu ();
	}
	public void SaveAndQuitToDesktop(){
		SaveGame ();
		QuitToDesktop ();
	}

	public bool CheckSystemConnection (int system1,int system2){

		for (int i = 0; i < mainGeneration.systemPoint [system1].connectedSystems.Count; i++) {
			if (mainGeneration.systemPoint [system1].connectedSystems [i] == system2) {
				return true;
			}
		}
		return false;
	}
	public bool CheckSystemDistance (int system1,int system2,float distance){

		if (Vector3.Distance(mainGeneration.systemPoint [system1].position,mainGeneration.systemPoint [system2].position) <= distance) {
			return true;
		}

		return false;
	}
	public float GetSystemDistance (int system1,int system2){
		return Vector3.Distance (mainGeneration.systemPoint [system1].position, mainGeneration.systemPoint [system2].position);
	}
	public void MoveBetweenSystems (int _targetSystem,bool _active){

		StarSystemGenerator.StarSystemInfo ssi = mainGeneration.GenerateSystem(mainGeneration.starSystem[_targetSystem],_active);

		ship.transform.position = GetPosition(GetDegrees(GetHeading (mainGeneration.systemPoint [_targetSystem].position, mainGeneration.systemPoint [mainGeneration.currentSystem].position).normalized),(ssi.systemSize + 20));

		//SystemViewMode ();
		if (targetSystem == mainGeneration.currentSystem) {
			targetSystem = _targetSystem;
		} 
		mainGeneration.currentSystem = _targetSystem;
		//mainGeneration.currentSystemObject.transform.position = mainGeneration.systemPoint [mainGeneration.currentSystem].position;
		UpdateCurrentSystemRing(mainGeneration.systemPoint [mainGeneration.currentSystem].position);
	
		ClearNumberOfSystemJumps ();
		CalculateNumberOfSystemJumpsBetweenSystems (mainGeneration.currentSystem,mainGeneration.currentSystem, 0);
		UpdateClue ();
		UpdateClueDisplay ();

		//transform.GetChild (1).gameObject.SetActive (_active);

	}
	public void SwitchGameView (){

		transform.GetChild (0).gameObject.SetActive (!transform.GetChild (0).gameObject.activeSelf);
		transform.GetChild (1).gameObject.SetActive (!transform.GetChild (0).gameObject.activeSelf);
		cameras [0].gameObject.SetActive (!cameras [0].gameObject.activeSelf);
		cameras [1].gameObject.SetActive (!cameras [1].gameObject.activeSelf);

		cameras [0].transform.position = mainGeneration.systemPoint[mainGeneration.currentSystem].position + new Vector3 (0, 100, -50);
		cameras [1].transform.position = new Vector3 (ship.transform.position.x, 250, ship.transform.position.z -250);
		ship.gameObject.SetActive (!ship.gameObject.activeSelf);


		gameViewButtonText.text = (gameViewButtonText.text == "Galaxy View" ? "System View" :"Galaxy View");
	}
	public void GalaxyViewMode (){
		transform.GetChild (0).gameObject.SetActive (true);
		transform.GetChild (1).gameObject.SetActive (false);
		cameras [0].gameObject.SetActive (true);
		cameras [1].gameObject.SetActive (false);

		cameras [0].transform.position = mainGeneration.systemPoint[mainGeneration.currentSystem].position + new Vector3 (0, 100, -50);
		//cameras [1].transform.position = new Vector3 (0, 250, -250);
		ship.gameObject.SetActive (false);
		
		//SystemInfoUI.SetActive (true);
		gameViewButtonText.text = "System View";
	}
	public void SystemViewMode (){
		transform.GetChild (0).gameObject.SetActive (false);
		transform.GetChild (1).gameObject.SetActive (true);
		cameras [0].gameObject.SetActive (false);
		cameras [1].gameObject.SetActive (true);

		//cameras [0].transform.position = mainGeneration.systemPoint[mainGeneration.currentSystem].position + new Vector3 (0, 100, -50);
		cameras [1].transform.position = new Vector3 (ship.transform.position.x, 250,ship.transform.position.z -250);
		ship.gameObject.SetActive (true);

		SystemInfoUI.SetActive (false);
		gameViewButtonText.text = "Galaxy View";
	}
	//public void SwitchJumpDriveMode (){

	//	jumping = !jumping;
	//	GalaxyViewMode ();
	//}
	//public void SwitchSystemDriveMode (){

	//	jumping = false;
	//	GalaxyViewMode ();
	//}

	public static int IntParseFast(string value)
	{
		int result = 0;
		for (int i = 0; i < value.Length; i++)
		{
			char letter = value[i];
			result = 10 * result + (letter - 48);
		}
		return result;
	}
	public void OpenEvent (int _event){
		eventSystem.OpenEvent (_event);
		//inEvent = true;
		//shipNavigationUI.SetActive (false);
		SetBlockingCanvas (true, 1);
		audioControl.PlaySoundEffect (3);
	}
	public void OpenTrader (bool _active){
		shopUI.SetActive (_active);
		SetBlockingCanvas (_active, 1);
		audioControl.PlaySoundEffect (4);
	}
	public void OpenResearchOutpost (bool _active){
		researchOutpostUI.SetActive (_active);
		SetBlockingCanvas (_active, 1);
		audioControl.PlaySoundEffect (4);
	}
	public void BuyJumpCharge (){
		bool successful;
		if (ship.jumpCharges < ship.maxJumpCharges) {
			ship.money = Shop.PurchaseJumpCharge (ship.money,out successful);
			if (successful) {
				ship.jumpCharges++;
			}
		}
	}
	public void BuyMaxJumpCharges (){
		int maxCharges = ship.maxJumpCharges - ship.jumpCharges;
		for (int i = 0; i < maxCharges; i++) {
			BuyJumpCharge ();
		}
	}
	public void BuyShipStrength (){
		bool successful;
		if (ship.strength < ship.maxStrength) {
			ship.money = Shop.PurchaseShipStrength (ship.money,out successful);
			if (successful) {
				ship.strength++;
			}
		}
	}
	public void BuyMaxShipStrength (){
		int maxStrength = ship.maxStrength - ship.strength;
		for (int i = 0; i < maxStrength; i++) {
			BuyShipStrength ();
		}
	}
	public void BuyClue (){
		bool successful;
		ship.research = ResearchOutpost.BuyClue (ship.research,clueKnown,out successful);
//		if (successful) {
//			for (int i = 0; i < clueKnown.Length; i++) {
//				if (clueKnown [i] == false) {
//					clueKnown [i] = true;
//					return;
//				}
//			}
//		}
		UpdateClueDisplay();
	}
	public void BuyResearch (){
		bool successful;
		ship.research = ResearchOutpost.BuyResearchPoints (ship.money,out successful);
		if (successful) {
			ship.research += 100;
		}

	}
	public Vector3 GetHeading(Vector3 point1,Vector3 point2){

		return  point2 - point1;

	}
	public float GetDegrees(Vector3 heading){

		return  Mathf.Atan2 (heading.x, heading.z) * Mathf.Rad2Deg;
		//return Vector3.Angle (point2, point1);
	}
	public Vector3 GetPosition(float degrees, float dist){
		float a = degrees * Mathf.PI / 180f;
		return new Vector3(Mathf.Sin(a) * dist, 25, Mathf.Cos(a) * dist);
	}
	public void UpdateClue(){

		clue.distance = GetSystemDistance (mainGeneration.currentSystem, mainEventProgress);
		clue.jumpDistance =	mainGeneration.systemPoint [mainEventProgress].numberOfJumpsAway;
	

		clue.distanceMin = clue.distance  - Random.Range (100, 250);
		if (clue.distanceMin < 0) {
			clue.distanceMin = 0;
		}

		clue.distanceMax = clue.distance  + Random.Range (100, 250);

		clue.jumpDistanceMin = clue.jumpDistance - Random.Range (8, 20);
		if (clue.jumpDistanceMin < 0) {
			clue.jumpDistanceMin = 0;
		}

		clue.jumpDistanceMax = clue.jumpDistance + Random.Range (8, 20);

	}
	public void UpdateClue (int[] known){

		//StarSystemGenerator.StarSystemInfo ssi = StarSystemGenerator.GetStarSystemInfo (mainGeneration.starSystem[mainGeneration.currentSystem].seed);

		for (int i = 0; i < known.Length; i++) {
			clueKnown [known [i]] = true;
		}
			
		UpdateClueDisplay();
	

	}
	public string[] RandomiseOrder (string[] _text){

		for (int i = 0; i < _text.Length; i++) {
			int r = Random.Range (0, _text.Length);
			string temp = _text [r];
			_text [r] = _text [i];
			_text [i] = temp;
		}
		return _text;
		
	}
	public void UpdateClueDisplay (){
	
		clueText [5].text = (clueKnown[0])? clue.systemNumberOfPlanets.ToString() : clue.systemNumberOfPlanetsMin.ToString() + "-" + clue.systemNumberOfPlanetsMax.ToString();
		clueText [6].text = (clueKnown[1])? clue.systemHasAsteroidBelt.ToString() : "???";

		string[] starTypeString = new string[] {
			clue.systemStarTypeFake1.ToString (),
			clue.systemStarType.ToString (),
			clue.systemStarTypeFake2.ToString ()
		};
		RandomiseOrder (starTypeString);

		clueText [7].text = (clueKnown[2])? clue.systemStarType.ToString() : starTypeString[0] + "-" + starTypeString[1] + "-" + starTypeString[2];
		clueText [8].text = (clueKnown[3])? ((int)clue.distance).ToString() : ((int)clue.distanceMin).ToString() + "-" + ((int)clue.distanceMax).ToString();
		clueText [9].text = (clueKnown[4])? clue.jumpDistance.ToString() : clue.jumpDistanceMin.ToString() + "-" + clue.jumpDistanceMax.ToString();


		StarSystemGenerator.StarSystemInfo ssi = StarSystemGenerator.GetStarSystemInfo (mainGeneration.starSystem [targetSystem].seed);

		clueText [10].text = ssi.numberOfPlanets.ToString ();
		clueText [11].text = ssi.asteroidBelt.ToString ();
		clueText [12].text = ssi.starType.ToString ();
		clueText [13].text = ((int)GetSystemDistance (mainGeneration.currentSystem, targetSystem)).ToString();
		clueText [14].text = mainGeneration.systemPoint [targetSystem].numberOfJumpsAway.ToString();


		if (clueKnown [0]) {
			clueText [15].text = (clue.systemNumberOfPlanets == ssi.numberOfPlanets ? "=" : "<color=#ff0000ff>X</color>");
		} else {
			if (ssi.numberOfPlanets >= clue.systemNumberOfPlanetsMin && ssi.numberOfPlanets <= clue.systemNumberOfPlanetsMax) {
				clueText [15].text = "~";
			} else {
				clueText [15].text = "<color=#ff0000ff>X</color>";
			}
		}

		if (clueKnown [1]) {
			clueText [16].text = (clue.systemHasAsteroidBelt == ssi.asteroidBelt ? "=" : "<color=#ff0000ff>X</color>");
		} else {
			clueText [16].text = "~";
		}

		if (clueKnown [2]) {
			clueText [17].text = (clue.systemStarType == ssi.starType ? "=" : "<color=#ff0000ff>X</color>");
		} else {
			if (ssi.starType == clue.systemStarType || ssi.starType == clue.systemStarTypeFake1 || ssi.starType == clue.systemStarTypeFake2) {
				clueText [17].text = "~";
			} else {
				clueText [17].text = "<color=#ff0000ff>X</color>";
			}
		}

		if (clueKnown [3]) {
			clueText [18].text = ((int)clue.distance == (int)GetSystemDistance (mainGeneration.currentSystem, targetSystem) ? "=" : "<color=#ff0000ff>X</color>");
		} else {
			if ((int)GetSystemDistance (mainGeneration.currentSystem, targetSystem) >= clue.distanceMin && (int)GetSystemDistance (mainGeneration.currentSystem, targetSystem) <= clue.distanceMax) {
				clueText [18].text = "~";
			} else {
				clueText [18].text = "<color=#ff0000ff>X</color>";
			}
		}

		if (clueKnown [4]) {
			clueText [19].text = (clue.jumpDistance == mainGeneration.systemPoint [targetSystem].numberOfJumpsAway ? "=" : "<color=#ff0000ff>X</color>");
		} else {
			if (mainGeneration.systemPoint [targetSystem].numberOfJumpsAway >= clue.jumpDistanceMin && mainGeneration.systemPoint [targetSystem].numberOfJumpsAway <= clue.jumpDistanceMax ) {
				clueText [19].text = "~";
			} else {
				clueText [19].text = "<color=#ff0000ff>X</color>";
			}
		}





	}
	public void NextClue (int[] clueKnown){
		CalculateNumberOfSystemJumpsBetweenSystems (mainGeneration.currentSystem,mainGeneration.currentSystem, 0);
		clue = CreateClue (mainGeneration.starSystem[mainEventProgress].seed,mainGeneration.systemPoint[mainEventProgress].numberOfJumpsAway,GetSystemDistance(mainGeneration.currentSystem,mainEventProgress));
		UpdateClue (clueKnown);
	}
	public Clue CreateClue (int seed,int numberOfJumps,float distance){

		clueKnown = new bool[5];

		Clue newClue = new Clue ();
		StarSystemGenerator.StarSystemInfo ssi = StarSystemGenerator.GetStarSystemInfo (seed);

		newClue.systemNumberOfPlanets = ssi.numberOfPlanets;
		newClue.systemNumberOfPlanetsMin = ssi.numberOfPlanets - Random.Range (1, 4);
		if (newClue.systemNumberOfPlanetsMin < 0) {
			newClue.systemNumberOfPlanetsMin = 0;
		}
		newClue.systemNumberOfPlanetsMax = ssi.numberOfPlanets + Random.Range (1, 4);

		newClue.systemHasAsteroidBelt = ssi.asteroidBelt;

		newClue.systemStarType = ssi.starType;
		newClue.systemStarTypeFake1 = GetRandomStarType (new StarType[]{ssi.starType});
		newClue.systemStarTypeFake2 = GetRandomStarType (new StarType[]{ssi.starType,newClue.systemStarTypeFake1});

		newClue.distance = distance;
		newClue.distanceMin = distance - Random.Range (50, 250);
		if (newClue.distanceMin < 0) {
			newClue.distanceMin = 0;
		}

		newClue.distanceMax = distance + Random.Range (50, 250);

		newClue.jumpDistance = numberOfJumps;
		newClue.jumpDistanceMin = numberOfJumps - Random.Range (8, 20);
		if (newClue.jumpDistanceMin < 0) {
			newClue.jumpDistanceMin = 0;
		}

		newClue.jumpDistanceMax = numberOfJumps + Random.Range (8, 20);

		return newClue;
	}
	public StarType GetRandomStarType (){
		return (StarType)Random.Range (0, 6);
	}
	public StarType GetRandomStarType (StarType[] _UsedType){

		StarType newType = GetRandomStarType ();

		for (int i = 0; i < _UsedType.Length; i++) {

			if (newType == _UsedType [i]) {
				GetRandomStarType (_UsedType);
			}

		}

		return newType;
	}
	public void ClearNumberOfSystemJumps(){

		for (int i = 0; i < mainGeneration.systemPoint.Length; i++) {
			mainGeneration.systemPoint [i].numberOfJumpsAway = 0;
		}
	}
	public void CalculateNumberOfSystemJumpsBetweenSystems(int originalSystem,int system,int numberOfJumps){

		numberOfJumps++;
		for (int i = 0; i < mainGeneration.systemPoint [system].connectedSystems.Count; i++) {

			if (mainGeneration.systemPoint [mainGeneration.systemPoint [system].connectedSystems [i]].numberOfJumpsAway == 0) {
				if (mainGeneration.systemPoint [system].connectedSystems [i] != originalSystem) {
					mainGeneration.systemPoint [mainGeneration.systemPoint [system].connectedSystems [i]].numberOfJumpsAway = numberOfJumps;
					//SetNumberOfJumps (mainGeneration.systemPoint [system].connectedSystems [i], numberOfJumps);
					CalculateNumberOfSystemJumpsBetweenSystems (originalSystem,mainGeneration.systemPoint [system].connectedSystems [i], numberOfJumps);
				}
			}
			
		}

	}
	public void UpdateNavigationLinePoints (int startingSystem,int endSystem,int length){
		length += 1;
		Vector3[] newPoints = new Vector3[length];
		int nextSystem = startingSystem;
		newPoints[0] = mainGeneration.systemPoint [nextSystem].position;
		for (int i = 1; i < newPoints.Length; i++) {	

			nextSystem = GetNextSystem (nextSystem, endSystem, length);
			newPoints[i] = mainGeneration.systemPoint [nextSystem].position;
			length--;


		}


		UpdateNavigationLine (newPoints);

	}
	public int GetNextSystem (int system,int endSystem,int length){
		for (int s = 0; s < mainGeneration.systemPoint [system].connectedSystems.Count; s++) {
			ClearNumberOfSystemJumps ();
			CalculateNumberOfSystemJumpsBetweenSystems (mainGeneration.systemPoint [system].connectedSystems [s], mainGeneration.systemPoint [system].connectedSystems [s], 0);

			if (mainGeneration.systemPoint [endSystem].numberOfJumpsAway < length) {

				return mainGeneration.systemPoint [system].connectedSystems [s];
			}

		}
		return system;
	}
	public Vector3 GetNavigationLinePoint (int system){
		return mainGeneration.systemPoint [system].position;
	}
	public void UpdateNavigationLine (Vector3[] points){

		int segments = points.Length;

		LineRenderer line = navigationLinePoint.GetComponent<LineRenderer>();
		line.positionCount = segments;
		for (int i = 0; i < segments; i++) {			
			line.SetPosition (i, points[i]);
		}
	}
	public GameObject DrawNavigationLine(Vector3[] points){

		GameObject newLine = new GameObject ("NavigationPath");

		int segments = points.Length;


		LineRenderer line;

		line = newLine.AddComponent<LineRenderer>();

		line.positionCount = segments;
		line.useWorldSpace = false;
		line.loop = false;
		line.material.mainTexture = new Texture ();
		line.material.color = Color.yellow;

		line.material.EnableKeyword ("_EMISSION");
		line.material.SetTexture ("_EmissionMap",new Texture());
		line.material.SetColor ("_EmissionColor",Color.yellow);

		line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		line.receiveShadows = false;
		line.startWidth = 2.0f;
		line.endWidth = 2.0f;

		for (int i = 0; i < segments; i++) {			
			line.SetPosition (i, points[i]);
		}

		return newLine;
	}
//	public void SetNumberOfJumps (int system,int numberOfJumps){
//		mainGeneration.systemPoint [system].numberOfJumpsAway = numberOfJumps;
//	}
	public void OpenCluePanel (){
		clueUI.SetActive (!clueUI.activeSelf);

	}
	public void OpenGameUI (){

		gameCanvas.SetActive (!gameCanvas.activeSelf);
	}
	public void StartJumpDriveProgress (int _nextSystem){
		if (ship.jumpCharges <= 0)
			return;
		if (CheckSystemDistance (mainGeneration.currentSystem, _nextSystem, ship.maxJumpDistance) == false)
			return;

		nextjumpDrive = _nextSystem;
		driveJumping = true;
		jumpDriveProgress = 1;
		jumpDriveSpeed = 1;
		if (mainMenu.switchToSystemOnMove.isOn) {
			SystemViewMode ();
		} else {
			GalaxyViewMode ();
		}
		// sound
		// animation
		audioControl.PlaySoundEffect (6);
	}
	public void UpdateJumpDrive (){

		jumpDriveProgress -= jumpDriveSpeed * Time.deltaTime;
		if (jumpDriveProgress > 0)
			return;


		mainGeneration.ClearSystem ();
		MoveBetweenSystems (nextjumpDrive,mainMenu.switchToSystemOnMove.isOn);
		jumpDriveProgress = 1;
		driveJumping = false;
		ship.jumpCharges--;
		SetupNewTargetSystem (targetSystem);
		if (mainMenu.switchToSystemOnMove.isOn) {
			SystemViewMode ();
		} else {
			GalaxyViewMode ();
		}

	}


	public void StartSystemJumpProgress (int _nextSystem){

		if (shipOnOuterEdge == false)
			return;
		if (CheckSystemConnection (mainGeneration.currentSystem, _nextSystem) == false)
			return;
//		if (audioControl.source [1].isPlaying == false) {
//			audioControl.PlaySoundEffect (5);
//		}
		
		nextSystemJump = _nextSystem;
		systemJumping = true;
		systemJumpProgress = 1.8f;
		systemJumpSpeed = 1;

		if (mainMenu.switchToSystemOnMove.isOn) {
			SystemViewMode ();
		} else {
			GalaxyViewMode ();
		}
		audioControl.PlaySoundEffect (5);
		// animation

	}
	public void UpdateSystemJump (){

		systemJumpProgress -= systemJumpSpeed * Time.deltaTime;
		if (systemJumpProgress > 0)
			return;

		
		mainGeneration.ClearSystem ();
		MoveBetweenSystems (nextSystemJump,mainMenu.switchToSystemOnMove.isOn);
		systemJumpProgress = 1.8f;
		systemJumping = false;
		SetupNewTargetSystem (targetSystem);
		if (mainMenu.switchToSystemOnMove.isOn) {
			SystemViewMode ();
		} else {
			GalaxyViewMode ();
		}
	}
	public void StartAutoPilot (){

		autoPilotToggle.isOn = true;

	}
	public void UpdateAutoPilot (){


		if (systemJumping)
			return;

		if (mainGeneration.currentSystem == targetSystem) {
			autoPilotToggle.isOn = false;
			return;
		}

		StartSystemJumpProgress(GetNextSystem (mainGeneration.currentSystem, targetSystem, mainGeneration.systemPoint[targetSystem].numberOfJumpsAway));




	}
	public static GameObject CreateCurrentSystemRing (float distance)
	{
		GameObject currentSystemRing = new GameObject ("CurrentSystemRing");

		int segments = 50;

		float xradius = distance;

		float yradius = xradius;
		LineRenderer line;

		line = currentSystemRing.AddComponent<LineRenderer>();

		line.SetVertexCount (segments + 1);
		line.useWorldSpace = false;
		line.loop = true;
		line.material.mainTexture = new Texture ();
		line.material.color = Color.green;
		line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		line.receiveShadows = false;

		float x;
		float y;
		float z;

		float angle = 20f;

		for (int i = 0; i < (segments + 1); i++) {
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
			z = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

			line.SetPosition (i, new Vector3 (x, 0, z));

			angle += (360f / segments);
		}

		return currentSystemRing;
	}
	public void UpdateCurrentSystemRing (Vector3 _position){
		currentSystemRing.transform.position = _position;
	}
	public void FinalEvent(){
		if (!finalEventBox.activeSelf) {
			finalEventBox.SetActive (true);
		}
	}
	public void EndGame (){
		ResetGame ();
	}
	public void ContinueGame (){
		finalEventBox.SetActive (false);
	}
	public void ShipDestroyed (){
		shipDestroyedBox.SetActive (true);

	}
}
