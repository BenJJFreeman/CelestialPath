using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyGeneration{

	//public int numberOfSystems;

	public static GameObject CreateGalaxy (out StarSystem[] starSystem,int numberOfSystems){

		GameObject newGalaxy = new GameObject ("Galaxy");

	
		starSystem = new StarSystem[numberOfSystems];


		for (int i = 0; i < starSystem.Length; i++) {

			int[] ae = new int[Random.Range (0, 3)];

			for (int e = 0; e<ae.Length;e++){

				ae [e] = Random.Range (0, 3);
			}

			bool me = false;

			if (i < 10) {
				me = true;
			}

			bool t = false;

			if (Random.Range (0, 100) < 50) {
				t = true;
			}

			bool ro = false;

			if (Random.Range (0, 100) < 50) {
				ro = true;
			}

			starSystem [i] = new StarSystem (Random.Range (0, 10000000),ae,me,t,ro);


			GameObject newSystemPoint = new GameObject ("SystemPoint");
			newSystemPoint.transform.parent = newGalaxy.transform;
			newSystemPoint.transform.position = new Vector3 ((20 * (i)), 0, 0);
			newSystemPoint.transform.localScale = new Vector3 (4, 4, 4);
			newSystemPoint.AddComponent<MeshFilter> ().mesh = SolarBodyGeneration.GetSphereMesh ();
			MeshRenderer mr = newSystemPoint.AddComponent<MeshRenderer> ();
			newSystemPoint.AddComponent <SphereCollider> ().radius = 1;
			newSystemPoint.AddComponent<SystemPoint> ().SetStarSystem(i);
			newSystemPoint.tag = "SystemPoint";


			int pixWidth = 16;
			int pixHeight = 16;
			int scale = 1;
			float xOrg = 0;
			float yOrg = 0;


			StarType st = StarSystemGenerator.GetStarSystemInfo(starSystem[i].seed).starType;


			mr.material.mainTexture = Noise.CalculateSeamlessNoise (pixWidth, pixHeight, scale, xOrg, yOrg, starSystem[i].seed, StarColour.GetStarColour (st));

			mr.material.EnableKeyword ("_EMISSION");
			mr.material.SetTexture ("_EmissionMap", Noise.CalculateSeamlessNoise (pixWidth, pixHeight, scale, xOrg, yOrg, starSystem[i].seed, StarColour.GetStarColour (st)));
			mr.material.SetColor ("_EmissionColor",new Color(0.5f,0.5f,0.5f));

		}


		return newGalaxy;

	}

}
[System.Serializable]
public class StarSystem {

	public int seed;

	public int[] anomaliEvent;
	public bool[] anomaliEventCompleted;

	public bool mainEvent;
	//public bool mainEventCompleted;

	public bool trader;
	public bool researchOutpost;

	public StarSystem (int _seed, int[] _anomaliEvent,bool _mainEvent,bool _trader,bool _researchOutpost){

		seed = _seed;
		anomaliEvent = _anomaliEvent;

		anomaliEventCompleted = new bool[anomaliEvent.Length];

		mainEvent = _mainEvent;
		//mainEventCompleted = false;

		trader = _trader;
		researchOutpost = _researchOutpost;
	}

}