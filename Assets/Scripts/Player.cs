using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Josh
{
	public class Player : MonoBehaviour
	{
		public int Unit_Number{ get; set; }
	
		public int Turn_Number{ get; set; }

		public int Max_Units { get; set; }
		public string[,] Unit_Stats;
		//[i,0]: ship name
		//[i,1]: ship class
		//[i,2]: hull strength ("cur/max")
		//[i,3]: kills

		public void Awake ()
		{
			DontDestroyOnLoad(transform.gameObject);

			this.Unit_Number = GameObject.FindGameObjectsWithTag (this.name).Length;

			this.Turn_Number = 1;

			this.Max_Units = Unit_Number;
			this.Unit_Stats = new string[20,4];

		}

		public void Start ()
		{
			for (int i = 0; i < GameObject.FindGameObjectsWithTag(this.name).Length; i++) {
				//This is basically the same as Get_Stats() in Unit, but whatever
				Josh.Unit u = GameObject.FindGameObjectsWithTag(this.name)[i].GetComponent ("Unit") as Josh.Unit;
				
				this.Unit_Stats[i,0] = u.name;

				if ((u.GetComponent ("Unit") as Josh.Unit) is Josh.Unit_Chevalier) {
					this.Unit_Stats[i,1] = "Chevalier";
				}
				else if ((u.GetComponent ("Unit") as Josh.Unit) is Josh.Unit_Halberd) {
					this.Unit_Stats[i,1] = "Halberd";
				}
				else if ((u.GetComponent ("Unit") as Josh.Unit) is Josh.Unit_Yeoman) {
					this.Unit_Stats[i,1] = "Yeoman";
				}
				else {
					Debug.LogWarning ("Error while trying to add ship class to stats");
				}
				
				this.Unit_Stats[i,2] = u.CurHP + "/" + u.MaxHP;
				this.Unit_Stats[i,3] = "0";	//Kills
			}
		}

		private void Done()
		{
			if (this.name == "Player_1")
			{
				GameObject.Find("Text_1").guiText.text = "Anglian Federation wins!";
				GameObject.Find("Text_2").guiText.text = "Anglian Federation wins!";
			}
			else
			{
				GameObject.Find("Text_1").guiText.text = "Nouvelle Entente wins!";
				GameObject.Find("Text_2").guiText.text = "Nouvelle Entente wins!";
			}
			StartCoroutine ("_D");
		}

		private IEnumerator _D()
		{
			for (float i=0.0F; i<1.0F; i+=0.05F) {
				GameObject.Find ("Text_1").guiText.color = new Color (255, 255, 255, i);
				GameObject.Find ("Text_2").guiText.color = new Color (0, 0, 0, i);
				yield return new WaitForSeconds (0.05F);
			}

			yield return new WaitForSeconds(1.0F);

			
			Application.LoadLevel("3");
		}

		public void Diminish ()
		{
			this.Unit_Number -= 1;

			if (this.Unit_Number == 0)
			{
				Done ();
			}
		}

		public void Refresh ()
		{
			this.Turn_Number += 1;

			if (this.Turn_Number == 10)
			{
				Done();
			}

			foreach (GameObject UO in GameObject.FindGameObjectsWithTag(this.name + "_Runout"))
			{
				UO.BroadcastMessage ("Refresh", SendMessageOptions.DontRequireReceiver);
			}
		}

		public void Update_Unit_Stats (string[] stats)
		{
			for (int i = 0; i < Max_Units; i++) {
//				if (Unit_Stats[i,0].Equals (stats[0])) {
				if (Unit_Stats[i,0] != null){
					if(Unit_Stats[i,0].Equals (stats[0])) {
					//Ugh whatever
						Unit_Stats[i,0] = stats[0];
						Unit_Stats[i,1] = stats[1];
						Unit_Stats[i,2] = stats[2];
						Unit_Stats[i,3] = stats[3];
						Debug.Log (Unit_Stats[i,3]);
					}
				}
			}
		}
	}
}