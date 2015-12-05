using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Josh
{
	public class Camera : MonoBehaviour
	{
		private Vector3 Position_Radar{ get; set; }

		private Josh.Collections.Generic.List<Vector3> Position_List { get; set; }

		private Josh.Collections.Generic.List<Vector3> Rotation_List { get; set; }

		private bool Radared { get; set; }

		private bool Locked { get; set; }

		private GameObject Ref_Indicator { get; set; }

		private GameObject Ref_Origin { get; set; }

		private AudioClip Ref_Clip_Confirm;
		private AudioClip Ref_Clip_Invalid;
		private AudioClip Ref_Clip_Turn_Start;
		private AudioClip Ref_Clip_Camera_Switch;

		private void Awake ()
		{
			this.Locked = false;
			this.Radared = false;

			this.Position_List = new Josh.Collections.Generic.List<Vector3> ();
			this.Rotation_List = new Josh.Collections.Generic.List<Vector3> ();

			Position_List.Add (new Vector3 (-3, 4.1F, -3));
			Rotation_List.Add (new Vector3 (45, 45, 0));

			Position_List.Add (new Vector3 (3, 4.1F, 3));
			Rotation_List.Add (new Vector3 (45, -135, 0));

			Position_List.Add (new Vector3 (0, 0, 0));
			Rotation_List.Add (new Vector3 (0, 0, 0));

			Ref_Clip_Confirm = Resources.Load ("Sounds/confirm") as AudioClip;
			Ref_Clip_Camera_Switch = Resources.Load ("Sounds/camera_switch") as AudioClip;
		}

		private void Start ()
		{
			Ref_Indicator = GameObject.Find ("Indicator");
		}

		private void Update ()
		{
			if (this.Radared) {
				float ox = Ref_Origin.transform.position.x;
				float oy = Ref_Origin.transform.position.y;
				float oz = Ref_Origin.transform.position.z;
				float tx = Ref_Indicator.transform.position.x;
				float ty = Ref_Indicator.transform.position.y;
				float tz = Ref_Indicator.transform.position.z;

				if (tx > ox && ty > oy && tz > oz) {
					iTween.MoveUpdate (this.gameObject, Ref_Origin.transform.position + new Vector3 (-1F, -1F, -1F), 1.0F);
				}

				if (tx > ox && ty > oy && tz < oz) {
					iTween.MoveUpdate (this.gameObject, Ref_Origin.transform.position + new Vector3 (-1F, -1F, 1F), 1.0F);
				}

				if (tx < ox && ty > oy && tz > oz) {
					iTween.MoveUpdate (this.gameObject, Ref_Origin.transform.position + new Vector3 (1F, -1F, -1F), 1.0F);
				}

				if (tx < ox && ty < oy && tz < oz) {
					iTween.MoveUpdate (this.gameObject, Ref_Origin.transform.position + new Vector3 (1F, -1F, 1F), 1.0F);
				}

				if (tx > ox && ty < oy && tz > oz) {
					iTween.MoveUpdate (this.gameObject, Ref_Origin.transform.position + new Vector3 (-1F, 1F, -1F), 1.0F);
				}
				
				if (tx > ox && ty < oy && tz < oz) {
					iTween.MoveUpdate (this.gameObject, Ref_Origin.transform.position + new Vector3 (-1F, 1F, 1F), 1.0F);
				}
				
				if (tx < ox && ty < oy && tz > oz) {
					iTween.MoveUpdate (this.gameObject, Ref_Origin.transform.position + new Vector3 (1F, 1F, -1F), 1.0F);
				}
				
				if (tx < ox && ty < oy && tz < oz) {
					iTween.MoveUpdate (this.gameObject, Ref_Origin.transform.position + new Vector3 (1F, 1F, 1F), 1.0F);
				}

				iTween.LookUpdate(this.gameObject, Ref_Indicator.transform.position, 1.0F);
			}
			else {
				if (this.gameObject.transform.position != Ref_Indicator.transform.position + Position_List.Current) {
					this.gameObject.transform.position = Ref_Indicator.transform.position + Position_List.Current;
				}
			}
		}

		private void Lock ()
		{
			this.Locked = true;
		}

		private void Unlock ()
		{
			this.Locked = false;
		}

		public void Radar_In (GameObject _Indicator)
		{
			this.Radared = true;
			this.Ref_Origin = _Indicator;
			//iTween.MoveTo (this.gameObject, _Indicator.transform.position + new Vector3 (-1F, 1F, -1F), 1.0F);
			//iTween.RotateTo (this.gameObject, new Vector3(0, 0, 0), 1.0F);
		}

		public void Radar_Out (GameObject _Indicator)
		{
			Invoke ("Radar_Unlock", 1.0F);
			iTween.RotateTo (this.gameObject, Rotation_List.Current, 1.0F);
			iTween.MoveTo (this.gameObject, _Indicator.transform.position + Position_List.Current, 1.0F);
		}

		public void Radar_Unlock ()
		{
			this.Radared = false;
		}
	}
}