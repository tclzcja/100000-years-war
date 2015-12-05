using UnityEngine;
using System.Collections;

namespace Josh
{
	public class UI : MonoBehaviour
	{
		private Josh.Unit currentUnit;
		private Josh.Unit enemyUnit;

		public GUIStyle bgStyle;
		private Texture2D bgStyleLeft;
		private Texture2D bgStyleRight;
//		public int guiX;
//		public int guiY;
//		public int guiWidth;
//		public int guiHeight;

		public GUIStyle gameInfoStyle;			//The line at the top that displays what round it is and the planet's hp
		private const int gameInfoYStart = 15;
		private const int gameInfoWidth = 200;
		private const int gameInfoHeight = 50;

		public GUIStyle pInfoStyle;
		private const int pInfoYStart = 65;
		private const int pInfoWidth = 200;
		private const int pInfoHeight = 50;

		public GUIStyle shipNameStyle;
		private const int shipNameX = 130;
		private int enemyShipNameX = Screen.width - 245;

		public GUIStyle statNameStyle;
		public GUIStyle statNumStyle;
		private const int statNameX = 70;
		private const int statNumXOffset = 120;	//How far to the right of the names the numbers are displayed
		private const int statYStart = 10;
		private const int statYOffset = 35;
		private const int statLineWidth = 100;
		private const int statLineHeight = 30;
		private int enemyStatNameX = Screen.width - 305;


		private string[] gameInfo;				//Contains info displayed at top of screen

		public void Awake ()
		{
			gameInfo = new string[] {"Player_1", "", "", "", ""};
	
			bgStyleLeft = Resources.Load ("Textures/UI/Interface-BG-Left") as Texture2D;
			bgStyleRight = Resources.Load ("Textures/UI/Interface-BG-Right") as Texture2D;
		}

