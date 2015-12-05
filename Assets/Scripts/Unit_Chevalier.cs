using UnityEngine;
using System.Collections;

namespace Josh
{
	public class Unit_Chevalier : Unit
	{
		public void Awake ()
		{
			this.MaxHP = 2500;
			this.MinPower = 1000;
			this.MaxPower = 1100;
			this.HighArmor = 300;
			this.MidArmor = 150;
			this.LowArmor = 50;
			this.MaxEnergy = 10;
			this.RotationCost = 2;
			this.Range = 3;
			this.FiringCost = 3;

			this.CurHP = MaxHP;
			this.Energy = MaxEnergy;
		}

		// Update is called once per frame
		public void Update ()
		{
	
		}

	}
}