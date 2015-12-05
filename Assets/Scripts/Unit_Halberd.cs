using UnityEngine;
using System.Collections;

namespace Josh
{
	public class Unit_Halberd : Unit
	{
		public void Awake ()
		{
			this.MaxHP = 2000;
			this.MinPower = 700;
			this.MaxPower = 800;
			this.HighArmor = 250;
			this.MidArmor = 200;
			this.LowArmor = 100;
			this.MaxEnergy = 6;
			this.RotationCost = 1;
			this.Range = 5;
			this.FiringCost = 2;

			this.CurHP = MaxHP;
			this.Energy = MaxEnergy;
		}

		// Update is called once per frame
		public void Update ()
		{
	
		}

	}
}