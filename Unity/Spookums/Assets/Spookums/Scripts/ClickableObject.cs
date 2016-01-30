using UnityEngine;
using System.Collections;

public class ClickableObject : MonoBehaviour {

	[SerializeField] private float cooldown = 5f;
	private float clickTime = 0f;
	private AudioSource audioSource;
    private Animator animator;

	void Start(){
		audioSource = GetComponent<AudioSource> ();
        animator = GetComponent<Animator>();
    }

	void OnMouseUpAsButton(){
        Debug.Log("MouseClick");
        if (Time.time > clickTime){

            //Trigger animation and audio here
            animator.SetTrigger("Click");
			audioSource.Play();

			//Don't allow players to activate until cooldown is finished
			clickTime = Time.time + cooldown;
		}
	}

	void OnMouseOver(){
        Debug.Log("MouseOver");
        animator.SetBool("MouseOver",true);
	}

	void OnMouseExit(){
        Debug.Log("MouseExit");
        animator.SetBool("MouseOver", false);
    }
}
