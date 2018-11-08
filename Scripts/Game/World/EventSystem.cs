using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSystem : MonoBehaviour {


	public Main main;
	public GameObject eventObject;
	public GameObject eventFeedbackObject;
	public Text eventFeedbackObjectText;
	public Text nameText;
	public Text eventText;
	public Text[] buttonText;

	public int currentEvent;
	//public int nextEvent;

	public int[] eventList = new int[]{
		58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80
	};
	public int[] mainEventList = new int[]{
		1,5,11,15,21,28,33,39,44,53
	};
	void Start () {
		//OpenEvent (0);
	}

	public void OpenEvent(int _event) {
		EventStruct newEvent = EventControl.GetEventStruct (_event);
		eventObject.SetActive (true);
		nameText.text = newEvent.name;
		eventText.text = newEvent.eventText;

		for (int i = 0; i < newEvent.buttonText.Length; i++) {
			buttonText [i].transform.parent.gameObject.SetActive (false);
		}
		for (int i = 0; i < newEvent.buttonText.Length; i++) {
			buttonText [i].transform.parent.gameObject.SetActive (true);
			buttonText[i].text = newEvent.buttonText[i];
		}
		currentEvent = _event;
	}
	public void Button (int _button){
		EventConsequence (currentEvent, _button);
	}
	public void EventConsequence (int _event,int _choice){

		EventConsequences newConsequence = EventControl.GetEventStruct (_event).eventConsequences[_choice];



		int nextEvent = newConsequence.nextEvent;

		string _feedbackText = "";
		if (nextEvent > 0) {
			OpenEvent (nextEvent);
			return;
		}


		if (newConsequence.newClue) {
			main.mainEventProgress++;
			main.NextClue (newConsequence.clueKnown);
			_feedbackText += "\nNew Clue";
		}

		if (newConsequence.money != 0) {
			_feedbackText += "\nUnits " + newConsequence.money.ToString();
			main.ship.money += newConsequence.money;
			if (main.ship.money < 0) {
				main.ship.strength -= 1;
				_feedbackText += " Not Enough Units";
			}
		}
		if (newConsequence.research != 0) {
			_feedbackText += "\nResearch " + newConsequence.research.ToString();
			main.ship.research += newConsequence.research;
		}
		if (newConsequence.strength != 0) {
			_feedbackText += "\nStrength " + newConsequence.strength.ToString();

			main.ship.strength += newConsequence.strength;
			if (main.ship.strength > main.ship.maxStrength) {
				main.ship.strength = main.ship.maxStrength;
			}
		}

		if (newConsequence.jumpCharges != 0) {
			_feedbackText += "\nJumpCharges " + newConsequence.jumpCharges.ToString();
			main.ship.jumpCharges += newConsequence.jumpCharges;
			if (main.ship.jumpCharges > main.ship.maxJumpCharges) {
				main.ship.jumpCharges = main.ship.maxJumpCharges;
			}
		}



		//main.inEvent = false;
		main.SetBlockingCanvas (false, 1);
		//main.shipNavigationUI.SetActive (true);
		eventObject.SetActive (false);

		eventFeedbackObjectText.text = _feedbackText;
		StartCoroutine ("EventFeedbackObject");
	}
		
	IEnumerator EventFeedbackObject()
	{
		eventFeedbackObject.SetActive (true);

		yield return new WaitForSeconds (3);

		eventFeedbackObject.SetActive (false);
	}

}
[System.Serializable]
public struct EventStruct {

	public string name;
	public string eventText;

	public string[] buttonText;

	public EventConsequences[] eventConsequences;

	public EventStruct (string _name,string _eventText,string[] _buttonText,EventConsequences[] _eventConsequences){

		name = _name;
		eventText = _eventText;
		buttonText = _buttonText;
		eventConsequences = _eventConsequences;
	}


}
public struct EventConsequences {

	public int nextEvent;
	public bool newClue;
	public int[] clueKnown;
	public int strength;
	public int research;
	public int money;
	public int jumpCharges;


	public EventConsequences (int _nextEvent,bool _newClue,int[] _clueKnown){
		nextEvent = _nextEvent;
		newClue = _newClue;
		clueKnown = _clueKnown;
		strength = 0;
		research = 0;
		money = 0;
		jumpCharges = 0;
	}

	public EventConsequences (int _nextEvent,bool _newClue,int[] _clueKnown,int _strength,int _research, int _money, int _jumpCharges){

		nextEvent = _nextEvent;
		newClue = _newClue;
		clueKnown = _clueKnown;
		strength = _strength;
		research = _research;
		money = _money;
		jumpCharges = _jumpCharges;
	}
	
