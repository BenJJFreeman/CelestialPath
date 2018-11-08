using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop{

	public static int PurchaseJumpCharge (int _money,out bool successful){
		int cost = 30;
		successful = false;
		if (cost <= _money) {
			_money -= cost;
			successful = true;
		}

		return _money;
	}
	public static int PurchaseShipStrength (int _money,out bool successful){
		int cost = 20;
		successful = false;
		if (cost <= _money) {
			_money -= cost;
			successful = true;
		}

		return _money;
	}

}
