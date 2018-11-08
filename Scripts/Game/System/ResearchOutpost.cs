using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchOutpost {

	public static int TradeInResearchPoints (int _researchPoints){

		return 0;
	}

	public static int BuyClue(int _researchPoints,bool[] clueKnown,out bool succesful){
		int cost = 100;

		if (_researchPoints >= cost) {
			for (int i = 0; i < clueKnown.Length; i++) {
				if (clueKnown [i] == false) {
					succesful = true;
					clueKnown [i] = true;
					_researchPoints -= cost;
					return _researchPoints;
				}
			}
		}

		succesful = false;
		return _researchPoints;
	}
	public static int BuyResearchPoints(int _money,out bool succesful){
		int cost = 100;

		if (_money >= cost) {

			succesful = true;
			_money -= cost;
			return _money;
		}

		succesful = false;
		return _money;

	}
}
