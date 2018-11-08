using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody {

	//public PlanetType planetType;
	////public Planet planet;
	//public Clouds[] cloud;
	//public Atmosphere atmosphere;

	//int seed;
	//int numberOfCloudLayers;
	//bool hasAtmosphere;

	//GameObject planetObject;
	//GameObject cloudObject;
	void Awake () {

		
	}
	public void SetUpStarBody (){
		//GenerateRandomStar ();
	//	SetUpStar ();

	}
	public void SetUpBody (bool _new){

		//GenerateRandomPlanet ();
		//SetUpPlanet (_new);
	}
	void GenerateRandomPlanet (){
		//seed = Random.Range (0, 99999);
		//numberOfCloudLayers = Random.Range (0, 3);
		//hasAtmosphere = Random.Range (0, 2) == 0;
		//planetType = (PlanetType)Random.Range (0, 9);
	}
	public static GameObject CreatePlanet (Vector3 _position,int _seed,PlanetType _planetType){

		GameObject planetObject = new GameObject ("Planet");
		planetObject.transform.position = _position;
		//planetObject.transform.parent = transform;
		//newPlanet.AddComponent<Planet> ();

		int size = Random.Range (3, 10);

		planetObject.transform.localScale = new Vector3 (size, size, size);

		///	planet = newPlanet.GetComponent<Planet> ();
		int pixWidth = 256;
		int pixHeight = 256;
		int scale = 6;
		float xOrg = 0;
		float yOrg = 0;

		//planetMesh = SolarBodyGeneration.GetSphereMesh ();
		planetObject.AddComponent<MeshFilter> ().mesh = SolarBodyGeneration.GetSphereMesh ();
		MeshRenderer mr = planetObject.AddComponent<MeshRenderer> ();

		mr.material.mainTexture = Noise.CalculateSeamlessNoise (pixWidth, pixHeight, scale, xOrg, yOrg, _seed, PlanetColour.GetPlanetColour (_planetType));

//		if (_planetType == PlanetType.Molten) {
//			planetObject.GetComponent<MeshRenderer> ().material.EnableKeyword ("_EMISSION");
//			planetObject.GetComponent<MeshRenderer> ().material.SetTexture ("_EmissionMap", Noise.CalculateSeamlessNoise (pixWidth, pixHeight, scale, xOrg, yOrg, _seed, PlanetColour.GetPlanetColour (_planetType)));
//			planetObject.GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", new Color(0.5f,0.5f,0.5f));
//			planetObject.GetComponent<MeshRenderer> ().material.SetFloat ("_Metallic", 1);
//			planetObject.GetComponent<MeshRenderer> ().material.SetFloat ("_Glossiness", 1);
//		}
		if (_planetType != PlanetType.Molten) {
			//cloud = new Clouds[numberOfCloudLayers];

		}

		return planetObject;



	}
	public static Vector3 MoonPoint (int i){

		Vector3 direction = Random.insideUnitCircle.normalized;
		direction.z = direction.y;
		direction.y = 0;

		//Vector3 distance = new Vector3 (7 * (i+1),Random.value,7 * (i+1));

		Vector3 point = direction * (1 * (i+1));
		return point;
	}
	public static GameObject CreateCloud (){

		GameObject cloudObject = new GameObject ("Cloud");
		//cloudObject.transform.position = planetObject.transform.position;
		//cloudObject.transform.parent = planetObject.transform;
		//cloudObject.AddComponent<Clouds> ();

		cloudObject.AddComponent<MeshFilter> ().mesh = SolarBodyGeneration.GetSphereMesh ();
		cloudObject.transform.localScale = new Vector3 (1.03f, 1.03f, 1.03f);

		int pixWidthC = 128;
		int pixHeightC = 128;
		int scaleC = 10;
		float xOrgC = Random.Range (0, 999.9f);
		float yOrgC = Random.Range (0, 999.9f);

		cloudObject.AddComponent<MeshRenderer> ();		

		//r = gameObject.GetComponent<MeshRenderer> ();		

		//noiseTex = new Texture2D (pixWidth, pixHeight);
		//pix = new Color[noiseTex.width * noiseTex.height];

		//r.material.mainTexture = noiseTex;

		//NewCloud (_scale,_rotationSpeed);



		MeshRenderer mr = cloudObject.GetComponent<MeshRenderer> ();



		mr.material.mainTexture = Noise.CalculatePerlinNoise (pixWidthC, pixHeightC, scaleC, xOrgC, yOrgC, PlanetColour.GetCloudColour ());


		mr.material.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		mr.material.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		mr.material.SetInt ("_ZWrite", 0);
		mr.material.DisableKeyword ("_ALPHATEST_ON");
		mr.material.EnableKeyword ("_ALPHABLEND_ON");
		mr.material.DisableKeyword ("_ALPHAPREMULTIPLY_ON");
		mr.material.renderQueue = 3000;

		return cloudObject;
	}
	public static GameObject CreateMoon (){

		GameObject moonObject = new GameObject ("Moon");
		//moonObject.transform.position = MoonPoint(0) + planetObject.transform.position;

		float sizeM = Random.Range (0.5f, 2.0f);

		moonObject.transform.localScale = new Vector3 (sizeM,sizeM, sizeM);
		//moonObject.transform.parent = planetObject.transform;
		//newPlanet.AddComponent<Planet> ();



		///	planet = newPlanet.GetComponent<Planet> ();
		int pixWidthM = 128;
		int pixHeightM = 128;
		int scaleM = 4;
		float xOrgM = 0;
		float yOrgM = 0;
		int seedM = Random.Range (0, 999);

		//planetMesh = SolarBodyGeneration.GetSphereMesh ();
		moonObject.AddComponent<MeshFilter> ().mesh = SolarBodyGeneration.GetSphereMesh ();
		MeshRenderer mr =  moonObject.AddComponent<MeshRenderer> ();

		mr.material.mainTexture = Noise.CalculateSeamlessNoise (pixWidthM, pixHeightM, scaleM, xOrgM, yOrgM, seedM, PlanetColour.GetPlanetColour ((PlanetType)Random.Range (0, 15)));


		return moonObject;

	}
	public static GameObject CreateStar (Vector3 _position,int _seed,StarType _starType){

		GameObject starObject = new GameObject ("Star");
		starObject.transform.position = _position;
		//planetObject.transform.parent = transform;
		//newPlanet.AddComponent<Planet> ();

		int size = Random.Range (20, 30);


		starObject.transform.localScale = new Vector3 (size, size, size);

		///	planet = newPlanet.GetComponent<Planet> ();
		int pixWidth = 256;
		int pixHeight = 256;
		int scale = 6;
		float xOrg = 0;
		float yOrg = 0;

		//planetMesh = SolarBodyGeneration.GetSphereMesh ();
		starObject.AddComponent<MeshFilter> ().mesh = SolarBodyGeneration.GetSphereMesh ();
		MeshRenderer mr = starObject.AddComponent<MeshRenderer> ();


		//StarType st = (StarType)Random.Range (0, 6);


		mr.material.mainTexture = Noise.CalculateSeamlessNoise (pixWidth, pixHeight, scale, xOrg, yOrg, _seed, StarColour.GetStarColour (_starType));

		mr.material.EnableKeyword ("_EMISSION");
		mr.material.SetTexture ("_EmissionMap", Noise.CalculateSeamlessNoise (pixWidth, pixHeight, scale, xOrg, yOrg, _seed, StarColour.GetStarColour (_starType)));
		mr.material.SetColor ("_EmissionColor",Color.white);

		return starObject;



	}
	public static GameObject CreateAsteroid (Vector3 _position){

		GameObject asteroidObject = new GameObject ("Asteroid");
		asteroidObject.transform.position = _position;
		//planetObject.transform.parent = transform;
		//newPlanet.AddComponent<Planet> ();

		int size = Random.Range (2, 4);


		asteroidObject.transform.localScale = new Vector3 (size, size, size);

		///	planet = newPlanet.GetComponent<Planet> ();
		int pixWidth = 18;
		int pixHeight = 18;
		int scale = 1;
		float xOrg = 0;
		float yOrg = 0;
		int _seed = Random.Range(0,500);

		//planetMesh = SolarBodyGeneration.GetSphereMesh ();
		asteroidObject.AddComponent<MeshFilter> ().mesh = SolarBodyGeneration.RandomisedIcoSphere ();
		MeshRenderer mr = asteroidObject.AddComponent<MeshRenderer> ();

		mr.material.mainTexture = Noise.CalculateSeamlessNoise (pixWidth, pixHeight, scale, xOrg, yOrg, _seed, PlanetColour.GetBarrenColour());



		return asteroidObject;



	}
}
