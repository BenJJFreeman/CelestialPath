using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebulaColour{

	public static Gradient GetColour (int _colour){

		switch (_colour) {
		case 0:
			return GetPurpleColour ();
		case 1:
			return GetBlueColour ();
		case 2:
			return GetYellowColour ();
		case 3:
			return GetGreenColour ();
		default:
			return GetPurpleColour ();
		}
			
	}

	public static Gradient GetPurpleColour (){
		Gradient newG = new Gradient();
		GradientColorKey[] gck = new GradientColorKey[] {				
			new GradientColorKey (new Color ( 0.31373f, 0.17647f, 0.31765f ), 0.0f),
			new GradientColorKey (new Color ( 0.18039f, 0.01569f, 0.25098f ), 1.0f),
			//new GradientColorKey (new Color (1.0f, 0.84f, 0.56f), 0.51f),
			//new GradientColorKey (new Color (0.93f, 0.66f, 0.24f), 0.76f),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
		};
		GradientAlphaKey[] gak = new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 1.0f),
			new GradientAlphaKey (1.0f, 1.0f),
		};
		newG.SetKeys (gck, gak);
		return newG;
	}
	public static Gradient GetBlueColour (){
		Gradient newG = new Gradient();
		GradientColorKey[] gck = new GradientColorKey[] {				
			new GradientColorKey (new Color ( 0.36471f, 0.57255f, 0.81569f ), 0.0f),
			new GradientColorKey (new Color ( 0.11765f, 0.39608f, 0.85490f ), 1.0f),
			//new GradientColorKey (new Color (1.0f, 0.84f, 0.56f), 0.51f),
			//new GradientColorKey (new Color (0.93f, 0.66f, 0.24f), 0.76f),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
		};
		GradientAlphaKey[] gak = new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 1.0f),
			new GradientAlphaKey (1.0f, 1.0f),
		};
		newG.SetKeys (gck, gak);
		return newG;
	}
	public static Gradient GetYellowColour (){
		Gradient newG = new Gradient();
		GradientColorKey[] gck = new GradientColorKey[] {				
			new GradientColorKey (new Color ( 0.86667f, 0.76471f, 0.29412f ), 0.0f),
			new GradientColorKey (new Color ( 0.85490f, 0.61961f, 0.11765f ), 1.0f),
			//new GradientColorKey (new Color (1.0f, 0.84f, 0.56f), 0.51f),
			//new GradientColorKey (new Color (0.93f, 0.66f, 0.24f), 0.76f),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
		};
		GradientAlphaKey[] gak = new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 1.0f),
			new GradientAlphaKey (1.0f, 1.0f),
		};
		newG.SetKeys (gck, gak);
		return newG;
	}
	public static Gradient GetGreenColour (){
		Gradient newG = new Gradient();
		GradientColorKey[] gck = new GradientColorKey[] {				
			new GradientColorKey (new Color ( 0.25882f, 0.43529f, 0.17647f ), 0.0f),
			new GradientColorKey (new Color ( 0.34902f, 0.73725f, 0.27451f ), 1.0f),
			//new GradientColorKey (new Color (1.0f, 0.84f, 0.56f), 0.51f),
			//new GradientColorKey (new Color (0.93f, 0.66f, 0.24f), 0.76f),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
			//new GradientColorKey (new Color (0, 0, 0), 0),
		};
		GradientAlphaKey[] gak = new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 1.0f),
			new GradientAlphaKey (1.0f, 1.0f),
		};
		newG.SetKeys (gck, gak);
		return newG;
	}
}
