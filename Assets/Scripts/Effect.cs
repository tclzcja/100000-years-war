using UnityEngine;
using System.Collections;

namespace Josh
{
	public class Effect : MonoBehaviour
	{
		private static GameObject Ref_PS_Explosive { get; set; }

		private static GameObject Ref_PS_Laser { get; set; }

		private static GameObject Ref_PS_Damage { get; set; }

		public void Awake ()
		{
			if (Ref_PS_Explosive == null) {
				Ref_PS_Explosive = Resources.Load ("Prefabs/PS_Explosive") as GameObject;
			}

			if (Ref_PS_Damage == null) {
				Ref_PS_Damage = Resources.Load ("Prefabs/PS_Damage") as GameObject;
			}

			if (Ref_PS_Laser == null) {
				Ref_PS_Laser = Resources.Load ("Prefabs/PS_Laser") as GameObject;
			}
		}

		public void Self_Rotation ()
		{
			this.gameObject.rigidbody.AddRelativeTorque (new Vector3 (100, 100, 0));
		}

		public void Shoot (GameObject _Target)
		{
			StartCoroutine ("_S", _Target);
		}

		private IEnumerator _S (GameObject _Target)
		{
			Vector3 R = this.gameObject.transform.rotation.eulerAngles;

			iTween.LookTo (this.gameObject, _Target.transform.position, 0.3F);

			yield return new WaitForSeconds (0.3F);

			GameObject GO = Instantiate (Ref_PS_Laser, this.gameObject.transform.position, this.transform.rotation) as GameObject;
			GameObject DO = Instantiate (Ref_PS_Damage, _Target.transform.position, Quaternion.identity) as GameObject;
	
			Destroy (DO, 2.0F);
			Destroy (GO, 0.5F);

			yield return new WaitForSeconds (0.2F);

			iTween.RotateTo (this.gameObject, R, 0.3F);
		}

		public void Explosive ()
		{
			GameObject GO = Instantiate (Ref_PS_Explosive, this.gameObject.transform.position, Quaternion.identity) as GameObject;
			GO.particleSystem.Play ();
			GameObject.Destroy (GO, 5.0F);
		}

		public void Floating ()
		{
			iTween.ShakePosition(this.gameObject, new Vector3(0.1F, 0.1F, 0.1F), 5.0F);
		}
	}
}