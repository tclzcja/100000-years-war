using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Josh
{
	public abstract class Unit : MonoBehaviour
	{
		public int MaxHP { get; set; }

		public int MinPower { get; set; }

		public int MaxPower { get; set; }

		public int HighArmor { get; set; }

		public int MidArmor { get; set; }

		public int LowArmor { get; set; }

		public int MaxEnergy { get; set; }

		public int RotationCost { get; set; }

		public int Range { get; set; }

		public int FiringCost { get; set; }

		public int Energy { get; set; }

		public int CurHP { get; set; }

		public int Kills { get; set; }

		private static AudioClip Fire_Laser;
		private static AudioClip Ship_Damaged;
		private static AudioClip Ship_Destroyed;

		public void Awake ()
		{
			//Note - this is probably very inefficient since every ship instance is loading these sounds
			//No More a Problem~

			if (Fire_Laser == null) {
				Fire_Laser = Resources.Load ("Sounds/fire_laser") as AudioClip;
			}

			if (Ship_Damaged == null) {
				Ship_Damaged = Resources.Load ("Sounds/ship_damaged") as AudioClip;
			}

			if (Ship_Destroyed == null) {
				Ship_Destroyed = Resources.Load ("Sounds/ship_destroyed") as AudioClip;
			}

			Kills = 0;
		}

		//Call after taking any action (move, rotate, attack). Updates energy and checks to see if we've run out. 
		private void FinishAction ()
		{	
			if (this.Energy <= 0) {
				this.Energy = 0;
				if (!this.gameObject.tag.Contains ("_Runout")) {
					this.gameObject.tag = this.gameObject.tag + "_Runout";
				}
			}
		}

		//Because every block is 1*1*1 in the Scene, so basically we're using the Ship.transform.position to know which block the ship is right now.

		public void Attack (GameObject _Target)
		{
			if (this.Energy >= FiringCost) {
				int damage = Random.Range (this.MinPower, this.MaxPower) - GetExposedArmor (_Target);
				if (damage >= (_Target.GetComponent ("Unit") as Josh.Unit).CurHP) {
					Kills++;
					(GameObject.Find (this.tag).GetComponent ("Player") as Josh.Player).SendMessage ("Update_Unit_Stats", Get_Stats (), SendMessageOptions.DontRequireReceiver);
					Debug.Log ("Kills: " + Kills);
				}
				_Target.BroadcastMessage ("TakeDamage", damage);

				this.Energy -= FiringCost;

				Josh.Audio audio = GameObject.Find ("Camera").GetComponent ("Audio") as Josh.Audio;
				audio.PlaySFX (Fire_Laser);
					
				FinishAction ();
			}
			else {
				GameObject.Find ("Indicator").SendMessage ("PlayInvalidSound", SendMessageOptions.DontRequireReceiver);
			}
		}

		public string[] Get_Stats ()
		{
			string[] stats = new string[4];
			stats [0] = this.name;
			if (this is Josh.Unit_Chevalier) {
				stats [1] = "Chevalier";
			}
			else if (this is Josh.Unit_Halberd) {
				stats [1] = "Halberd";
			}
			else if (this is Josh.Unit_Yeoman) {
				stats [1] = "Yeoman";
			}
			else {
				Debug.LogWarning ("Error while trying to add ship class to stats");
			}
			
			stats [2] = CurHP + "/" + MaxHP;
			stats [3] = Kills + "";	
		
			return stats;
		}

		public void Pass ()
		{
			this.Energy = 0;
			FinishAction ();
		}

		public void TakeDamage (int damage)
		{
			CurHP -= damage;

			if (CurHP <= 0) {
				Die ();
			}
			else {

				GameObject.Find (this.tag.Replace ("_Runout", "")).SendMessage ("Update_Unit_Stats", Get_Stats (), SendMessageOptions.DontRequireReceiver);
				GameObject.Find ("Camera").SendMessage ("PlaySFX2", Ship_Damaged, SendMessageOptions.DontRequireReceiver);
			}
		}

		private void Die ()
		{
			CurHP = 0;
			GameObject.Find (this.tag.Replace ("_Runout", "")).SendMessage ("Update_Unit_Stats", Get_Stats (), SendMessageOptions.DontRequireReceiver);

			GameObject.Find ("Indicator").SendMessage ("Target_Kick", SendMessageOptions.DontRequireReceiver);
			GameObject.Find (this.tag.Replace ("_Runout", "")).SendMessage ("Diminish", SendMessageOptions.DontRequireReceiver);
			this.gameObject.SendMessage ("Explosive");

			GameObject.Find ("Camera").SendMessage ("PlaySFX2", Ship_Destroyed, SendMessageOptions.DontRequireReceiver);
			GameObject.Destroy (this.gameObject, 2.0F);
		}

		public void Refresh ()
		{
			this.Energy = this.MaxEnergy;
			this.tag = this.tag.Replace ("_Runout", "");
		}
			
		public void Move (Quaternion _RotateDirection)
		{
			Vector3 R = _RotateDirection.eulerAngles;

			if (_RotateDirection != Quaternion.identity) {
				if (this.Energy >= RotationCost) {
					this.Energy -= RotationCost;

					R.x = R.x > 180F ? R.x - 360F : R.x;
					R.y = R.y > 180F ? R.y - 360F : R.y;
					R.z = R.z > 180F ? R.z - 360F : R.z;
					iTween.RotateAdd (this.gameObject, new Vector3 (Mathf.RoundToInt (R.x), Mathf.RoundToInt (R.y), Mathf.RoundToInt (R.z)), 0.8F);
				}
				else {
					GameObject.Find ("Indicator").SendMessage ("PlayInvalidSound", SendMessageOptions.DontRequireReceiver);
				}
			}
			else {
				this.Energy -= 1;
				Vector3 P = this.gameObject.transform.position + this.transform.forward;
				iTween.MoveTo (this.gameObject, new Vector3 (Mathf.RoundToInt (P.x), Mathf.RoundToInt (P.y), Mathf.RoundToInt (P.z)), 0.8F);
			}

			FinishAction ();
		}

		public void Scan ()
		{
			string S = this.tag.Replace ("_Runout", "");
			if (S == "Player_1") {
				GameObject.Find ("Indicator").BroadcastMessage ("Target_Add", GameObject.FindGameObjectsWithTag ("Player_2"), SendMessageOptions.DontRequireReceiver);
				GameObject.Find ("Indicator").BroadcastMessage ("Target_Add", GameObject.FindGameObjectsWithTag ("Player_2_Runout"), SendMessageOptions.DontRequireReceiver);
			}
			else {
				GameObject.Find ("Indicator").BroadcastMessage ("Target_Add", GameObject.FindGameObjectsWithTag ("Player_1"), SendMessageOptions.DontRequireReceiver);
				GameObject.Find ("Indicator").BroadcastMessage ("Target_Add", GameObject.FindGameObjectsWithTag ("Player_1_Runout"), SendMessageOptions.DontRequireReceiver);
			}
		}

		/*
		//Tells the UI this ship's stats if this is the currently selected ship
		public void SetAsCurrent ()
		{
			GameObject ui = GameObject.FindWithTag ("GUI");
			Josh.UI a = ui.GetComponent ("UI") as Josh.UI; 
			a.UpdateCurrentShip (this);
		}

		//Tells the UI this ship's stats if this is the currently targeted enemy ship
		public void SetAsEnemy ()
		{
			GameObject ui = GameObject.FindWithTag ("GUI");
			Josh.UI a = ui.GetComponent ("UI") as Josh.UI; 
			a.UpdateEnemyUnit (this);
		}
*/


		public void GetEnemiesInRange ()
		{
			List<GameObject> EnemyUnits = new List<GameObject> ();
		
			string S = this.tag.Replace ("_Runout", "");
			if (S == "Player_1") {
				EnemyUnits.AddRange (GameObject.FindGameObjectsWithTag ("Player_2"));
				EnemyUnits.AddRange (GameObject.FindGameObjectsWithTag ("Player_2_Runout"));
			}
			else if (S == "Player_2") {
				EnemyUnits.AddRange (GameObject.FindGameObjectsWithTag ("Player_1"));
				EnemyUnits.AddRange (GameObject.FindGameObjectsWithTag ("Player_1_Runout"));
			}
			else {
				Debug.LogWarning ("Error - something messed up while finding this ship's player tag");
			}

			List<Vector3> SpacesInRange = GetRange ();
			List<GameObject> EnemiesInRange = new List<GameObject> ();

			/*
			foreach (Vector3 V in SpacesInRange)
			{
				Debug.Log("VVV"+V.ToString());
			}

			foreach (GameObject GO in EnemyUnits)
			{
				Debug.Log("EEE"+GO.transform.position.ToString());
			}*/

			foreach (GameObject GO in EnemyUnits) {
				if (Vector3.Distance (GO.transform.position, this.gameObject.transform.position) <= this.Range) {
					EnemiesInRange.Add(GO);
				}
			}

			//Planet in range?
			if (S == "Player_1") {
				GameObject planet = GameObject.Find ("Planet");
				if (planet.transform.position.z - 10 - transform.position.z <= Range) {
					EnemiesInRange.Add (planet);
				}
			}
		
			GameObject.Find ("Indicator").SendMessage ("Target_Add", EnemiesInRange, SendMessageOptions.DontRequireReceiver);
		}

		//Returns a list of all positions within range. 
		//Used to determine if an enemy is in range, could also be used for UI.
		public List<Vector3> GetRange ()
		{
			List<Vector3> R = new List<Vector3> ();
			for (int i = 1; i <= Range; i++) {
				Vector3 Center = transform.position + transform.forward * i;

				for (int j = i * -1; j <= i; j++) {
					for (int k = i * -1; k <= i; k++) {
						R.Add (Center + transform.up * j + transform.right * k);
					}
				}
			}

			return R;
		}
	
		//Returns the armor stat of the exposed side of the  enemy ship.
		public int GetExposedArmor (GameObject Enemy)
		{
			Josh.Unit EnemyUnit = Enemy.GetComponent ("Unit") as Josh.Unit;
			//Get a vector 2 spaces directly ahead of you
			Vector3 V = this.transform.position + this.transform.forward * 2;

			//Make a list of vectors 1 cube away from selected vector in all 6 directions from ship 
			List<Vector3> VList = new List<Vector3> ();
			VList.Add (V + EnemyUnit.transform.forward);			//Front
			VList.Add (V + EnemyUnit.transform.up);					//Top
			VList.Add (V + EnemyUnit.transform.right);				//Right side
			VList.Add (V + EnemyUnit.transform.right * -1);			//Left side
			VList.Add (V + EnemyUnit.transform.forward * -1);		//Back
			VList.Add (V + EnemyUnit.transform.up * -1);			//Bottom

			//Return armor based on which of these vectors is 1 cube in front of you
			Vector3 VTarget = this.transform.position + this.transform.forward;
			if (VTarget == (VList [0]) || VTarget == (VList [1])) {
				return EnemyUnit.HighArmor;
			}
			if (VTarget == (VList [2]) || VTarget == (VList [3])) {
				return EnemyUnit.MidArmor;
			}
			if (VTarget == (VList [4]) || VTarget == (VList [5])) {
				return EnemyUnit.LowArmor;
			}

			Debug.LogWarning ("Something messed up while figuring out exposed armor");
			return -1;
		}
	}
}