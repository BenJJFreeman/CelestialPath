using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {

	public static Texture2D CalculateSeamlessNoise(int width,int height,float scale,float xOrg,float yOrg,int seed,Gradient colouring) {
		Texture2D noiseTex = new Texture2D (width, height);
		Color[] pix = new Color[noiseTex.width * noiseTex.height];

		float y = 0.0F;
		while (y < noiseTex.height) {
			float x = 0.0F;
			while (x < noiseTex.width) {
				float xCoord = xOrg + x / noiseTex.width;
				float yCoord = yOrg + y / noiseTex.height;
				float noise = SimplexNoise.SeamlessNoise (xCoord, yCoord, scale, scale, (float)seed);
				noise = (noise + 1.0f) * .5f;

				pix [(int)(y * noiseTex.width + x)] = colouring.Evaluate (noise);
				x++;
			}
			y++;
		}

		noiseTex.SetPixels (pix);
		noiseTex.Apply ();

		return noiseTex;
	}

	public static Texture2D CalculatePerlinNoise(int width,int height,float scale,float xOrg,float yOrg,Gradient colouring) {
		Texture2D noiseTex = new Texture2D (width, height);
		Color[] pix = new Color[noiseTex.width * noiseTex.height];

		float y = 0.0F;
		while (y < noiseTex.height) {
			float x = 0.0F;
			while (x < noiseTex.width) {
				float xCoord = xOrg + x / noiseTex.width * scale;
				float yCoord = yOrg + y / noiseTex.height * scale;
				float sample = Mathf.PerlinNoise (xCoord, yCoord);
				pix [(int)(y * noiseTex.width + x)] = colouring.Evaluate (sample);
				x++;
			}
			y++;
		}

		noiseTex.SetPixels (pix);
		noiseTex.Apply ();

		return noiseTex;
	}


}
