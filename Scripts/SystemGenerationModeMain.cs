using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemGenerationModeMain : MonoBehaviour {

	public GameObject systemGeneratorObj;
	public ParticleSystem[] ps;
	public Slider[] planet;
	public Slider starType;
	public Slider asteroidBelt;
	public InputField seedText;
	public Text[] resultText;
	public void GenerateSystem(){
		
		ParticleSystem.MainModule mainP = ps [1].main;
		mainP.startColor = new ParticleSystem.MinMaxGradient (NebulaColour.GetColour (Random.Range (0, 4)));


		for (int i = 0; i < ps.Length; i++) {
			ps[i].Stop ();
			ps[i].Clear ();
		}
		if (transform.childCount > 0) {
			Destroy (transform.GetChild (0).gameObject);
		}

		int numberOfPlanets;
		numberOfPlanets = (int)Random.Range (planet [0].value, planet [1].value + 1);

		bool hasAsteroidBelt = false;
		if (asteroidBelt.value == 1) {
			hasAsteroidBelt = true;
		} else if (asteroidBelt.value == 0){
			if (Random.Range (0, 100) > 70) {
				hasAsteroidBelt = true;
			}
		}

		StarType st;
		if (starType.value == 0) {
			st = (StarType)Random.Range (0, 6);
		} else {
			st = (StarType)(starType.value-1);
		}

		int seed = Main.IntParseFast (seedText.text);

		GenerateSystem (seed,numberOfPlanets, hasAsteroidBelt, st);

	}

	public void GenerateSystem (int seed,int numberOfPlanets,bool hasAsteroidBelt,StarType starType){
		GameObject newSystem = StarSystemGenerator.CreateSystem (seed,numberOfPlanets,hasAsteroidBelt,starType);
		newSystem.transform.parent = transform;

		Vector3 particlePosition = Random.insideUnitSphere * 200;
		particlePosition.y = Random.Range(-350,-500);
		ps [1].transform.position = particlePosition;


		for (int i = 0; i < ps.Length; i++) {
			ps[i].randomSeed = (uint)Random.Range (0, 99999);
			ps[i].Play ();
		}
	}
	public void RandomSeed(){
		seedText.text = Random.Range (0, 10000000).ToString ();

	}
	public void UpdateSystemGenerator (){

		if (Input.GetKeyDown (KeyCode.H)) {
			HidePanel (!systemGeneratorObj.activeSelf);
		}

	}
	public void HidePanel (bool _active){
		systemGeneratorObj.SetActive (_active);

	}
	public void UpdatePlanetSlider (int _slider){

		if (_slider == 0) {
			if (planet [0].value > planet [1].value) {
				planet [1].value = planet [0].value;
			}
		} else {
			if (planet [1].value < planet [0].value) {
				planet [0].value = planet [1].value;
			}
		}

	}
	public void UpdateResultText (){



		if (starType.value == 0) {
			resultText [0].text = "Random";
		} else {
			resultText [0].text = ((StarType)starType.value-1).ToString ();
		}

		resultText [1].text = planet[0].value.ToString ();

		resultText [2].text = planet[1].value.ToString ();

		if (asteroidBelt.value == 0) {
			resultText [3].text = "Random";
		} else {
			resultText [3].text = (asteroidBelt.value == 1 ? "Yes" : "No");
		}
	}
}
