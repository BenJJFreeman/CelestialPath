using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;


public class SaveLoadSystem {

	public static void Save (GameData _gameData) {

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");

		GameData data = new GameData();

		data = _gameData;
		//

		bf.Serialize(file, data);
		file.Close();

	} 

	public static GameData Load () {

		if(File.Exists(Application.persistentDataPath + "/gameInfo.dat")){

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat",FileMode.Open);
			GameData data = (GameData)bf.Deserialize(file);
			file.Close();


			return data;
			//



		}

		return new GameData ();
	}


	public static void SaveOptions (GameOptions _gameOptions) {

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gameOptions.dat");

		GameOptions data = new GameOptions();

		data = _gameOptions;
		//

		bf.Serialize(file, data);
		file.Close();

	} 

	public static GameOptions LoadOptions () {

		if(File.Exists(Application.persistentDataPath + "/gameOptions.dat")){

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gameOptions.dat",FileMode.Open);
			GameOptions data = (GameOptions)bf.Deserialize(file);
			file.Close();


			return data;
			//



		}

		return new GameOptions ();
	}

}

[System.Serializable]
public class GameData {

	public bool savedGame;

	// galaxy
	public int seed;
	public int numberOfSystems;
	public GalaxyType galaxyType;
	public int startingSystem;

	// investigation
	public int mainEventProgress;
	public bool[,] anomaliCompleted;
	public Clue clue;
	public bool[] clueKnown;
	public int targetSystem;

	// ship
	public ShipType shipType;
	public Commander commander;
	public int strength;
	public int money;
	public int jumpCharges;
	public int research;
	public float x;
	public float z;


}
[System.Serializable]
public class GameOptions {


	// options
	public float musicVolume;
	public float effectVolume;

	public int resolution;
	public int quality;
	public bool fullscreen;

	public bool holdToMove;
	public bool switchToSystemOnMove;
	public bool cameraFollowShip;


}