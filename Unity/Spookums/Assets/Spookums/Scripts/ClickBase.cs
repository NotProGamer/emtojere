using UnityEngine;
using System.Collections;

public abstract class ClickBase : MonoBehaviour
{

    [SerializeField]
    protected float cooldown = 5f;
    protected float clickTime = 0f;
    protected AudioSource[] audioSources;
    protected Animator animator;

    protected virtual void Start()
    {
        audioSources = GetComponents<AudioSource>();
        animator = GetComponent<Animator>();
    }

    protected virtual void OnMouseUpAsButton()
    {
        if (Time.time > clickTime)
        {

            //Trigger animation and audio here
            animator.SetTrigger("Click");
            audioSources[0].Play();

            //Don't allow players to activate until cooldown is finished
            clickTime = Time.time + cooldown;
        }
    }

    protected virtual void OnMouseOver()
    {
        animator.SetBool("MouseOver", true);
    }

    protected virtual void OnMouseExit()
    {
        animator.SetBool("MouseOver", false);
    }
}
