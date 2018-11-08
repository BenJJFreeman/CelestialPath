using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemGenerator {

	public struct StarSystemInfo {


		public int numberOfPlanets;
		public bool asteroidBelt;
		public StarType starType;

		public float systemSize;

		public StarSystemInfo (int _numberOfPlanets,bool _asteroidBelt,StarType _starType,float _systemSize){
			numberOfPlanets =_numberOfPlanets;
			asteroidBelt = _asteroidBelt;
			starType = _starType;
			systemSize = _systemSize;
		}

	}
	public static StarSystemInfo GetStarSystemInfo (int seed){
		Random.InitState (seed);

		int numberOfPlanets = Random.Range (2,10);
		bool asteroidBelt = false;
		//bool asteroidBeltCreated = false;

		if (Random.Range (0, 100) > 70) {
			asteroidBelt = true;
		}

		StarType st = (StarType)Random.Range (0, 6);




		float ss = GetStarSystemEdgeDistance (asteroidBelt, numberOfPlanets);

		StarSystemInfo ssi = new StarSystemInfo (numberOfPlanets,asteroidBelt,st,ss);

		return ssi;

	}
	public static GameObject CreateSystem (int seed,int numberOfPlanets,bool hasAsteroidBelt,StarType starType) {


		GameObject newSystem = new GameObject ("StarSystem");

		Random.InitState (seed);

		StarSystemInfo ssi = new StarSystemInfo();

		ssi.numberOfPlanets = numberOfPlanets;
		ssi.asteroidBelt = hasAsteroidBelt;
		ssi.starType = starType;

		int numberOfBodies = ssi.numberOfPlanets;
		if ( ssi.asteroidBelt) {
			numberOfBodies++;
		}


		GameObject newStar = CelestialBody.CreateStar (Vector3.zero, Random.Range (0, 99999),  ssi.starType);
		newStar.transform.parent = newSystem.transform;



		for (int i = 0; i < numberOfBodies; i++) {
	


			if (ssi.asteroidBelt) {
				if (Random.Range (0, 100) > 50) {
					AddAsteroidBelt (i).transform.parent = newSystem.transform;
					ssi.asteroidBelt = false;

				} else if(i == (numberOfBodies -1)) {
					AddAsteroidBelt (i).transform.parent = newSystem.transform;
					ssi.asteroidBelt = false;
				}else {
					AddPlanet (i).transform.parent = newSystem.transform;
				}
			} else {

				AddPlanet (i).transform.parent = newSystem.transform;

			}


		}
			

		return newSystem;

	}
	public static GameObject CreateSystem (int seed) {
		
		GameObject newSystem = new GameObject ("StarSystem");

		Random.InitState (seed);

		StarSystemInfo ssi = GetStarSystemInfo (seed);



		int numberOfBodies = ssi.numberOfPlanets;
		if ( ssi.asteroidBelt) {
			numberOfBodies++;
		}




		GameObject newStar = CelestialBody.CreateStar (Vector3.zero, Random.Range (0, 99999),  ssi.starType);
		newStar.transform.parent = newSystem.transform;







		for (int i = 0; i < numberOfBodies; i++) {


			if (ssi.asteroidBelt) {
				if (Random.Range (0, 100) > 50) {
					AddAsteroidBelt (i).transform.parent = newSystem.transform;
					ssi.asteroidBelt = false;

				} else if(i == (numberOfBodies -1)) {
					AddAsteroidBelt (i).transform.parent = newSystem.transform;
					ssi.asteroidBelt = false;
				}else {
					AddPlanet (i).transform.parent = newSystem.transform;
				}
			} else {

				AddPlanet (i).transform.parent = newSystem.transform;

			}


		}

	



		CreateSystemEdgeRing (ssi.systemSize).transform.parent = newSystem.transform;

			
		return newSystem;

	}
	public static GameObject AddPlanet (int i){

		GameObject newPlanet = CelestialBody.CreatePlanet (PlanetPoint(i), Random.Range (0, 99999), (PlanetType)Random.Range (0, 15));
		//orbitSpeed [i] = Random.Range (1.0f, 10.0f);
		//newPlanet.transform.parent = transform;
		//holeRadius = holeRadius  + 20;
		//diskRadius = diskRadius + 20;
		if (Random.Range (0, 100) > 50) {
			GameObject newMoon = CelestialBody.CreateMoon ();
			newMoon.transform.parent = newPlanet.transform;
			newMoon.transform.localPosition = Vector3.zero;
			newMoon.transform.localPosition += CelestialBody.MoonPoint(0);
		}
		if (Random.Range (0, 100) > 30) {
			GameObject newCloud = CelestialBody.CreateCloud ();
			newCloud.transform.parent = newPlanet.transform;
			newCloud.transform.localPosition = Vector3.zero;
			newCloud.transform.localScale = new Vector3(1.01f,1.0f,1.0f);
		}

		if (Random.Range (0, 100) > 80) {
			GameObject newAsteroidField = CreateAsteroidFieldPoints ();
			newAsteroidField.transform.parent = newPlanet.transform;
			newAsteroidField.transform.localPosition = Vector3.zero;
			newAsteroidField.transform.localScale = new Vector3(0.20f,0.20f,0.20f);
		}

		return newPlanet;

	}
	public static GameObject AddAsteroidBelt (int i){
		int numberOfAsteroids = Random.Range (50,100);
		GameObject newAsteroidBelt = new GameObject ("Asteroid Belt");
		//newAsteroidBelt.transform.parent = transform;

		float holeRadius = 45f; 
		holeRadius = holeRadius  + (20 * (i +2));
		float diskRadius = 55f;
		diskRadius = diskRadius + (20* (i +2));

		for (int a = 0; a < numberOfAsteroids; a++) {

			Vector3 newPos = FindPos (Vector3.zero,diskRadius,holeRadius);
			Vector3 newPos3 = new Vector3 (newPos.x, Random.value, newPos.y);

			GameObject newAsteroid = CelestialBody.CreateAsteroid (newPos3);
			newAsteroid.transform.localScale = new Vector3 ((newAsteroid.transform.localScale.x / 100) * Random.Range (50, 80), (newAsteroid.transform.localScale.y / 100) * Random.Range (40, 60), (newAsteroid.transform.localScale.z / 100) * Random.Range (50, 80));
			newAsteroid.transform.localScale /= 2;

			newAsteroid.transform.parent = newAsteroidBelt.transform;					
			newAsteroid.transform.rotation = Random.rotation;

		}

		return newAsteroidBelt;

	}
	static Vector3 FindPos(Vector3 holeLocalPos,float diskRadius,float holeRadius) {
		Vector3 pos = Vector3.zero;
		for (var i = 0; i < 1000; i++) {
			pos = Random.insideUnitCircle * diskRadius;
			if (Vector3.Distance(pos, holeLocalPos) > holeRadius) {
				return pos;
			}

		}
		pos = Vector3.zero;
		return pos;
	}

	public static Vector3 PlanetPoint (int i){

		Vector3 direction = Random.insideUnitCircle.normalized;
		direction.z = direction.y;
		direction.y = 0;

		//Vector3 distance = new Vector3 (10 * (i+1),Random.value,10 * (i+1));

		Vector3 point = direction * (20 * (i+2));
		return point;
	}

	public static GameObject CreateAsteroidFieldPoints ()
	{
		GameObject asteroidField = new GameObject ("AsteroidField");

		int segments = 50;

		float xradius = 5;

		float yradius = 5;
		LineRenderer line;

		line = asteroidField.AddComponent<LineRenderer>();

		line.SetVertexCount (segments + 1);
		line.useWorldSpace = false;
		line.loop = true;
		line.material.mainTexture = new Texture ();
		line.material.color = Color.gray;
		line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;

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

		return asteroidField;
	}
	public static Vector3 GetRandomPoint (float _distance){
		Vector3 point = Random.insideUnitSphere * _distance;
		point.y = 0;

		return point;

	}
	public static GameObject CreateAnomaliPoint (){

		GameObject newPoint = new GameObject ("AnomaliPoint");

		newPoint.AddComponent<MeshFilter> ().mesh = SolarBodyGeneration.GetConeMesh ();

		newPoint.AddComponent<MeshRenderer> ();



		return newPoint;

	}
	public static float GetStarSystemEdgeDistance (int _seed){

		StarSystemInfo ssi = GetStarSystemInfo (_seed);

		int i = 0;
		if (ssi.asteroidBelt) {
			i++;
		}
		i += ssi.numberOfPlanets;

		float distance = 20 * (i + 3);


		return distance;
	}
	public static float GetStarSystemEdgeDistance (bool _asteroidBelt,int _numberOfPlanets){

		int i = 0;
		if (_asteroidBelt) {
			i++;
		}
		i += _numberOfPlanets;

		float distance = 20 * (i + 3);


		return distance;
	}
	public static GameObject CreateSystemEdgeRing (float distance)
	{
		GameObject systemEdgeRing = new GameObject ("SystemEdgeRing");

		int segments = 50;

		float xradius = distance;

		float yradius = xradius;
		LineRenderer line;

		line = systemEdgeRing.AddComponent<LineRenderer>();

		line.SetVertexCount (segments + 1);
		line.useWorldSpace = false;
		line.loop = true;
		line.material.mainTexture = new Texture ();
		line.material.color = Color.gray;
		line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		line.receiveShadows = false;

		float x;
		float y;
		float z;

		float angle = 20f;

		for (int i = 0; i < (segments + 1); i++) {
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
			z = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

			line.SetPosition (i, new Vector3 (x, 25, z));

			angle += (360f / segments);
		}

		return systemEdgeRing;
	}
}
