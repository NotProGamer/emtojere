using UnityEngine;
using System.Collections;

public class Door : ClickBase {

	void Awake(){
		clickable = false;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit");
        if (other.transform.position.x < transform.position.x)
        {
            animator.SetTrigger("OpenRight");
            audioSources[1].Play();
			clickable = true;
        }
        else {
            animator.SetTrigger("OpenLeft");
            audioSources[1].Play();
			clickable = true;

        }
    }

	protected override void Clicked(){
		clickable = false;
	}

}
