using UnityEngine;
using System.Collections;

public class Torbs_Player_Testing : MonoBehaviour {

	void Update() {
        if(Input.GetKeyDown(KeyCode.RightArrow)){
			transform.position += new Vector3(0.1f, 0, 0);
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow)){
			transform.position -= new Vector3(0.1f, 0, 0);
        }
    }
}
