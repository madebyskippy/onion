using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class multimanager : MonoBehaviour {

	[SerializeField] Text seqtext;
	[SerializeField] Image mousedot;
	[SerializeField] RectTransform[] knobs;
	[SerializeField] Canvas canvas;

	GameObject[][] frames;
	int numActive;
	List<int>[] frameInput;

	int numseq = 3;

	// Use this for initialization
	void Start () {
		seqtext.text = "";
		numActive = 3;
		frames = new GameObject[numseq][];
		frameInput = new List<int>[numseq];

		for (int j = 0; j < numseq; j++) {
			Object[] s = Resources.LoadAll ("seq"+(j+1).ToString(), typeof(Sprite));
			frames[j] = new GameObject[s.Length];
			frameInput [j] = new List<int> ();
			for (int i = 0; i < s.Length; i++) {
				frames [j][i] = new GameObject ();
				SpriteRenderer sr = frames [j][i].AddComponent<SpriteRenderer> ();
				sr.sprite = s [i] as Sprite;
				frameInput[j].Add (i);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		for (int k = 0; k < numseq; k++) {
			for (int j = 0; j < frameInput[k].Count; j++) {
				if (j < numActive) {
					frames [k] [frameInput [k][j]].SetActive (true);
				} else {
					frames [k] [frameInput [k][j]].SetActive (false);
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			SceneManager.LoadScene ("prototype");
		}

		if (Input.GetMouseButton (0)) {
			Vector3 mouseP = Input.mousePosition;
			mousedot.rectTransform.position = mouseP;
			for (int i = 0; i < knobs.Length; i++) {
				float distance = Vector3.Distance (mouseP, knobs [i].position);
				if (distance < 0.5f*knobs [i].rect.width*canvas.scaleFactor) {
					//angle between that and vector pointing y+ (that's my angle 0)
					//need sign, whether it's to right or left of it ...
					float offset = 0;
					float sign = 1f;
					if (mouseP.x < knobs [i].position.x) {
						offset = 360;
						sign = -1f;
					}
					Vector3 up = knobs[i].position - new Vector3(knobs[i].position.x,knobs[i].position.y+distance,knobs[i].position.z);
					Vector3 mouse = knobs[i].position-mouseP;
					float angle = Vector3.Angle (up, mouse)*sign+offset;

					int step = (int)(angle / 360f * frames[i].Length);
					Debug.Log (step);

					frameInput[i].Remove (step);
					frameInput[i].Insert (0, step);
				}
			}
		}
	}
}
