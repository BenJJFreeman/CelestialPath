using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGeneration : MonoBehaviour {


	//public int[] systemSeed;
	public StarSystem[] starSystem;
	public SystemPointNode[] systemPoint;

	public int currentSystem;
	public int currentHoverSystem;


	//public GameObject currentSystemObject;

	public ParticleSystem[] ps;
	public GalaxyType galaxyType;
	public int seed;

	public GameObject[] traderGameObject;

	public GameObject[] researchGameObject;

	public GameObject[] shipGameObject;

	public void GenerateGalaxy (GameData gameData){

		//string _seedS = .ToString ();
		//inputField.text = _seedS;
		//	ssg.GenereateSystemFromStringSeed (_seedS);

		//Random.InitState (Random.Range (0, 1000));
		seed = gameData.seed;
		galaxyType = gameData.galaxyType;

		Random.InitState (seed);
		//Random.InitState (17);
		//Random.InitState (677);

		//int numberOfSystems = 250;

		//starSystem = new StarSystem[numberOfSystems];
		//systemSeed = new int[numberOfSystems];
		systemPoint = new SystemPointNode[gameData.numberOfSystems];

		for (int i = 0; i < gameData.numberOfSystems; i++) {
			//starSystem [i] = new StarSystem ( Random.Range (0, 99999));
			//starSystem [i].seed = Random.Range (0, 99999);
			systemPoint[i] = new SystemPointNode();
			systemPoint [i].numberOfJumpsAway = 0;
		}
			
		GalaxyGeneration.CreateGalaxy (out starSystem,gameData.numberOfSystems).transform.parent = transform;

		if (gameData.savedGame) {
			for (int i = 0; i < gameData.numberOfSystems; i++) {
				for (int j = 0; j < starSystem [i].anomaliEventCompleted.Length; j++) {
					starSystem [i].anomaliEventCompleted [j] = gameData.anomaliCompleted [i, j];

				}
			}
		}

		float diskRadius = 250;
		float holeRadius = 150;

		if (gameData.numberOfSystems == 50) {
			diskRadius = 100;
			holeRadius = 25;
		} 
		else if (gameData.numberOfSystems == 125) {
			diskRadius = 200;
			holeRadius = 75;
		} 
		else if (gameData.numberOfSystems == 250) {
			diskRadius = 250;
			holeRadius = 150;
		} 
		else if (gameData.numberOfSystems == 500) {
			diskRadius = 400;
			holeRadius = 200;
		}



		foreach (Transform child in transform.GetChild (0).transform) {
			
			switch (galaxyType){

			case GalaxyType.Elliptical:
				child.position = SetSystemPointPosition (diskRadius);
				break;
			case GalaxyType.ring:
				child.position = SetDonutSystemPointPosition (Vector3.zero, diskRadius, holeRadius);
				break;
			case GalaxyType.spiral:

				break;
//
//			default:
//				child.position = SetDonutSystemPointPosition (Vector3.zero, 250, 150);
//				break;
			}
		}



		for (int s = 0; s < systemPoint.Length; s++) {
			systemPoint [s].SetPosition(transform.GetChild (0).GetChild (s).position);
		}

		systemPoint [0].SetConectedToZero(true);

		//		for (int s = 0; s < systemPoint.Length; s++) {
		//
		//			FindConnections (s);
		//
		//		}
		for (int s = 0; s < systemPoint.Length; s++) {
			//if (systemPoint [s].conectedToZero) {
			FindSecondaryConnections ();
			//}
		}

		//		for (int s = 0; s < systemPoint.Length; s++) {
		//			//if (systemPoint [s].conectedToZero) {
		//			FindThirdConnections (s);
		//			//}
		//		}
		for (int s = 0; s < systemPoint.Length; s++) {

			ConnectSystems (s).transform.parent = transform.GetChild (0).GetChild (s);

		}



		GenerateSystem(starSystem [gameData.startingSystem],true);
		currentSystem = gameData.startingSystem;

		transform.GetChild (0).gameObject.SetActive (false); 
		//currentSystemObject.transform.parent = transform.GetChild (0);
		//currentSystemObject.transform.position = systemPoint [gameData.startingSystem].position;
	}
	public StarSystemGenerator.StarSystemInfo GenerateSystem (StarSystem _starSystem,bool _active){

		GameObject newSystem = StarSystemGenerator.CreateSystem (_starSystem.seed);
		newSystem.transform.parent = transform;
		//StarSystemGenerator.CreateSystemEdgeRing (seed).transform.parent = transform.GetChild(1);

		for (int i = 0; i < _starSystem.anomaliEvent.Length; i++) {

			GameObject newPoint = new GameObject("Anomali Event");
			newPoint.transform.parent = newSystem.transform;
			newPoint.transform.localScale = new Vector3(2,2,2);

			newPoint.AddComponent<MeshFilter> ().mesh = SolarBodyGeneration.GetTorusMesh ();

			newPoint.AddComponent<MeshRenderer> ().material.mainTexture = new Texture();
			newPoint.GetComponent<MeshRenderer> ().material.color = Color.white;

			GameObject newPoint2 = new GameObject();
			newPoint2.transform.parent = newPoint.transform;
			newPoint2.transform.rotation = new Quaternion (-180,0,0,0);
			newPoint2.transform.position = new Vector3(0,6,0);
			newPoint2.transform.localScale = new Vector3(2,2,2);


			newPoint2.AddComponent<MeshFilter> ().mesh = SolarBodyGeneration.GetConeMesh ();

			newPoint2.AddComponent<MeshRenderer> ().material.mainTexture = new Texture();
			newPoint2.GetComponent<MeshRenderer> ().material.color = Color.white;
			//newPoint.transform.position = StarSystemGenerator.GetRandomPoint (StarSystemGenerator.GetStarSystemEdgeDistance(_starSystem.seed));
			//Vector3 newPos = Random.insideUnitSphere * StarSystemGenerator.GetStarSystemEdgeDistance(_starSystem.seed);
			//newPos.y = 25;
			newPoint.transform.position = GetRandomNonCollidingPoint(StarSystemGenerator.GetStarSystemEdgeDistance(_starSystem.seed));

			newPoint.tag = "EventPoint";
			newPoint.AddComponent<SphereCollider> ();
			newPoint.AddComponent<EventPoint> ().eventNumber = i;
			newPoint.GetComponent<EventPoint> ().eventListNumber = Random.Range (0, 23);
		}

		if(_starSystem.mainEvent){

			GameObject newPoint = new GameObject("Main Event");
			newPoint.transform.parent = newSystem.transform;
			newPoint.transform.localScale = new Vector3(2,2,2);

			newPoint.AddComponent<MeshFilter> ().mesh = SolarBodyGeneration.GetTorusMesh ();

			newPoint.AddComponent<MeshRenderer> ().material.mainTexture = new Texture();
			newPoint.GetComponent<MeshRenderer> ().material.color = Color.yellow;

			GameObject newPoint2 = new GameObject();
			newPoint2.transform.parent = newPoint.transform;
			newPoint2.transform.rotation = new Quaternion (-180,0,0,0);
			newPoint2.transform.position = new Vector3(0,6,0);
			newPoint2.transform.localScale = new Vector3(2,2,2);


			newPoint2.AddComponent<MeshFilter> ().mesh = SolarBodyGeneration.GetConeMesh ();

			newPoint2.AddComponent<MeshRenderer> ().material.mainTexture = new Texture();
			newPoint2.GetComponent<MeshRenderer> ().material.color = Color.yellow;

			//newPoint.transform.position = StarSystemGenerator.GetRandomPoint (StarSystemGenerator.GetStarSystemEdgeDistance(_starSystem.seed));
			//Vector3 newPos = Random.insideUnitSphere * StarSystemGenerator.GetStarSystemEdgeDistance(_starSystem.seed);
			//newPos.y = 25;
			//newPoint.transform.position = newPos;

			newPoint.transform.position = GetRandomNonCollidingPoint(StarSystemGenerator.GetStarSystemEdgeDistance(_starSystem.seed));


			newPoint.tag = "MainEventPoint";
			newPoint.AddComponent<SphereCollider> ();
		}
		if(_starSystem.trader){

			GameObject newPoint = new GameObject("Trader");
			newPoint.transform.parent = newSystem.transform;


			GameObject ob = Instantiate(traderGameObject [Random.Range (0, traderGameObject.Length)],new Vector3(0,-12,0),traderGameObject [Random.Range (0, traderGameObject.Length)].transform.rotation,newPoint.transform);

			newPoint.transform.position = GetRandomNonCollidingPoint(StarSystemGenerator.GetStarSystemEdgeDistance(_starSystem.seed));


			newPoint.tag = "Trader";
			newPoint.AddComponent<SphereCollider> ();		

		}
		if(_starSystem.researchOutpost){

			GameObject newPoint = new GameObject("Research Outpost");
			newPoint.transform.parent = newSystem.transform;

			GameObject ob = Instantiate(researchGameObject [Random.Range (0, researchGameObject.Length)],new Vector3(0,-12,0),researchGameObject [Random.Range (0, researchGameObject.Length)].transform.rotation,newPoint.transform);

			newPoint.transform.position = GetRandomNonCollidingPoint(StarSystemGenerator.GetStarSystemEdgeDistance(_starSystem.seed));


			newPoint.tag = "ResearchOutpost";
			newPoint.AddComponent<SphereCollider> ();
		}

		Vector3 particlePosition = Random.insideUnitSphere * 200;
		particlePosition.y = Random.Range(-350,-500);
		ps [1].transform.position = particlePosition;

		ParticleSystem.MainModule mainP = ps [1].main;
		mainP.startColor = new ParticleSystem.MinMaxGradient (NebulaColour.GetColour (Random.Range (0, 4)));

		for (int i = 0; i < ps.Length; i++) {
			ps[i].Stop ();
			ps[i].Clear ();
			ps[i].randomSeed = (uint)Random.Range (0, 99999);
			ps[i].Play ();
		}

		newSystem.SetActive (_active);

		return StarSystemGenerator.GetStarSystemInfo (_starSystem.seed);

	}
	public Vector3 GetRandomNonCollidingPoint (float _distance){
		Vector3 newPos = new Vector3 ();
		for (int n = 0; n < 1000; n++) {
			newPos = Random.insideUnitCircle * _distance;
			newPos.z = newPos.y;
			newPos.y = 25;
			Collider[] overLaps = Physics.OverlapSphere (newPos, 10);
			//			for (int i = 0; i < overLaps.Length; i++) {
			//				if (overLaps [i].tag == "SystemPoint") {
			//					//SetSystemPointPosition ();
			//				}
			//			}
			if (overLaps.Length == 0) {
				return newPos;
			}
		}
		return newPos;
	}
	public void ClearSystem (){
		for (int i = 0; i < ps.Length; i++) {
			ps[i].Stop ();
			ps[i].Clear ();
		}
		if (transform.childCount > 1 ) {
			Destroy (transform.GetChild (1).gameObject);

		}
	}
	public Vector3 SetSystemPointPosition (float radius){
		Vector3 newPos = new Vector3 ();
		for (int n = 0; n < 1000; n++) {
			newPos = Random.insideUnitCircle * radius;
			newPos.z = newPos.y;
			newPos.y = Random.value * 10;
			Collider[] overLaps = Physics.OverlapSphere (newPos, 10);
			//			for (int i = 0; i < overLaps.Length; i++) {
			//				if (overLaps [i].tag == "SystemPoint") {
			//					//SetSystemPointPosition ();
			//				}
			//			}
			if (overLaps.Length == 0) {
				return newPos;
			}
		}
		return newPos;

	}
	public Vector3 SetDonutSystemPointPosition (Vector3 holeLocalPos,float diskRadius,float holeRadius){
		Vector3 newPos = new Vector3 ();
		for (int n = 0; n < 1000; n++) {
			newPos = Random.insideUnitCircle * diskRadius;
			newPos.z = newPos.y;
			newPos.y = Random.value * 10;

			if (Vector3.Distance (newPos, holeLocalPos) > holeRadius) {	

				Collider[] overLaps = Physics.OverlapSphere (newPos, 10);

				if (overLaps.Length == 0) {
					return newPos;
				}
			}
		}
		return newPos;

	}
	public void FindConnections (int system){

		if (systemPoint [system].conectedToZero == true) {

			for (int s = 0; s < systemPoint.Length; s++) {
				if (systemPoint [s].conectedToZero == false) {
					if (Vector3.Distance (systemPoint [system].position, systemPoint [s].position) < 50) {

						systemPoint [s].conectedToZero = true;
						systemPoint [system].connectedSystems.Add (s);
						systemPoint [s].connectedSystems.Add (system);
						FindConnections (s);

					}
				}

			}
			//			for (int s = 0; s < systemPoint.Length; s++) {
			//				if (systemPoint [s].conectedToZero == true) {
			//					if (Vector3.Distance (systemPoint [system].position, systemPoint [s].position) < 20) {
			//
			//						systemPoint [s].conectedToZero = true;
			//						systemPoint [system].connectedSystems.Add (s);
			//						systemPoint [s].connectedSystems.Add (system);
			//
			//
			//					}
			//				}
			//
			//			}
		} 
		//		else {
		//			float newSmallestDistance = 0;
		//			float smallestDistance = 999999;
		//			int potentialSystemConnection = 0;
		//			for (int s = 0; s < systemPoint.Length; s++) {
		//				if (systemPoint [s].conectedToZero) {
		//					newSmallestDistance = Vector3.Distance (systemPoint [system].position, systemPoint [s].position);
		//
		//					if (newSmallestDistance < smallestDistance) {
		//						smallestDistance = newSmallestDistance;
		//						potentialSystemConnection = s;
		//					}
		//				}
		//				
		//
		//			}
		//				
		//			systemPoint [system].conectedToZero = true;
		//			systemPoint [system].connectedSystems.Add (potentialSystemConnection);
		//			systemPoint [potentialSystemConnection].connectedSystems.Add (system);
		//		}




	}
	public void FindSecondaryConnections (){
		int system = 0;
		float newSmallestDistance = 0;
		float smallestDistance = 999999;
		int potentialSystemConnection = 0;
		for (int x = 0; x < systemPoint.Length; x++) {
			if (systemPoint [x].conectedToZero == true) {
				for (int s = 0; s < systemPoint.Length; s++) {
					if (systemPoint [s].conectedToZero == false) {
						newSmallestDistance = Vector3.Distance (systemPoint [x].position, systemPoint [s].position);

						if (newSmallestDistance < smallestDistance) {
							smallestDistance = newSmallestDistance;
							system = x;
							potentialSystemConnection = s;
						}
					}


				}
			}
		}

		if (system != potentialSystemConnection) {
			//systemPoint [system].conectedToZero = true;
			systemPoint [system].connectedSystems.Add (potentialSystemConnection);
			systemPoint [potentialSystemConnection].connectedSystems.Add (system);
			systemPoint [potentialSystemConnection].conectedToZero = true;

		}
		//FindConnections (potentialSystemConnection);

	}
	public void FindThirdConnections (int system){

		float newSmallestDistance = 0;
		float smallestDistance = 100;
		int potentialSystemConnection = system;
		if (systemPoint [system].connectedSystems.Count == 1) {
			for (int s = 0; s < systemPoint.Length; s++) {
				if (systemPoint [s].connectedSystems.Count == 1) {
					newSmallestDistance = Vector3.Distance (systemPoint [system].position, systemPoint [s].position);
					if (newSmallestDistance < smallestDistance) {
						smallestDistance = newSmallestDistance;
						potentialSystemConnection = s;
					}


					//					if (Vector3.Distance (systemPoint [system].position, systemPoint [s].position) < 100) {
					//						//systemPoint [s].conectedToZero = true;
					//						systemPoint [system].connectedSystems.Add (s);
					//						systemPoint [s].connectedSystems.Add (system);
					//					}					
				}	

			}
		}

		if (system != potentialSystemConnection) {
			//systemPoint [system].conectedToZero = true;
			systemPoint [system].connectedSystems.Add (potentialSystemConnection);
			systemPoint [potentialSystemConnection].connectedSystems.Add (system);
			//systemPoint [potentialSystemConnection].conectedToZero = true;

		}


	}
	public void ConnectSystem (int system){

		//systemPoint [system].conectedToZero = true;


	}
	public GameObject ConnectSystems (int system){


		GameObject newLine = new GameObject ("SystemPath");

		int segments = 0;


		LineRenderer line;

		line = newLine.AddComponent<LineRenderer>();

		line.SetVertexCount (segments);
		line.useWorldSpace = false;
		line.loop = false;
		line.material.mainTexture = new Texture ();
		line.material.color = Color.white;
		line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		line.receiveShadows = false;
		line.startWidth = 0.5f;
		line.endWidth = 0.5f;

		for (int i = 0; i < systemPoint[system].connectedSystems.Count; i++) {


			segments++;
			line.SetVertexCount (segments);
			line.SetPosition (segments-1, systemPoint[system].position);

			segments++;
			line.SetVertexCount (segments);
			line.SetPosition (segments-1, systemPoint[systemPoint[system].connectedSystems[i]].position);


		}


		return newLine;



	}
	


}
[System.Serializable]
public class SystemPointNode {

	public bool conectedToZero = new bool();
	public Vector3 position = new Vector3();
	public List<int> connectedSystems = new List<int>();

	public int numberOfJumpsAway;

	public void SetPosition (Vector3 _pos){
		position = _pos;
	}
	public void SetConectedToZero (bool _connected){
		conectedToZero = _connected;
	}
	public void SetConnectedSystems (int _system){
		connectedSystems.Add(_system);
	}
	//	public SystemPointNode (bool zeroC,Vector3 pos){
	//
	//		conectedToZero = zeroC;
	//		position = pos;
	//
	//	}


}
public enum GalaxyType {Elliptical,ring,spiral}
