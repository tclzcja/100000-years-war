using UnityEngine;
using System.Collections;

public class Scene_1 : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		StartCoroutine ("CO");
	}

	IEnumerator CO ()
	{
		for (float i=0.0F; i<1.0F; i+=0.05F) {
			GameObject.Find ("Text_1").guiText.color = new Color (255, 255, 255, i);
			GameObject.Find ("Text_2").guiText.color = new Color (0, 0, 0, i);
			yield return new WaitForSeconds (0.05F);
		}

		iTween.MoveTo (GameObject.Find ("Chevalier"), Vector3.zero, 2.0F);
		yield return new WaitForSeconds (2.0F);

		for (float i=0.0F; i<1.0F; i+=0.05F) {
			GameObject.Find ("Text_3").guiText.color = new Color (255, 0, 0, i);
			GameObject.Find ("Text_4").guiText.color = new Color (0, 0, 0, i);
			yield return new WaitForSeconds (0.05F);
		}


	}

	IEnumerator EO ()
	{
		iTween.ScaleTo (GameObject.Find ("Plane"), Vector3.zero, 2.0F);
		iTween.FadeTo (GameObject.Find ("Text_1"), 0.0F, 2.0F);
		iTween.FadeTo (GameObject.Find ("Text_2"), 0.0F, 2.0F);
		iTween.FadeTo (GameObject.Find ("Text_3"), 0.0F, 2.0F);
		iTween.FadeTo (GameObject.Find ("Text_4"), 0.0F, 2.0F);
		iTween.MoveTo (GameObject.Find ("Chevalier"), new Vector3 (10, 1, 10), 2.0F);
		yield return new WaitForSeconds (2F);

		Application.LoadLevel ("2");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.anyKeyDown) {
			StartCoroutine ("EO");
		}
	}
}
