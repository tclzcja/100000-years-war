using UnityEngine;
using System.Collections;

namespace Josh
{
	public class Audio : MonoBehaviour
	{
		private AudioClip Ref_Confirm { get; set; }

		public AudioSource MusicSource;
		public AudioSource SfxSource;
		public AudioSource SfxSource2; //Used when we need to play 2 simultaneous sfx, like when you fire a laser and damage/destroy a ship

		// Use this for initialization
		void Start ()
		{
		
		}
		
		// Update is called once per frame
		void Update ()
		{
		
		}

		public void PlaySFX (AudioClip sfx)
		{
			SfxSource.clip = sfx;
			SfxSource.Play ();
		}

		public void PlaySFX2 (AudioClip sfx)
		{
			SfxSource2.clip = sfx;
			SfxSource2.Play ();
		}
	}
}