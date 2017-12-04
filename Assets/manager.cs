using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class manager : MonoBehaviour {

	[SerializeField] Text seqtext;
	[SerializeField] Animator anim;
	[SerializeField] Image mousedot;
	[SerializeField] RectTransform[] knobs;
	[SerializeField] Canvas canvas;

	GameObject[] frames;
	int numActive;
	List<int> frameInput = new List<int>();

	char[] keys = new char[10]{'Q','W','E','R','T','Y','U','I','O','P'}; //just used for getting input

	int[] sequence = new int[10]{0,1,2,3,4,5,6,7,8,9}; //used for getting keypress > frame shown
	string seqstring = "QWERTYUIOP"; //the actual answer to the sequence

	// Use this for initialization
	void Start () {
		seqtext.text = "";
		numActive = 3;
		Object[] s = Resources.LoadAll ("getup", typeof(Sprite));
		frames = new GameObject[s.Length];
		for (int i = 0; i < s.Length; i++) {
			frames [i] = new GameObject ();
			SpriteRenderer sr = frames [i].AddComponent<SpriteRenderer> ();
			sr.sprite = s [i] as Sprite;
			frameInput.Add (i);
		}
//		randomizeSequence ();

	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < keys.Length; i++) {
			if (Input.GetKeyDown(KeyCode.A+((int)keys[i])-65)){
				frameInput.Remove (i);
				frameInput.Insert (0, i);
			}
		}

		seqtext.text = ". . . ";
		string inp = "";
		for (int j = 0; j < frameInput.Count; j++) {
			if (j < numActive) {
				frames [sequence[frameInput[j]]].SetActive (true);
				seqtext.text = " "+ keys[frameInput[j]] + seqtext.text;
			} else {
				frames [sequence[frameInput[j]]].SetActive (false);
			}
			inp += keys[frameInput [j]];
		}
//		Debug.Log (inp);
		seqtext.text = ". . ." + seqtext.text;

		if (check ()) {
			Debug.Log ("win!!!");
			for (int i = 0; i < frames.Length; i++) {
				frames [i].SetActive (false);
			}
			anim.Play ("ending");
			seqtext.text = "win!!!!!!!!!!";
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

					int step = (int)(angle / 360f * sequence.Length);
					Debug.Log (step);

					frameInput.Remove (step);
					frameInput.Insert (0, step);
				}
			}
		}
	}

	void randomizeSequence(){
		string s = "0000000000";
		string s2="";
		List<int> seq = new List<int> (sequence);
		for (int i = 0; i < sequence.Length; i++) {
			int x = seq [Random.Range (0, seq.Count)];
			seq.Remove (x);
			sequence [i] = x;
			s = s.Insert (x, keys[i].ToString());
			s = s.Remove (x+1,1);
			s2 += keys[x].ToString ();
		}
		seqstring = s;
		Debug.Log (s);
	}

	bool check(){

		for (int i = 0; i < sequence.Length; i++) {
			if (keys[frameInput [frameInput.Count-1-i]].ToString() != seqstring[i].ToString()) {
				return false;
			}
		}

		return true;
	}
}
