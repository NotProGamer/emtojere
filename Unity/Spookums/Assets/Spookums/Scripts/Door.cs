using UnityEngine;
using System.Collections;

public class Door : ClickBase {

    private bool open = false;

	void Awake(){
		clickable = false;
	}

    void OnTriggerEnter2D(Collider2D other)
    {

        if (!open) {
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

        open = true;
        }
    }

	protected override void Clicked(){
		clickable = false;
        open = false;
	}

}
