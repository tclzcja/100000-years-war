using UnityEngine;
using System.Collections;

public class End_UI : MonoBehaviour {

	public GUIStyle winnerStyle;
	public GUIStyle planetStyle;
	public GUIStyle headerStyle;
	public GUIStyle statsStyle;

	private const int winnerY = 20;
	private const int planetY = 70;

	private const int headerY = 100;
	private const int headerWidth = 30;
	private const int headerHeight = 30;

	private const int statsYStart = 155;
	private int statsY;
	public int statsYOffset = 30;
	private const int statsWidth = 30;
	private const int statsHeight = 30;

	private const int p1ShipNameX = 80;
	public int p2ShipNameX = 900;
	public int classNameXOffset = 200;
	public int hullStrengthXOffset = 220;
	public int killsXOffset = 200;

	public string[,] P1_Stats;
	public string[,] P2_Stats;
	//[i,0]: ship name
	//[i,1]: ship class
	//[i,2]: hull strength ("cur/max")
	//[i,3]: kills

	// Use this for initialization
	void Start () {
		P1_Stats = (GameObject.Find ("Player_1").GetComponent ("Player") as Josh.Player).Unit_Stats;
		P2_Stats = (GameObject.Find ("Player_2").GetComponent ("Player") as Josh.Player).Unit_Stats;

		winnerStyle.font = Resources.Load ("Fonts/XOLONIUM-REGULAR") as Font;
		winnerStyle.fontSize = 48;
		winnerStyle.alignment = TextAnchor.UpperCenter;

		headerStyle.font = Resources.Load ("Fonts/XOLONIUM-REGULAR") as Font;
		headerStyle.fontSize = 32;
		headerStyle.alignment = TextAnchor.UpperCenter;

		statsStyle.font = Resources.Load ("Fonts/XOLONIUM-REGULAR") as Font;
		statsStyle.fontSize = 20;
		statsStyle.alignment = TextAnchor.UpperCenter;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("X"))
		{
			Application.LoadLevel("1");
		}
	}

	public void OnGUI () {

		Josh.Player p1 = GameObject.Find ("Player_1").GetComponent ("Player") as Josh.Player;
		Josh.Player p2 = GameObject.Find ("Player_2").GetComponent ("Player") as Josh.Player;

		if (p1.Unit_Number == 0) {
			winnerStyle.normal.textColor = new Color (255, 0, 0);
			GUI.Label (new Rect(Screen.width/2 - 50, winnerY, 200, 50), "Victory: Anglian Federation!", winnerStyle);
		}
		else {
			winnerStyle.normal.textColor = new Color (0, 0, 255);
			GUI.Label (new Rect(Screen.width/2 - 50, winnerY, 200, 50), "Victory: Nouvelle Entente!", winnerStyle);
		}
		/**
		//TODO - planetary shield
	
		headerStyle.normal.textColor = new Color (0, 0, 255);
		GUI.Label (new Rect(350, headerY, 100, 50), "Name           Class           Hull Strength           Kills", headerStyle);     
		headerStyle.normal.textColor = new Color (255, 0, 0);
		GUI.Label (new Rect(Screen.width-450, headerY, 100, 50), "Name           Class           Hull Strength           Kills", headerStyle);     

		statsY = statsYStart;

		for (int i = 0; i < p1.Max_Units; i++) {
			GUI.Label (new Rect(p1StatsX, statsY, 50, 50), p1.Unit_Stats[i], statsStyle);
			statsY += statsYOffset;
		}

		statsY = statsYStart;
		for (int i = 0; i < p2.Max_Units; i++) {
			GUI.Label (new Rect(p2StatsX, statsY, 50, 50), p2.Unit_Stats[i], statsStyle);
			statsY += statsYOffset;
		}
		*/

		headerStyle.normal.textColor = new Color (0, 0, 255);
		GUI.Label (new Rect(p1ShipNameX, headerY, headerWidth, headerHeight), "Ship", headerStyle);
		GUI.Label (new Rect(p1ShipNameX + classNameXOffset, headerY, headerWidth, headerHeight), "Class", headerStyle);
		GUI.Label (new Rect(p1ShipNameX + classNameXOffset + hullStrengthXOffset, headerY, headerWidth, headerHeight), "Hull Strength", headerStyle);
		GUI.Label (new Rect(p1ShipNameX + classNameXOffset + hullStrengthXOffset + killsXOffset, headerY, headerWidth, headerHeight), "Kills", headerStyle);

		headerStyle.normal.textColor = new Color (255, 0, 0);
		GUI.Label (new Rect(p2ShipNameX, headerY, headerWidth, headerHeight), "Ship", headerStyle);
		GUI.Label (new Rect(p2ShipNameX + classNameXOffset, headerY, headerWidth, headerHeight), "Class", headerStyle);
		GUI.Label (new Rect(p2ShipNameX + classNameXOffset + hullStrengthXOffset, headerY, headerWidth, headerHeight), "Hull Strength", headerStyle);
		GUI.Label (new Rect(p2ShipNameX + classNameXOffset + hullStrengthXOffset + killsXOffset, headerY, headerWidth, headerHeight), "Kills", headerStyle);

		statsY = statsYStart;
		statsStyle.normal.textColor = new Color(255, 255, 255);

		for (int i = 0; i < p1.Max_Units; i++) {
			GUI.Label (new Rect(p1ShipNameX, statsY, statsWidth, statsHeight), p1.Unit_Stats[i,0], statsStyle);
			GUI.Label (new Rect(p1ShipNameX + classNameXOffset, statsY, statsWidth, statsHeight), p1.Unit_Stats[i,1], statsStyle);
			GUI.Label (new Rect(p1ShipNameX + classNameXOffset + hullStrengthXOffset, statsY, statsWidth, statsHeight), p1.Unit_Stats[i,2], statsStyle);
			GUI.Label (new Rect(p1ShipNameX + classNameXOffset + hullStrengthXOffset + killsXOffset, statsY, statsWidth, statsHeight), p1.Unit_Stats[i,3], statsStyle);
			statsY += statsYOffset;
		}

		statsY = statsYStart;
		for (int i = 0; i < p2.Max_Units; i++) {
			GUI.Label (new Rect(p2ShipNameX, statsY, statsWidth, statsHeight), p2.Unit_Stats[i,0], statsStyle);
			GUI.Label (new Rect(p2ShipNameX + classNameXOffset, statsY, statsWidth, statsHeight), p2.Unit_Stats[i,1], statsStyle);
			GUI.Label (new Rect(p2ShipNameX + classNameXOffset + hullStrengthXOffset, statsY, statsWidth, statsHeight), p2.Unit_Stats[i,2], statsStyle);
			GUI.Label (new Rect(p2ShipNameX + classNameXOffset + hullStrengthXOffset + killsXOffset, statsY, statsWidth, statsHeight), p2.Unit_Stats[i,3], statsStyle);
			statsY += statsYOffset;
		}

	}



}
