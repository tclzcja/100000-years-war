using UnityEngine;
using System.Collections;

namespace Josh
{
	public class Unit_Planet : Unit
	{
		// Use this for initialization
		public void Start ()
		{
			this.MaxHP = 100000;
			this.MinPower = 0;
			this.MaxPower = 0;
			this.HighArmor = 0;
			this.MidArmor = 0;
			this.LowArmor = 0;
			this.MaxEnergy = 0;
			this.RotationCost = 0;
			this.Range = 0;
			this.FiringCost = 0;
			
			this.CurHP = MaxHP;
			this.Energy = MaxEnergy;

			this.gameObject.SendMessage("Self_Rotation", SendMessageOptions.DontRequireReceiver);
		}
	
		public new void TakeDamage (int _Damage)
		{
			this.CurHP -= _Damage;

			if (this.CurHP <= 0)
			{
				//Loses
				Debug.Log(this.tag + " Loses");
			}
		}
	}
}