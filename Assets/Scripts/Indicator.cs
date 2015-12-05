using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Josh
{
	//TODO - LB and RB
	//TODO - Call UI
	//TODO - Planet
	public class Indicator : MonoBehaviour
	{
		private static Josh.Collections.Generic.List<GameObject> Players { get; set; }

		private static Josh.Collections.Generic.List<GameObject> Units { get; set; }

		private static Josh.Collections.Generic.List<GameObject> Enemies { get; set; }

		private static Josh.Collections.Generic.List<GameObject> Targets { get; set; }

		private static Josh.Collections.Generic.List<string> Modes { get; set; }

		private static Quaternion Direction { get; set; }

		private bool Locked { get; set; }

		private AudioClip Ref_Clip_Select_Chevalier;
		private AudioClip Ref_Clip_Select_Halberd;
		private AudioClip Ref_Clip_Select_Yeoman;
		private AudioClip Ref_Clip_Switch_Target;
		private AudioClip Ref_Clip_Enter_Move_Mode;
		private AudioClip Ref_Clip_Enter_Attack_Mode;
		private AudioClip Ref_Clip_Invalid;
		private AudioClip Ref_Clip_Turn_Start;
		private AudioClip Ref_Clip_Rotate_Ship;
		private AudioClip Ref_Clip_Move_Ship;
		private AudioClip Ref_Clip_Pass;

		public void Awake ()
		{
			Ref_Clip_Select_Chevalier = Resources.Load ("Sounds/select_chevalier") as AudioClip;
			Ref_Clip_Select_Halberd = Resources.Load ("Sounds/select_halberd") as AudioClip;
			Ref_Clip_Select_Yeoman = Resources.Load ("Sounds/select_yeoman") as AudioClip;
			Ref_Clip_Switch_Target = Resources.Load ("Sounds/switch_target") as AudioClip;
			Ref_Clip_Enter_Move_Mode = Resources.Load ("Sounds/enter_move_mode") as AudioClip;
			Ref_Clip_Enter_Attack_Mode = Resources.Load ("Sounds/enter_attack_mode") as AudioClip;
			Ref_Clip_Invalid = Resources.Load ("Sounds/invalid") as AudioClip;
			Ref_Clip_Turn_Start = Resources.Load ("Sounds/turn_start") as AudioClip;
			Ref_Clip_Rotate_Ship = Resources.Load ("Sounds/rotate") as AudioClip;
			Ref_Clip_Move_Ship = Resources.Load ("Sounds/move") as AudioClip;
			Ref_Clip_Pass = Resources.Load ("Sounds/pass") as AudioClip;
		}

		public void Start ()
		{
			Direction = new Quaternion (0, 0, 0, 0);
			Modes = new Josh.Collections.Generic.List<string> () {"Move", "Attack"};
			Players = new Josh.Collections.Generic.List<GameObject> (GameObject.FindGameObjectsWithTag ("Player"));
			Units = new Josh.Collections.Generic.List<GameObject> (GameObject.FindGameObjectsWithTag (Players.Current.name));

			Enemies = new Josh.Collections.Generic.List<GameObject> ();
			foreach (GameObject PO in Players) {
				if (PO.name != Players.Current.name) {
					Enemies.AddRange (GameObject.FindGameObjectsWithTag (PO.name));
				}
			}
			Enemies.Add (GameObject.Find ("Planet"));

			Targets = new Josh.Collections.Generic.List<GameObject> ();
			Units.Current.SendMessage ("GetEnemiesInRange", SendMessageOptions.DontRequireReceiver);

			Locked = false;

			iTween.MoveTo (this.gameObject, Units.Current.transform.position, 1.0F);
			iTween.FadeTo (GameObject.Find ("Indicator_Attack"), 0F, 0.0F);

			Call_UI ();
		}

		public void Update ()
		{
			if (Input.GetAxis ("X-Axis") != 0 || Input.GetAxis ("Y-Axis") != 0 || Input.GetAxis ("Z-Axis") != 0) {
				Input_Axises ();
			}
			else {
				Input_0 ();
			}

			if (!Locked) {
				if (Input.GetButtonDown ("X")) {
					Input_X ();
				}
				if (Input.GetButtonDown ("A")) {
					Input_A ();
				}
				if (Input.GetButtonDown ("B")) {
					Input_B ();
				}
				if (Input.GetButtonDown ("LB")) {
					Input_LB ();
				}
				if (Input.GetButtonDown ("RB")) {
					Input_RB ();
				}
			}
			Call_UI ();
		}

		private void Lock ()
		{
			this.Locked = true;
		}

		private void Unlock ()
		{
			this.Locked = false;
		}

		private void Input_0 ()
		{
			Direction = Quaternion.identity;
			switch (Modes.Current) {
				case "Move":
				{
					if (Units.Count > 0) {
						iTween.RotateTo (this.gameObject, (Units.Current.transform.rotation * Direction).eulerAngles, 1.0F);
					}
					break;
				}
				case "Attack":
				{
					break;
				}
			}

		}

		private void Input_Axises ()
		{
			if (Mathf.Abs (Input.GetAxis ("X-Axis")) > Mathf.Abs (Input.GetAxis ("Y-Axis"))) {
				if (Input.GetAxis ("X-Axis") > 0) {
					Direction = Quaternion.Euler (Vector3.up * 90);
				}
				else {
					Direction = Quaternion.Euler (Vector3.down * 90);
				}
			}
			else {
				if (Input.GetAxis ("Y-Axis") > 0) {
					Direction = Quaternion.Euler (Vector3.left * 90);
				}
				else {
					Direction = Quaternion.Euler (Vector3.right * 90);
				}
			}

			if (Input.GetAxis ("Z-Axis") != 0) {
				if (Input.GetAxis ("Z-Axis") < 0) {
					Direction = Quaternion.Euler (Vector3.forward * 90);
				}
				else {
					Direction = Quaternion.Euler (Vector3.back * 90);
				}
			}

			switch (Modes.Current) {
				case "Move":
				{
					iTween.RotateTo (this.gameObject, (Units.Current.transform.rotation * Direction).eulerAngles, 1.0F);
					break;
				}
				case "Attack":
				{
					break;
				}
			}

		}

		private void Input_LB ()
		{
			switch (Modes.Current) {

				case "Move":
				{
					Units.Prev ();
					Units.Current.SendMessage ("GetEnemiesInRange", SendMessageOptions.DontRequireReceiver);
					iTween.MoveTo (this.gameObject, Units.Current.transform.position, 1.0F);
					Josh.Unit unit = Units.Current.GetComponent ("Unit") as Josh.Unit;
					if (unit is Josh.Unit_Chevalier) {
						GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Select_Chevalier, SendMessageOptions.DontRequireReceiver);
					}
					else if (unit is Josh.Unit_Halberd) {
						GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Select_Halberd, SendMessageOptions.DontRequireReceiver);
					}
					else if (unit is Josh.Unit_Yeoman) {
						GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Select_Yeoman, SendMessageOptions.DontRequireReceiver);
					}
					break;
				}
				case "Attack":
				{
					Enemies.Prev ();
					
					LineRenderer LRO = GameObject.Find ("Line").GetComponent ("LineRenderer") as LineRenderer;
					
					if (Targets.Contains (Enemies.Current)) {
						LRO.SetColors (new Color (255F, 0F, 0F, 128F), new Color (255F, 0F, 0F, 128F));
						
						LRO.SetVertexCount (2);
						LRO.SetPosition (0, Units.Current.transform.position);
						LRO.SetPosition (1, Enemies.Current.transform.position);
					}
					else {
						LRO.SetColors (new Color (255F, 255F, 255F, 255F), new Color (255F, 255F, 255F, 255F));
						
						float x = Enemies.Current.transform.position.x;
						float y = Enemies.Current.transform.position.y;
						float z = Enemies.Current.transform.position.z;
						
						LRO.SetVertexCount (4);
						LRO.SetPosition (0, Units.Current.transform.position);
						LRO.SetPosition (1, new Vector3 (x, Units.Current.transform.position.y, Units.Current.transform.position.z));
						LRO.SetPosition (2, new Vector3 (x, y, Units.Current.transform.position.z));
						LRO.SetPosition (3, new Vector3 (x, y, z));
					}

					GameObject.Find ("Camera").SendMessage ("Radar_In", Units.Current, SendMessageOptions.DontRequireReceiver);
					iTween.MoveTo (this.gameObject, Enemies.Current.transform.position, 1.0F);
					GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Switch_Target, SendMessageOptions.DontRequireReceiver);
					break;
				}
			}
		}

		private void Input_RB ()
		{
			switch (Modes.Current) {
				
				case "Move":
				{
					Units.Next ();
					Units.Current.SendMessage ("GetEnemiesInRange", SendMessageOptions.DontRequireReceiver);
					iTween.MoveTo (this.gameObject, Units.Current.transform.position, 1.0F);
					Josh.Unit unit = Units.Current.GetComponent ("Unit") as Josh.Unit;
					if (unit is Josh.Unit_Chevalier) {
						GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Select_Chevalier, SendMessageOptions.DontRequireReceiver);
					}
					else if (unit is Josh.Unit_Halberd) {
						GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Select_Halberd, SendMessageOptions.DontRequireReceiver);
					}
					else if (unit is Josh.Unit_Yeoman) {
						GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Select_Yeoman, SendMessageOptions.DontRequireReceiver);
					}
					break;
				}
				case "Attack":
				{
					Enemies.Next ();

					LineRenderer LRO = GameObject.Find ("Line").GetComponent ("LineRenderer") as LineRenderer;
					
					if (Targets.Contains (Enemies.Current)) {
						LRO.SetColors (new Color (255F, 0F, 0F, 128F), new Color (255F, 0F, 0F, 128F));
						
						LRO.SetVertexCount (2);
						LRO.SetPosition (0, Units.Current.transform.position);
						LRO.SetPosition (1, Enemies.Current.transform.position);
					}
					else {
						LRO.SetColors (new Color (255F, 255F, 255F, 255F), new Color (255F, 255F, 255F, 255F));
						
						float x = Enemies.Current.transform.position.x;
						float y = Enemies.Current.transform.position.y;
						float z = Enemies.Current.transform.position.z;
						
						LRO.SetVertexCount (4);
						LRO.SetPosition (0, Units.Current.transform.position);
						LRO.SetPosition (1, new Vector3 (x, Units.Current.transform.position.y, Units.Current.transform.position.z));
						LRO.SetPosition (2, new Vector3 (x, y, Units.Current.transform.position.z));
						LRO.SetPosition (3, new Vector3 (x, y, z));
					}

					GameObject.Find ("Camera").SendMessage ("Radar_In", Units.Current, SendMessageOptions.DontRequireReceiver);
					iTween.MoveTo (this.gameObject, Enemies.Current.transform.position, 1.0F);
					GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Switch_Target, SendMessageOptions.DontRequireReceiver);
					break;
				}
			}
		}

		private void Input_X ()
		{
			Modes.Next ();

			GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Enter_Move_Mode, SendMessageOptions.DontRequireReceiver);

			switch (Modes.Current) {
				case "Move":
				{
					GameObject.Find ("Camera").SendMessage ("Radar_Out", Units.Current, SendMessageOptions.DontRequireReceiver);
					iTween.MoveTo (this.gameObject, Units.Current.transform.position, 1.0F);
					iTween.FadeTo (GameObject.Find ("Indicator_Move"), 0.5F, 0.2F);
					iTween.FadeTo (GameObject.Find ("Indicator_Attack"), 0.0F, 0.2F);
					LineRenderer LRO = GameObject.Find ("Line").GetComponent ("LineRenderer") as LineRenderer;
					LRO.SetVertexCount (0);
					break;
				}
				case "Attack":
				{
					Units.Current.SendMessage ("GetEnemiesInRange", SendMessageOptions.DontRequireReceiver);
					Debug.Log (Targets.Count);
					iTween.MoveTo (this.gameObject, Enemies.Current.transform.position, 1.0F);
					GameObject.Find ("Camera").SendMessage ("Radar_In", Units.Current, SendMessageOptions.DontRequireReceiver);
					iTween.FadeTo (GameObject.Find ("Indicator_Move"), 0.0F, 0.2F);
					iTween.FadeTo (GameObject.Find ("Indicator_Attack"), 0.5F, 0.2F);
					GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Enter_Attack_Mode, SendMessageOptions.DontRequireReceiver);

					LineRenderer LRO = GameObject.Find ("Line").GetComponent ("LineRenderer") as LineRenderer;

					if (Targets.Contains (Enemies.Current)) {
						LRO.SetColors (new Color (255F, 0F, 0F, 128F), new Color (255F, 0F, 0F, 128F));

						LRO.SetVertexCount (2);
						LRO.SetPosition (0, Units.Current.transform.position);
						LRO.SetPosition (1, Enemies.Current.transform.position);
					}
					else {
						LRO.SetColors (new Color (255F, 255F, 255F, 255F), new Color (255F, 255F, 255F, 255F));

						float x = Enemies.Current.transform.position.x;
						float y = Enemies.Current.transform.position.y;
						float z = Enemies.Current.transform.position.z;
						
						LRO.SetVertexCount (4);
						LRO.SetPosition (0, Units.Current.transform.position);
						LRO.SetPosition (1, new Vector3 (x, Units.Current.transform.position.y, Units.Current.transform.position.z));
						LRO.SetPosition (2, new Vector3 (x, y, Units.Current.transform.position.z));
						LRO.SetPosition (3, new Vector3 (x, y, z));
					}

					/*
					float x = Units.Current.transform.position.x;
					float y = Units.Current.transform.position.y;
					float z = Units.Current.transform.position.z;

					while (x != Enemies.Current.transform.position.x || y != Enemies.Current.transform.position.y || z != Enemies.Current.transform.position.z) {

						Instantiate (Ref_Indicator_Cube, new Vector3 (x, y, z), Quaternion.identity);
						if (x < Enemies.Current.transform.position.x) {
							x++;
							continue;
						}
						if (x > Enemies.Current.transform.position.x) {
							x--;
							continue;
						}
						if (y < Enemies.Current.transform.position.y) {
							y++;
							continue;
						}
						if (y > Enemies.Current.transform.position.y) {
							y--;
							continue;
						}
						if (z < Enemies.Current.transform.position.z) {
							z++;
							continue;
						}
						if (z > Enemies.Current.transform.position.z) {
							z--;
							continue;
						}
					}
*/
					break;
				}
			}

			Lock ();
			Invoke ("Unlock", 1.0F);
		}

		private void Input_B ()
		{
			Units.Current.SendMessage ("Pass", SendMessageOptions.DontRequireReceiver);
			GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Pass, SendMessageOptions.DontRequireReceiver);
			LineRenderer LRO = GameObject.Find ("Line").GetComponent ("LineRenderer") as LineRenderer;
			LRO.SetVertexCount (0);
			Units.Kick ();
			if (Units.Count > 0) {
				Input_RB ();
			}

			Input_Status_Change ();
		}

		private void Input_Status_Change ()
		{
			bool moveflag = false;

			if (Targets.Count == 0) {
				if (Modes.Current == "Attack") {
					Modes.Next ();
					foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Indicator_Cube")) {
						GameObject.Destroy (GO);
					}
					GameObject.Find ("Camera").SendMessage ("Radar_Out", Units.Current, SendMessageOptions.DontRequireReceiver);
					iTween.FadeTo (GameObject.Find ("Indicator_Move"), 0.5F, 0.2F);
					iTween.FadeTo (GameObject.Find ("Indicator_Attack"), 0.0F, 0.2F);
					LineRenderer LRO = GameObject.Find ("Line").GetComponent ("LineRenderer") as LineRenderer;
					LRO.SetVertexCount (0);
				}
			}
			
			if (Units.Count != 0 && Units.Current.tag != Players.Current.name) {
				Debug.Log ("Jump");
				if (Modes.Current == "Attack") {
					Modes.Next ();
					GameObject.Find ("Camera").SendMessage ("Radar_Out", Units.Current, SendMessageOptions.DontRequireReceiver);
					iTween.FadeTo (GameObject.Find ("Indicator_Move"), 0.5F, 0.2F);
					iTween.FadeTo (GameObject.Find ("Indicator_Attack"), 0.0F, 0.2F);
					LineRenderer LRO = GameObject.Find ("Line").GetComponent ("LineRenderer") as LineRenderer;
					LRO.SetVertexCount (0);
					Units.Kick ();
				}
				else {
					if (Units.Count != 0) {
						Units.Kick ();
						Units.Next ();
					}
				}
				moveflag = true;
			}

			if (Units.Count == 0) {
				Debug.Log ("TURN");
				Players.Kick ();

				if (Players.Count != 0) {
					Units = new Josh.Collections.Generic.List<GameObject> (GameObject.FindGameObjectsWithTag (Players.Current.name));
					Targets = new Josh.Collections.Generic.List<GameObject> ();
					Enemies = new Josh.Collections.Generic.List<GameObject> ();
					foreach (GameObject PO in GameObject.FindGameObjectsWithTag("Player")) {
						if (PO.name != Players.Current.name) {
							Enemies.AddRange (GameObject.FindGameObjectsWithTag (PO.name));
							Enemies.AddRange (GameObject.FindGameObjectsWithTag (PO.name + "_Runout"));
						}
					}

					Debug.Log ("ENE: " + Enemies.Count.ToString ());
					/** THE FOLLOWING LINE MAY BE IN THE WRONG PLACE */
					GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Turn_Start, SendMessageOptions.DontRequireReceiver);
					moveflag = true;
					if (Modes.Current == "Attack") {
						Modes.Next ();
						foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Indicator_Cube")) {
							GameObject.Destroy (GO);
						}
						GameObject.Find ("Camera").SendMessage ("Radar_Out", Units.Current, SendMessageOptions.DontRequireReceiver);
						iTween.FadeTo (GameObject.Find ("Indicator_Move"), 0.5F, 0.2F);
						iTween.FadeTo (GameObject.Find ("Indicator_Attack"), 0.0F, 0.2F);
					}
				}
				else {
					Debug.Log ("TTT");
					foreach (GameObject PO in GameObject.FindGameObjectsWithTag("Player")) {
						Players.Add (PO);
						PO.SendMessage ("Refresh", SendMessageOptions.DontRequireReceiver);
					}
					
					Units = new Josh.Collections.Generic.List<GameObject> (GameObject.FindGameObjectsWithTag (Players.Current.name));
					Enemies = new Josh.Collections.Generic.List<GameObject> ();
					foreach (GameObject PO in Players) {
						if (PO.name != Players.Current.name) {
							Enemies.AddRange (GameObject.FindGameObjectsWithTag (PO.name));
						}
					}
					Enemies.Add (GameObject.Find ("Planet"));

					Targets = new Josh.Collections.Generic.List<GameObject> ();
					moveflag = true;
					if (Modes.Current == "Attack") {
						Modes.Next ();
						foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Indicator_Cube")) {
							GameObject.Destroy (GO);
						}
						GameObject.Find ("Camera").SendMessage ("Radar_Out", Units.Current, SendMessageOptions.DontRequireReceiver);
						iTween.FadeTo (GameObject.Find ("Indicator_Move"), 0.5F, 0.2F);
						iTween.FadeTo (GameObject.Find ("Indicator_Attack"), 0.0F, 0.2F);
					}
				}

				LineRenderer LRO = GameObject.Find ("Line").GetComponent ("LineRenderer") as LineRenderer;
				LRO.SetVertexCount (0);
			}

			if (moveflag) {
				Debug.Log (this.gameObject.transform.position);
				Debug.Log (Units.Current.transform.position);
				iTween.MoveTo (this.gameObject, Units.Current.transform.position, 1.0F);
			}
		}
        
		private void Input_A ()
		{
			if (Locked) {
				Debug.Log ("Locked");
			}
			else {
				Lock ();

				switch (Modes.Current) {
					case "Move":
					{
						if (Direction != Quaternion.identity) {
							Units.Current.SendMessage ("Move", Direction, SendMessageOptions.DontRequireReceiver);
							GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Rotate_Ship, SendMessageOptions.DontRequireReceiver);
						}

						Vector3 T = this.transform.position + this.transform.forward;

						if (Direction == Quaternion.identity) {
							if (!Physics.Raycast (this.transform.position, this.transform.forward, 1.0F) && T.x < 11F && T.x > -11F && T.y < 11F && T.y > -11F && T.z < 11F && T.z > -11F) {

								iTween.MoveTo (this.gameObject, Units.Current.transform.position + Units.Current.transform.forward, 1.0F);

								Units.Current.SendMessage ("Move", Direction, SendMessageOptions.DontRequireReceiver);

								GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Move_Ship, SendMessageOptions.DontRequireReceiver);
							}
							else {
								GameObject.Find ("Camera").SendMessage ("PlaySFX", Ref_Clip_Invalid, SendMessageOptions.DontRequireReceiver);
							}
						}
						Input_Status_Change ();

						break;
					}
					case "Attack":
					{
						if (Targets.Contains (Enemies.Current)) {
							Units.Current.SendMessage ("Shoot", Enemies.Current, SendMessageOptions.DontRequireReceiver);
							Units.Current.SendMessage ("Attack", Enemies.Current, SendMessageOptions.DontRequireReceiver);
							Input_Status_Change ();
						}
						break;
					}
				}

				Invoke ("Unlock", 1.0F);
			}

			//If All the Targets are destroyed, switch back to Move mode

		}

		private void Call_UI ()
		{
			switch (Modes.Current) {
				case "Move":
				{
					GameObject.Find ("UI").BroadcastMessage ("UpdateCurrentUnit", Units.Current, SendMessageOptions.DontRequireReceiver);
					GameObject.Find ("UI").BroadcastMessage ("ClearEnemyUnit", SendMessageOptions.DontRequireReceiver);
					break;
				}
				case "Attack":
				{

					GameObject.Find ("UI").BroadcastMessage ("UpdateEnemyUnit", Enemies.Current, SendMessageOptions.DontRequireReceiver);

					break;
				}
			}

			string[] infoPackage = new string[5];
			infoPackage [0] = Players.Current.name;
			infoPackage [1] = Units.Count + "";
			Player p = Players.Current.GetComponent ("Player") as Player;
			infoPackage [2] = p.Unit_Number + ""; 
			infoPackage [3] = (GameObject.Find ("Player_1").GetComponent ("Player") as Josh.Player).Turn_Number + ""; //Round
			Unit_Planet planet = GameObject.Find ("Planet").GetComponent ("Unit_Planet") as Unit_Planet;
			infoPackage [4] = planet.CurHP + "/" + planet.MaxHP;
			
			GameObject.Find ("UI").SendMessage ("UpdateGameInfo", infoPackage, SendMessageOptions.DontRequireReceiver);
		}

		public void Target_Instantiate ()
		{
			Targets = new Josh.Collections.Generic.List<GameObject> ();
		}

		public void Target_Add (List<GameObject> _GLO)
		{
			Debug.Log (_GLO.Count);
			Targets = new Josh.Collections.Generic.List<GameObject> (_GLO);
		}

		public void Target_Kick ()
		{
			Targets.Kick ();
			Input_RB ();
		}
	}
}