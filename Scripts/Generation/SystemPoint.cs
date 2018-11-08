using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPoint : MonoBehaviour {

	public int starSystem;

	public void SetStarSystem (int _starSystem){
		starSystem = _starSystem;
	}
	public int GetStarSystem (){
		return starSystem;
	}		
}
