using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour {

	public Main main;
	public SystemGenerationModeMain systemGenerationModeMain;

	public GameObject mainObj;
	public GameObject mainGameSetUp;

	public GameObject optionsObj;

	public Dropdown galaxySize;
	public Dropdown galaxyType;
	public Dropdown commander;
	public Dropdown shipType;

	public GameObject[] optionsTabObj;

	// Graphics
	public int[] width;
	public int[] height;
	public Dropdown resolutionDropdown;
	public Toggle fullscreenToggle;
	public Dropdown qualityDropdown;

	// Audio
	//public AudioMixer musicMixer;
	//public AudioMixer soundMixer;
	public Slider musicSlider;
	public Slider soundSlider;

	// Game
	public Toggle holdToMove;
	public Toggle switchToSystemOnMove;
	public Toggle cameraFollowShip;

	// Generation
	public Text shipSelectionResult;
	public Text commanderSelectionResult;
	public Text galaxySizeSelectionResult;

	void Start () {

		width = new int[] {
			1920,
			1600,
			1366,
			1360,
			1280
		};

		height = new int[] {
			1080,
			900,
			768,
			768,
			720
		};

		LoadOptions (SaveLoadSystem.LoadOptions ());

		systemGenerationModeMain.seedText.text = ((int)System.DateTime.Now.Ticks).ToString();
		systemGenerationModeMain.GenerateSystem ();

	}
	public void Play (){

		if (SaveLoadSystem.Load ().savedGame == true) {
			main.StartMain (SaveLoadSystem.Load ());
			mainObj.SetActive (false);
			systemGenerationModeMain.transform.GetChild (0).gameObject.SetActive (false);
		} else {
			NewGame (true);
		}

	}
	public void NewGame (bool _active){
		mainObj.SetActive (!_active);
		mainGameSetUp.SetActive (_active);
	}
	public void StartNewGame (){
		GameData data = new GameData ();

		data.savedGame = false;
		data.seed = (int)System.DateTime.Now.Ticks;
		//data.seed = 255;
		switch (galaxySize.value) {
		case 0:
			data.numberOfSystems = 50;
			break;
		case 1:
			data.numberOfSystems = 125;
			break;
		case 2:
			data.numberOfSystems = 250;
			break;
		case 3:
			data.numberOfSystems = 500;
			break;
		}

		switch (galaxyType.value) {
		case 0:
			data.galaxyType = (GalaxyType)Random.Range(0,2);
			break;
		case 1:
			data.galaxyType = GalaxyType.ring;
			break;
		case 2:
			data.galaxyType = GalaxyType.Elliptical;
			break;
		case 3:
			data.galaxyType = GalaxyType.spiral;
			break;
		}
		switch (shipType.value) {
		case 0:
			data.shipType = (ShipType)Random.Range(0,3);
			break;
		case 1:
			data.shipType = ShipType.a;
			break;
		case 2:
			data.shipType = ShipType.b;
			break;
		case 3:
			data.shipType = ShipType.c;
			break;
		}
		switch (commander.value) {
		case 0:
			data.commander = (Commander)Random.Range(0,3);
			break;
		case 1:
			data.commander = Commander.a;
			break;
		case 2:
			data.commander = Commander.b;
			break;
		case 3:
			data.commander = Commander.c;
			break;
		}





		data.startingSystem = 0;

		data.anomaliCompleted = new bool[0,0];
		data.clue = new Clue ();
		data.clueKnown = new bool[5];
		data.targetSystem = 0;

		systemGenerationModeMain.transform.GetChild (0).gameObject.SetActive (false);
		mainGameSetUp.SetActive (false);
		main.StartMain (data);
	}
	public void SystemGeneration (){
		systemGenerationModeMain.systemGeneratorObj.SetActive (true);
		mainObj.SetActive (false);
		main.systemGenerator = true;
		systemGenerationModeMain.GenerateSystem ();
		systemGenerationModeMain.UpdateResultText ();
	}
	public void Options (bool _active){
		optionsObj.SetActive (_active);
	}
	public void ApplyOptions (){

		

		Screen.SetResolution (width[resolutionDropdown.value], height[resolutionDropdown.value], fullscreenToggle.isOn);
		QualitySettings.SetQualityLevel (qualityDropdown.value);

		main.audioControl.mixer[0].SetFloat ("Volume", musicSlider.value);
		main.audioControl.mixer[1].SetFloat ("Volume", soundSlider.value);

		SaveOptions ();
	}
	public void QuitGame (){
		Application.Quit ();
	}
	public void OpenLinkToItchIO(){

		Application.OpenURL ("https://benfreeman.itch.io/");

	}
	public void SaveOptions (){

		GameOptions newGameOptions = new GameOptions ();

		//float musicVolume;
		//main.audioControl.mixer [0].GetFloat ("Volume",out musicVolume);
		newGameOptions.musicVolume = musicSlider.value;

		//float effectVolume;
		//main.audioControl.mixer [1].GetFloat ("Volume",out effectVolume);
		newGameOptions.effectVolume = soundSlider.value;

		newGameOptions.resolution = resolutionDropdown.value;
		newGameOptions.quality = qualityDropdown.value;
		newGameOptions.fullscreen = fullscreenToggle.isOn;

		newGameOptions.holdToMove = holdToMove.isOn;
		newGameOptions.switchToSystemOnMove = switchToSystemOnMove.isOn;
		newGameOptions.cameraFollowShip = cameraFollowShip.isOn;

		SaveLoadSystem.SaveOptions (newGameOptions);
	}
	public void LoadOptions (GameOptions _gameOptions){

		//main.audioControl.mixer [0].SetFloat ("Volume",_gameOptions.musicVolume);
		//main.audioControl.mixer [1].SetFloat ("Volume",_gameOptions.effectVolume);

		musicSlider.value = _gameOptions.musicVolume;
		soundSlider.value = _gameOptions.effectVolume;


		resolutionDropdown.value = _gameOptions.resolution;
		qualityDropdown.value = _gameOptions.quality;
		fullscreenToggle.isOn = _gameOptions.fullscreen;
		holdToMove.isOn = _gameOptions.holdToMove;
		switchToSystemOnMove.isOn = _gameOptions.switchToSystemOnMove;
		cameraFollowShip.isOn = _gameOptions.cameraFollowShip;



		ApplyOptions ();
	}
	public void SwitchOptionsTab (int _tab){

		for (int i = 0; i < optionsTabObj.Length; i++) {

			optionsTabObj [i].SetActive (false);

		}

		optionsTabObj [_tab].SetActive (true);

	}
	public void UpdateShipSelectionResult (){

		if (shipType.value == 0) {
			shipSelectionResult.text = "Random";
		} else {
			switch (shipType.value) {
			case 1:
				shipSelectionResult.text = "Strength = 25\nJump Charges = 2\nJump Distance = 125";
				break;
			case 2:
				shipSelectionResult.text = "Strength = 15\nJump Charges = 4\nJump Distance = 200";
				break;
			case 3:
				shipSelectionResult.text = "Strength = 20\nJump Charges = 3\nJump Distance = 150";
				break;
			}

		}


	}
	public void UpdateCommanderSelectionResult (){
		if (commander.value == 0) {
			commanderSelectionResult.text = "Random";
		} else {
			switch (commander.value) {
			case 1:
				commanderSelectionResult.text = "Units = 800\nResearch = 200\nStrength + 3\nJump Charges +1";
				break;
			case 2:
				commanderSelectionResult.text = "Units = 500\nResearch = 500\nStrength + 2\nJump Charges +2";
				break;
			case 3:
				commanderSelectionResult.text =  "Units = 200\nResearch = 800\nStrength + 1\nJump Charges +3";
				break;
			}

		}
	}
	public void UpdateGalaxySizeSelectionResult (){

		switch (galaxySize.value) {
		case 0:
			galaxySizeSelectionResult.text = "Number of Systems = 50";
			break;
		case 1:
			galaxySizeSelectionResult.text = "Number of Systems = 125";
			break;
		case 2:
			galaxySizeSelectionResult.text = "Number of Systems = 250";
			break;
		case 3:
			galaxySizeSelectionResult.text = "Number of Systems = 500";
			break;
		}

	}

}