	public EventConsequences (int _nextEvent){

		nextEvent = _nextEvent;
		newClue = false;
		clueKnown = new int[0];
		strength = 0;
		research = 0;
		money = 0;
		jumpCharges = 0;
	}
}
public class EventControl {
	
	public static EventStruct GetEventStruct (int _event){
		EventStruct newEvent = new EventStruct();

		switch (_event) {

		case 0:
			 newEvent = new EventStruct (
				"N/A",
				"N/A", 
				new string[]{
					"N/A"
				}, 
				new EventConsequences[]{
					new EventConsequences()
				}
			);

			break;
		case 1:
			newEvent = new EventStruct (
				"New Orders",
				"We have got reports from an observer saying they have found the information regarding the location of the tomb", 
				new string[]{
					"Continue" 
				}, 
				new EventConsequences[]{
					new EventConsequences(2)
				}
			);

			break;
		case 2:
			newEvent = new EventStruct (
				"New Orders",
				"You will be given a sizeable grant to find the tomb and a free reign to do so", 
				new string[]{
					"Continue" 
				}, 
				new EventConsequences[]{
					new EventConsequences(3)
				}
			);

			break;
		case 3:
			newEvent = new EventStruct (
				"New Orders",
				"Remember you should use us to help you find the tomb", 
				new string[]{
					"Continue" 
				}, 
				new EventConsequences[]{
					new EventConsequences(4)
				}
			);

			break;
		case 4:
			newEvent = new EventStruct (
				"New Orders",
				"Now go and find the tomb", 
				new string[]{
					"End" 
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{0,1,2,3,4})
				}
			);

			break;
		case 5:
			newEvent = new EventStruct (
				"Mystery Station",
				"As you approach the station your scanners begin to scramble", 
				new string[]{
					"Continue" 
				}, 
				new EventConsequences[]{
					new EventConsequences(6)
				}
			);
			break;
		case 6:
			newEvent = new EventStruct (
				"Mystery Station",
				"Through the mess your second makes out a ship", 
				new string[]{
					"Scan For Signals",
					"Continue To Station"
				}, 
				new EventConsequences[]{
					new EventConsequences(7),
					new EventConsequences(8)
				}
			);
			break;
		case 7:
			newEvent = new EventStruct (
				"Mystery Station",
				"You are not able to pick up any message through the mess", 
				new string[]{
					"Increase scanning"
				}, 
				new EventConsequences[]{
					new EventConsequences(9)
				}
			);
			break;
		case 8:
			newEvent = new EventStruct (
				"Mystery Station",
				"As you move towards the station your ship receives a big data transfer", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(10)
				}
			);
			break;
		case 9:
			newEvent = new EventStruct (
				"Mystery Station",
				"A plip appears on the system. It turns out to be a black sphere", 
				new string[]{
					"Leave it alone",
					"Collect the black sphere"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{1}),
					new EventConsequences(0,true,new int[]{1,2,4})
				}
			);
			break;
		case 10:
			newEvent = new EventStruct (
				"Mystery Station",
				"What should you do with the transfer", 
				new string[]{
					"Stop the transfer",
					"leave it be",
					"Control the data flow"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{2}),
					new EventConsequences(0,true,new int[]{1,2,4},-2,0,0,0),
					new EventConsequences(0,true,new int[]{1,2,3,4})
				}
			);
			break;
		case 11:
			newEvent = new EventStruct (
				"Expedition Force",
				"You meet up with an expedition force on a remote outpost", 
				new string[]{
					"Transfer the data",
					"Collect the data"
				}, 
				new EventConsequences[]{
					new EventConsequences(12),
					new EventConsequences(14)
				}
			);
			break;
		case 12:
			newEvent = new EventStruct (
				"Expedition Force",
				"Whilst transfering information between the ships an analyst uncovers something", 
				new string[]{
					"Order a closer inspection",
					"Thank them for the data and leave"
				}, 
				new EventConsequences[]{
					new EventConsequences(13),
					new EventConsequences(0,true,new int[]{2,3,4})
				}
			);
			break;
		case 13:
			newEvent = new EventStruct (
				"Expedition Force",
				"On deeper inspection it is realised that the information has less to give than you thought", 
				new string[]{
					"Thank them for the data and leave"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{2,4})
				}
			);
			break;
		case 14:
			newEvent = new EventStruct (
				"Expedition Force",
				"The commander of the force gives you the data that they think you want", 
				new string[]{
					"Thank them for the data and leave"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{4})
				}
			);
			break;
		case 15:
			newEvent = new EventStruct (
				"Radar Improvement",
				"Whilst improving your ships radar your techniction discovers an unkown piece of equipment", 
				new string[]{
					"Setup a trap",
					"Remove and examine the equipment"
				}, 
				new EventConsequences[]{
					new EventConsequences(16),
					new EventConsequences(20)
				}
			);
			break;

		case 16:
			newEvent = new EventStruct (
				"Radar Improvement",
				"You set up a trap with the false information you fed it", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(17)
				}
			);
			break;
		case 17:
			newEvent = new EventStruct (
				"Radar Improvement",
				"The trap reveals a ship following you", 
				new string[]{
					"Communicate",
					"Enage in Combat"
				}, 
				new EventConsequences[]{
					new EventConsequences(18),
					new EventConsequences(19)
				}
			);
			break;
		case 18:
			newEvent = new EventStruct (
				"Radar Improvement",
				"Communications with the ship exposes that they are from a rival company tasked with the same objective as you", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{0,2},0,100,0,0)
				}
			);
			break;
		case 19:
			newEvent = new EventStruct (
				"Radar Improvement",
				"Engaging the ship in combat results in damage to the ship", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{1,4},-3,50,0,0)
				}
			);
			break;
		case 20:
			newEvent = new EventStruct (
				"Radar Improvement",
				"Examining the piece of equipment gives details about who placed it", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{3},0,250,0,0)
				}
			);
			break;
		case 21:
			newEvent = new EventStruct (
				"Trading Station",
				"You dock with the trading station after receiving reports of a valuable information", 
				new string[]{
					"Meet Contact",
					"Send Team"
				}, 
				new EventConsequences[]{
					new EventConsequences(22),
					new EventConsequences(26)
				}
			);
			break;
		case 22:
			newEvent = new EventStruct (
				"Trading Station",
				"Your contact at the station tells you that what you are looking for has been taken a local gang", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(23)
				}
			);
			break;
		case 23:
			newEvent = new EventStruct (
				"Trading Station",
				"As you enter the area you notice the gang members lurking around", 
				new string[]{
					"Talk",
					"Engage"
				}, 
				new EventConsequences[]{
					new EventConsequences(24),
					new EventConsequences(25)
				}
			);
			break;
		case 24:
			newEvent = new EventStruct (
				"Trading Station",
				"The gang agrees to sell you the information at a very high price", 
				new string[]{
					"Accept",
					"Decline"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{0,2,4},0,25,-250,0),
					new EventConsequences(25)
				}
			);
			break;
		case 25:
			newEvent = new EventStruct (
				"Trading Station",
				"As soon as you enter combat wih the gang they immedietely run away leaving behind the information", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{0,2,4},2,25,0,0)
				}
			);
			break;

		case 26:
			newEvent = new EventStruct (
				"Trading Station",
				"You send a team to retrieve the information as you wait aboard the station", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(27)
				}
			);
			break;
		case 27:
			newEvent = new EventStruct (
				"Trading Station",
				"A few hours later your team returns with a part of the information", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{1,3},0,10,0,0)
				}
			);
			break;

		case 28:
			newEvent = new EventStruct (
				"Asteroid Base",
				"The asteroid base that hold information you want is in radar range", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(29)
				}
			);
			break;
		case 29:
			newEvent = new EventStruct (
				"Asteroid Base",
				"An automatic message repeats'Do not approach'", 
				new string[]{
					"Ignore",
					"Listen"
				}, 
				new EventConsequences[]{
					new EventConsequences(30),
					new EventConsequences(32)
				}
			);
			break;
		case 30:
			newEvent = new EventStruct (
				"Asteroid Base",
				"There appers to be no visble signs of life aboard the base", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(31)
				}
			);
			break;
		case 31:
			newEvent = new EventStruct (
				"Asteroid Base",
				"You secure the data and make a way back to the ship", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{0,1,3},0,150,50,0)
				}
			);
			break;
		case 32:
			newEvent = new EventStruct (
				"Asteroid Base",
				"You listen to the message and leave", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{},2,25,0,0)
				}
			);
			break;

		case 33:
			newEvent = new EventStruct (
				"Unkown Vessel",
				"Your scanners pick up an unkown vessel", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(34)
				}
			);
			break;
		case 34:
			newEvent = new EventStruct (
				"Unkown Vessel",
				"The ship appears to be of unkown origin", 
				new string[]{
					"Communicate",
					"Board"
				}, 
				new EventConsequences[]{
					new EventConsequences(35),
					new EventConsequences(36)
				}
			);
			break;
		case 35:
			newEvent = new EventStruct (
				"Unkown Vessel",
				"Communications with the ship fail", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(36)
				}
			);
			break;
		case 36:
			newEvent = new EventStruct (
				"Unkown Vessel",
				"Your boarding party reaches the other ships and begins to search", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(37)
				}
			);
			break;
		case 37:
			newEvent = new EventStruct (
				"Unkown Vessel",
				"You are told of empty cryocapsules aboard the ship", 
				new string[]{
					"Investigate",
					"Leave"
				}, 
				new EventConsequences[]{
					new EventConsequences(38),
					new EventConsequences(0,true,new int[]{},1,50,0,0)
				}
			);
			break;
		case 38:
			newEvent = new EventStruct (
				"Unkown Vessel",
				"Investigation into the capsules reveal that they have been empty for hundreds of years", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{2,3,4},1,30,0,0)
				}
			);
			break;
		case 39:
			newEvent = new EventStruct (
				"Message",
				"Your communicator informs you of a message being sent to you 'Help us we are un' the message ends there", 
				new string[]{
					"Approach",
					"Send Probe",
					"Leave"
				}, 
				new EventConsequences[]{
					new EventConsequences(40),
					new EventConsequences(43),
					new EventConsequences(0,true,new int[]{},1,0,0,0)
				}
			);
			break;
		case 40:
			newEvent = new EventStruct (
				"Message",
				"You aproach the signal but before being able to get visuals on the origins of the message, you crew begins to show signs of illness", 
				new string[]{
					"Continue",
					"Leave"
				}, 
				new EventConsequences[]{
					new EventConsequences(41),
					new EventConsequences(0,true,new int[]{},1,15,0,0)
				}
			);
			break;
		case 41:
			newEvent = new EventStruct (
				"Message",
				"before long all the crew are down", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(42)
				}
			);
			break;
		case 42:
			newEvent = new EventStruct (
				"Message",
				"You are woken by your second who slapping you, she informs you that the ship had been pilaged by an unkown group", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{1},-3,-30,-250,-1)
				}
			);
			break;
		case 43:
			newEvent = new EventStruct (
				"Message",
				"The probe reveals a hidden ship hidding behind a gas cloud waiting to ambush any ship that approaches", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{2,4},1,25,0,0)
				}
			);
			break;

		case 44:
			newEvent = new EventStruct (
				"Beast",
				"Alarms begin to sound, unsure of what they are for the ship moves to battlestations", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(45)
				}
			);
			break;
		case 45:
			newEvent = new EventStruct (
				"Beast",
				"You get mixed reports of something aboard the ship", 
				new string[]{
					"Secure",
					"Hunt"
				}, 
				new EventConsequences[]{
					new EventConsequences(46),
					new EventConsequences(48)
				}
			);
			break;
		case 46:
			newEvent = new EventStruct (
				"Beast",
				"Your security teams locks down key sections of your ship", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(47)
				}
			);
			break;
		case 47:
			newEvent = new EventStruct (
				"Beast",
				"The ship is being cleared section by section", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(49)
				}
			);
			break;
		case 48:
			newEvent = new EventStruct (
				"Beast",
				"You send the security team to clear the ship section by section", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(50)
				}
			);
			break;
		case 49:
			newEvent = new EventStruct (
				"Beast",
				"The beast is cornered into a section of the ship", 
				new string[]{
					"Send In Team",
					"Flush out Into Space"
				}, 
				new EventConsequences[]{
					new EventConsequences(51),
					new EventConsequences(52)
				}
			);
			break;
		case 50:
			newEvent = new EventStruct (
				"Beast",
				"The beast gets cornered in the engine room and damages the engines before being taken down", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{2,3,4},-3,125,0,0)
				}
			);
			break;
		case 51:
			newEvent = new EventStruct (
				"Beast",
				"The beast gets blown out into the vacuum of space", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{2,3},-1,20,0,0)
				}
			);
			break;
		case 52:
			newEvent = new EventStruct (
				"Beast",
				"The beast is taken down alive with very little difficulty", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{1,2,4},-1,220,0,0)
				}
			);
			break;



		case 53:
			newEvent = new EventStruct (
				"Tomb",
				"Finally you have reached the tomb", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(54)
				}
			);
			break;
		case 54:
			newEvent = new EventStruct (
				"Tomb",
				"You send a message back to HQ informing that the tomb has been found", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(55)
				}
			);
			break;
		case 55:
			newEvent = new EventStruct (
				"Tomb",
				"Without waiting for a reply you order the ship to dock", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(56)
				}
			);
			break;
		case 56:
			newEvent = new EventStruct (
				"Tomb",
				"Your crew is excited to explore the tomb", 
				new string[]{
					"Continue"
				}, 
				new EventConsequences[]{
					new EventConsequences(57)
				}
			);
			break;
		case 57:
			newEvent = new EventStruct (
				"Tomb",
				"You lead you crew in exploration of the tomb", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,true,new int[]{})
				}
			);
			break;
			// Single Events
		case 58:
			newEvent = new EventStruct (
				"",
				"Your lead scientist asks if the ship can remain here for his experiment to be completed", 
				new string[]{
					"Accept",
					"Decline"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,10,0,0),
					new EventConsequences(0,false,new int[]{},1,0,0,0)
				}
			);
			break;
		case 59:
			newEvent = new EventStruct (
				"",
				"You discover a hidden vessel inside an asteroid", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,20,20,1)
				}
			);
			break;
		case 60:
			newEvent = new EventStruct (
				"",
				"Members of your crew are experiencing incereased intelligence", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,80,0,0)
				}
			);
			break;
		case 61:
			newEvent = new EventStruct (
				"",
				"You receive a mysterious signal", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,20,10,0)
				}
			);
			break;
		case 62:
			newEvent = new EventStruct (
				"",
				"Your engineers have upgraded your jump drive", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,0,0,2)
				}
			);
			break;
		case 63:
			newEvent = new EventStruct (
				"",
				"Your ship takes a hit from a object peircing the hull", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},-5,0,0,0)
				}
			);
			break;
		case 64:
			newEvent = new EventStruct (
				"",
				"Your last jump caused too much stress on your ship and is causing damage", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},-3,0,0,-1)
				}
			);
			break;
		case 65:
			newEvent = new EventStruct (
				"",
				"You get lucky and find a hidden stash of units", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,0,50,0)
				}
			);
			break;
		case 66:
			newEvent = new EventStruct (
				"",
				"You find a stash of jump charges", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,0,0,3)
				}
			);
			break;
		case 67:
			newEvent = new EventStruct (
				"",
				"You find a stash of strength upgrades", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},5,0,0,0)
				}
			);
			break;
		case 68:
			newEvent = new EventStruct (
				"",
				"Your emplyer has sent you a sum of resources to aid you", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},1,30,40,1)
				}
			);
			break;
		case 69:
			newEvent = new EventStruct (
				"",
				"Your crew are beginning to get uneasy", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},-2,-20,-10,0)
				}
			);
			break;
		case 70:
			newEvent = new EventStruct (
				"",
				"You head of security has asked to run a full scan on the androids in the ship", 
				new string[]{
					"Accept",
					"Decline"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},1,-20,-10,0),
					new EventConsequences(0,false,new int[]{},-2,20,0,0)
				}
			);
			break;
		case 71:
			newEvent = new EventStruct (
				"",
				"Experiments by your science team reveal extra strength to the ship", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},3,0,0,0)
				}
			);
			break;
		case 72:
			newEvent = new EventStruct (
				"",
				"Experiments by your science team have given your ship extra jump charges", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,0,0,2)
				}
			);
			break;
		case 73:
			newEvent = new EventStruct (
				"",
				"Experiments by your science team give a greater insight", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,60,0,0)
				}
			);
			break;
		case 74:
			newEvent = new EventStruct (
				"",
				"New equipment has increased the ships strength", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},2,0,0,0)
				}
			);
			break;
		case 75:
			newEvent = new EventStruct (
				"",
				"You have received a transfer of units", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,0,100,0)
				}
			);
			break;
		case 76:
			newEvent = new EventStruct (
				"",
				"Unfortunately there was a glitch in your ships system and you have lost some units", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,0,-100,0)
				}
			);
			break;
		case 77:
			newEvent = new EventStruct (
				"",
				"Unfortunately whilst travelling the ships has suffered a failure to the jump drive", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},0,0,0,-2)
				}
			);
			break;
		case 78:
			newEvent = new EventStruct (
				"",
				"A debilitating illness has ravaged your crew putting crital systems at risk", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},-5,-80,-50,-2)
				}
			);
			break;
		case 79:
			newEvent = new EventStruct (
				"",
				"Members of the science team have realised their experiment was corrupted", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},-1,-100,-10,0)
				}
			);
			break;
		case 80:
			newEvent = new EventStruct (
				"",
				"Experiments by the science team didn't go great with a section of the ship missing", 
				new string[]{
					"End"
				}, 
				new EventConsequences[]{
					new EventConsequences(0,false,new int[]{},-5,-100,-100,-1)
				}
			);
			break;
		default:
			
			break;

		}


		return newEvent;
	}



}