using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class manager : MonoBehaviour {

	[SerializeField] Text seqtext;

	GameObject[] frames;
	bool[] frameState;
	int numActive;
	List<int> frameActive = new List<int>();

	char[] keys = new char[10]{'Q','W','E','R','T','Y','U','I','O','P'};

	int[] sequence = new int[10]{0,1,2,3,4,5,6,7,8,9};

	// Use this for initialization
	void Start () {
		seqtext.text = "";
		numActive = 0;
		Object[] s = Resources.LoadAll ("getup", typeof(Sprite));
		frames = new GameObject[s.Length];
		frameState = new bool[s.Length];
		for (int i = 0; i < s.Length; i++) {
			frames [i] = new GameObject ();
			SpriteRenderer sr = frames [i].AddComponent<SpriteRenderer> ();
			sr.sprite = s [i] as Sprite;
			frameState [i] = false;
			frames [i].SetActive (frameState[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < keys.Length; i++) {
			if (Input.GetKeyDown(KeyCode.A+((int)keys[i])-65)){
				if (!frameState[i]) {
					frameState [i] = true;
					numActive++;
					frameActive.Add (i);
					seqtext.text += keys [i];
					if (numActive > 3) {
						frameState [frameActive [0]] = false;
						frameActive.RemoveAt (0);
						numActive--;
						seqtext.text = seqtext.text.Remove (0, 1);
					}
				}
			}
		}

		for (int j = 0; j < frames.Length; j++) {
			frames [j].SetActive (frameState[j]);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			SceneManager.LoadScene ("prototype");
		}
	}
}