		public void OnGUI ()
		{
			string pInfoLine = "";

//			GUI.Label (new Rect(Screen.width/2 - 480, 0, 1140, 150), "", bgStyle);

			//PLAYER STATE: Current player's name, how many active and total units they have
			if (gameInfo[0].Equals ("Player_1"))
			{
				pInfoLine += "Nouvelle Entente";
				pInfoStyle.normal.textColor = new Color(255, 0, 0);
			}
			
			else if (gameInfo[0].Equals ("Player_2"))
			{
				pInfoLine += "Anglian Federation";
				pInfoStyle.normal.textColor = new Color(0, 0, 255);
			}
			
			else
			{
				Debug.LogWarning ("Error - something messed up while determining current player name in UI");
			}
			
			pInfoLine += " - " + gameInfo[1] + "/" + gameInfo[2] + " Units Active";
			GUI.Label (new Rect (Screen.width/2-80, pInfoYStart, pInfoWidth, pInfoHeight), pInfoLine, pInfoStyle); 

			//GAME STATE: Current round, planetary shield hp
			string gameInfoLine = "Round " + gameInfo[3] + " - Planetary Shield: " + gameInfo[4];
			GUI.Label (new Rect (Screen.width/2-80, gameInfoYStart, gameInfoWidth, gameInfoHeight), gameInfoLine, gameInfoStyle);

			//CURRENT SHIP STATS - sorry for the terrible code
			if (currentUnit != null)
			{
				shipNameStyle.fontSize = 28;

				//				GUI.Label (new Rect(0, 0, 425, 475), "", bgStyle);
//				GUI.Label (new Rect(guiX, guiY, guiWidth, guiHeight), "", bgStyle);
				bgStyle.normal.background = bgStyleLeft;
				GUI.Label (new Rect(-1, -1, 376, 336), "", bgStyle);

				int statY = statYStart;
				if (gameInfo[0].Equals ("Player_1"))
			    {
					shipNameStyle.normal.textColor = new Color(255, 0, 0);
				}
				else if (gameInfo[0].Equals ("Player_2"))
			    {
					shipNameStyle.normal.textColor = new Color(0, 0, 255);
				}
				else
				{
					Debug.LogWarning ("Error - something messed up while determining current player name in UI");
				}
				GUI.Label (new Rect(shipNameX, statY, statLineWidth, statLineHeight), currentUnit.name, shipNameStyle);
				statY += statYOffset;

				GUI.Label (new Rect (statNameX, statY, statLineWidth, statLineHeight), "Class", statNameStyle);
				if (currentUnit is Josh.Unit_Chevalier)
				{
					GUI.Label (new Rect (statNameX + statNumXOffset, statY, statLineWidth, statLineHeight), "Chevalier", statNumStyle);				
				}
				if (currentUnit is Josh.Unit_Halberd)
				{
					GUI.Label (new Rect (statNameX + statNumXOffset, statY, statLineWidth, statLineHeight), "Halberd", statNumStyle);				
				}

				if (currentUnit is Josh.Unit_Yeoman)
				{
					GUI.Label (new Rect (statNameX + statNumXOffset, statY, statLineWidth, statLineHeight), "Yeoman", statNumStyle);				
				}	
				statY += statYOffset;

				GUI.Label (new Rect (statNameX, statY, statLineWidth, statLineHeight), "Hull Strength", statNameStyle); 
				GUI.Label (new Rect (statNameX + statNumXOffset, statY, statLineWidth, statLineHeight), currentUnit.CurHP + "/" + currentUnit.MaxHP, statNumStyle); 
				statY += statYOffset;

				GUI.Label (new Rect (statNameX, statY, statLineWidth, statLineHeight), "Power", statNameStyle); 
				GUI.Label (new Rect (statNameX + statNumXOffset, statY, statLineWidth, statLineHeight), currentUnit.MinPower + "-" + currentUnit.MaxPower, statNumStyle); 
				statY += statYOffset;

				GUI.Label (new Rect (statNameX, statY, statLineWidth, statLineHeight), "Armor", statNameStyle); 
				GUI.Label (new Rect (statNameX + statNumXOffset, statY, statLineWidth, statLineHeight), currentUnit.HighArmor + "/" + currentUnit.MidArmor + "/" + currentUnit.LowArmor, statNumStyle); 
				statY += statYOffset;

				GUI.Label (new Rect (statNameX, statY, statLineWidth, statLineHeight), "Range", statNameStyle); 
				GUI.Label (new Rect (statNameX + statNumXOffset, statY, statLineWidth, statLineHeight), currentUnit.Range + "", statNumStyle); 
				statY += statYOffset;
				
				GUI.Label (new Rect (statNameX, statY, statLineWidth, statLineHeight), "Energy", statNameStyle); 
				GUI.Label (new Rect (statNameX + statNumXOffset, statY, statLineWidth, statLineHeight), currentUnit.Energy + "/" + currentUnit.MaxEnergy, statNumStyle); 
				statY += statYOffset;

				GUI.Label (new Rect (statNameX, statY, statLineWidth, statLineHeight), "Rotation Cost", statNameStyle);
				GUI.Label (new Rect (statNameX + statNumXOffset, statY, statLineWidth, statLineHeight), currentUnit.RotationCost + " Energy", statNumStyle); 
				statY += statYOffset;

				GUI.Label (new Rect (statNameX, statY, statLineWidth, statLineHeight), "Firing Cost", statNameStyle); 
				GUI.Label (new Rect (statNameX + statNumXOffset, statY, statLineWidth, statLineHeight), currentUnit.FiringCost + " Energy", statNumStyle); 
				statY += statYOffset;
			}

			//ENEMY SHIP STATS - sorry again
			if (enemyUnit != null)
			{
				bgStyle.normal.background = bgStyleRight;
				GUI.Label (new Rect(Screen.width - 375, -1, 376, 336), "", bgStyle);

				int statY = statYStart;
				if (gameInfo[0].Equals ("Player_1"))
				{
					shipNameStyle.normal.textColor = new Color(0, 0, 255);
				}
				else if (gameInfo[0].Equals ("Player_2"))
				{
					shipNameStyle.normal.textColor = new Color(255, 0, 0);
				}
				else
				{
					Debug.LogWarning ("Error - something messed up while determining current player name in UI");
				}
			
				if (enemyUnit is Josh.Unit_Planet)
				{
					shipNameStyle.fontSize = 42;
					GUI.Label (new Rect(enemyShipNameX + 10, statY + 70, statLineWidth, statLineHeight), "PLANETARY", shipNameStyle);
					GUI.Label (new Rect(enemyShipNameX + 10, statY + 140, statLineWidth, statLineHeight), "SHIELD", shipNameStyle);
					GUI.Label (new Rect(enemyShipNameX + 10, statY + 210, statLineWidth, statLineHeight), "TARGETED", shipNameStyle);
				}

				else {
					shipNameStyle.fontSize = 28;

					GUI.Label (new Rect(enemyShipNameX, statY, statLineWidth, statLineHeight), enemyUnit.name, shipNameStyle);
					statY += statYOffset;

					GUI.Label (new Rect (enemyStatNameX, statY, statLineWidth, statLineHeight), "Class", statNameStyle);
					if (enemyUnit is Josh.Unit_Chevalier)
					{
						GUI.Label (new Rect (enemyStatNameX + statNumXOffset, statY, statLineWidth, statLineHeight), "Chevalier", statNumStyle);				
					}
					if (enemyUnit is Josh.Unit_Halberd)
					{
						GUI.Label (new Rect (enemyStatNameX + statNumXOffset, statY, statLineWidth, statLineHeight), "Halberd", statNumStyle);				
					}
					
					if (enemyUnit is Josh.Unit_Yeoman)
					{
						GUI.Label (new Rect (enemyStatNameX + statNumXOffset, statY, statLineWidth, statLineHeight), "Yeoman", statNumStyle);				
					}	
					statY += statYOffset;
					
					GUI.Label (new Rect (enemyStatNameX, statY, statLineWidth, statLineHeight), "Hull Strength", statNameStyle); 
					GUI.Label (new Rect (enemyStatNameX + statNumXOffset, statY, statLineWidth, statLineHeight), enemyUnit.CurHP + "/" + enemyUnit.MaxHP, statNumStyle); 
					statY += statYOffset;
					
					GUI.Label (new Rect (enemyStatNameX, statY, statLineWidth, statLineHeight), "Power", statNameStyle); 
					GUI.Label (new Rect (enemyStatNameX + statNumXOffset, statY, statLineWidth, statLineHeight), enemyUnit.MinPower + "-" + enemyUnit.MaxPower, statNumStyle); 
					statY += statYOffset;
					
					GUI.Label (new Rect (enemyStatNameX, statY, statLineWidth, statLineHeight), "Armor", statNameStyle); 
					GUI.Label (new Rect (enemyStatNameX + statNumXOffset, statY, statLineWidth, statLineHeight), enemyUnit.HighArmor + "/" + enemyUnit.MidArmor + "/" + enemyUnit.LowArmor, statNumStyle); 
					statY += statYOffset;
					
					GUI.Label (new Rect (enemyStatNameX, statY, statLineWidth, statLineHeight), "Range", statNameStyle); 
					GUI.Label (new Rect (enemyStatNameX + statNumXOffset, statY, statLineWidth, statLineHeight), enemyUnit.Range + "", statNumStyle); 
					statY += statYOffset;

					GUI.Label (new Rect (enemyStatNameX, statY, statLineWidth, statLineHeight), "Energy", statNameStyle); 
					GUI.Label (new Rect (enemyStatNameX + statNumXOffset, statY, statLineWidth, statLineHeight), enemyUnit.Energy + "/" + enemyUnit.MaxEnergy, statNumStyle); 
					statY += statYOffset;
					
					GUI.Label (new Rect (enemyStatNameX, statY, statLineWidth, statLineHeight), "Rotation Cost", statNameStyle);
					GUI.Label (new Rect (enemyStatNameX + statNumXOffset, statY, statLineWidth, statLineHeight), enemyUnit.RotationCost + " Energy", statNumStyle); 
					statY += statYOffset;

					GUI.Label (new Rect (enemyStatNameX, statY, statLineWidth, statLineHeight), "Firing Cost", statNameStyle); 
					GUI.Label (new Rect (enemyStatNameX + statNumXOffset, statY, statLineWidth, statLineHeight), enemyUnit.FiringCost + " Energy", statNumStyle); 
					statY += statYOffset;
				}
			}
		}

		public void UpdateCurrentUnit (GameObject _Unit)
		{
			currentUnit = _Unit.GetComponent ("Unit") as Josh.Unit;
		}

		public void UpdateEnemyUnit (GameObject _Unit)
		{
			if(_Unit != null)
			{ enemyUnit = _Unit.GetComponent ("Unit") as Josh.Unit; }

		}

		public void ClearEnemyUnit ()
		{
			enemyUnit = null;
		}

		public void UpdateGameInfo (string[] infoPackage)
		{
			gameInfo = infoPackage;
		}

	}
}