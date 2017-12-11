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
	int[] numActive;
	List<int>[] frameInput;

	int numseq = 3;

	// Use this for initialization
	void Start () {
		seqtext.text = "";
		frames = new GameObject[numseq][];
		frameInput = new List<int>[numseq];

		numActive = new int[numseq];

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
			numActive[j] = 0;
			frameInput [j].Insert (0, -1);
		}
	}
	
	// Update is called once per frame
	void Update () {

		for (int k = 0; k < numseq; k++) {
			for (int j = 0; j < frameInput[k].Count; j++) {
				if (j < numActive[k]) {
					frames [k] [frameInput [k][j]].SetActive (true);
					frames [k] [frameInput [k] [j]].GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f - (j * 0.4f));
				} else {
					if (frameInput [k] [j] != -1) {
						frames [k] [frameInput [k] [j]].SetActive (false);
					}
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			SceneManager.LoadScene ("multiple");
		}

		if (Input.GetMouseButton (0)) {
			Vector3 mouseP = Input.mousePosition;
			mousedot.rectTransform.position = mouseP;
			for (int i = 0; i < knobs.Length; i++) {
				float distance = Vector3.Distance (mouseP, knobs [i].position);
				if (distance < 0.5f*knobs [i].rect.width*canvas.scaleFactor) {
					float offset = 0;
					float sign = 1f;
					if (mouseP.x < knobs [i].position.x) {
						offset = 360;
						sign = -1f;
					}
					Vector3 up = knobs[i].position - new Vector3(knobs[i].position.x,knobs[i].position.y+distance,knobs[i].position.z);
					Vector3 mouse = knobs[i].position-mouseP;
					float angle = Vector3.Angle (up, mouse)*sign+offset;

					knobs [i].rotation = Quaternion.Euler (new Vector3 (0f,0f,-angle));

					int step = (int)(angle / 360f * frames[i].Length);
					Debug.Log (step);

					if (numActive [i] < 3 && frameInput[i][0] != step) {
						Debug.Log (numActive [i]);
						numActive [i] = (int)Mathf.Min (numActive [i] + 1, 3);
						if (frameInput [i] [0] == -1) {
							frameInput [i].RemoveAt (0);
						}
					}

					frameInput[i].Remove (step);
					frameInput[i].Insert (0, step);
				}
			}
		}
	}
}
