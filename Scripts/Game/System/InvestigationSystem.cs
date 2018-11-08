using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationSystem{


	public static Clue CreateClue (StarSystemGenerator.StarSystemInfo ssi,int numberOfJumps,float distance){
		Clue newClue = new Clue ();

//		switch (value) {
//		case 0:
//			newClue.systemNumberOfPlanets = ssi.numberOfPlanets;
//			break;
//		case 1:
//			newClue.systemHasAsteroidBelt = ssi.asteroidBelt;
//			break;
//		case 2:
//			newClue.systemStarType = ssi.starType;
//			break;
//		case 3:
//			newClue.systemNumberOfPlanets = ssi.numberOfPlanets;
//			newClue.systemHasAsteroidBelt = ssi.asteroidBelt;
//			break;
//		case 4:
//			newClue.systemNumberOfPlanets = ssi.numberOfPlanets;
//			newClue.systemStarType = ssi.starType;
//			break;
//		case 5:
//			newClue.systemHasAsteroidBelt = ssi.asteroidBelt;
//			newClue.systemStarType = ssi.starType;
//			break;
//		case 6:
//			newClue.systemNumberOfPlanets = ssi.numberOfPlanets;
//			newClue.systemHasAsteroidBelt = ssi.asteroidBelt;
//			newClue.systemStarType = ssi.starType;
//			break;
//		default:
//
//			break;
//
//		}


		newClue.systemNumberOfPlanets = ssi.numberOfPlanets;
		newClue.systemHasAsteroidBelt = ssi.asteroidBelt;
		newClue.systemStarType = ssi.starType;
		newClue.distance = distance;
		newClue.jumpDistance = numberOfJumps;

		return newClue;
	}
//	public static string CreateCluePassage (Clue _clue){
//		string passage;
//
//		passage = "Look towards the system of a blue star ";
//
//
//
//		return "";
//	}
}
//public enum Investigation {destroyedPlanet,deadStation,missingFleet,disruptedSystems}
[System.Serializable]
public struct Clue {	

	public int system;

	public int systemNumberOfPlanetsMin;
	public int systemNumberOfPlanetsMax;
	public int systemNumberOfPlanets;

	public bool systemHasAsteroidBelt;

	public StarType systemStarType;
	public StarType systemStarTypeFake1;
	public StarType systemStarTypeFake2;

	public int jumpDistanceMin;
	public int jumpDistanceMax;
	public int jumpDistance;

	public float distanceMin;
	public float distanceMax;
	public float distance;

}