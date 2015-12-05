using UnityEngine;
using System.Collections;

namespace Josh
{
	public class Unit_Yeoman : Unit
	{
		public void Awake ()
		{
			this.MaxHP = 1000;
			this.MinPower = 500;
			this.MaxPower = 600;
			this.HighArmor = 100;
			this.MidArmor = 50;
			this.LowArmor = 0;
			this.MaxEnergy = 8;
			this.RotationCost = 1;
			this.Range = 7;
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