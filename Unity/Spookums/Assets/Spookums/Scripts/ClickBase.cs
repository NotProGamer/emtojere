using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class ClickBase : MonoBehaviour
{
	[SerializeField] protected float fearIncrement = 0.5f;
	[SerializeField] protected float fearThreshold = 4f;
    [SerializeField]
    protected float cooldown = 5f;
    protected float clickTime = 0f;
    protected AudioSource[] audioSources;
    protected Animator animator;
	protected Slider fearMeter;
	protected bool clickable = true;

    public bool isHouseExit = false;

	PlayerAudio npcAudio;
    NPCScript npc;
    int floor;

    protected virtual void Start()
    {
        audioSources = GetComponents<AudioSource>();
        animator = GetComponent<Animator>();
		fearMeter = GameObject.Find ("FearMeter").GetComponent<Slider> ();

        npc = GameObject.Find("PlayerCollider").GetComponent<NPCScript>();
		npcAudio = GameObject.Find("PlayerCollider").GetComponent<PlayerAudio>();

        // determine what floor we are on using elevation markers.
        if      (transform.position.y > GameObject.Find("2").transform.position.y) floor = 2;
        else if (transform.position.y > GameObject.Find("1").transform.position.y) floor = 1;
        else if (transform.position.y > GameObject.Find("G").transform.position.y) floor = 0;
        else floor = -1;
    }

    protected virtual void OnMouseUpAsButton()
    {
        if (isHouseExit || npc.IsFleeing()) return;

		if ((Time.time > clickTime) && clickable)
        {
            //Trigger animation and audio here
            animator.SetTrigger("Click");
            audioSources[0].Play();

            //Don't allow players to activate until cooldown is finished
            clickTime = Time.time + cooldown;

			bool lure = true;

			if (fearMeter.value > fearThreshold) {
				lure = false;
			}

			// alert player
            npc.Alert(transform.position, lure, floor, true);
			npcAudio.React ();
			fearMeter.value += fearIncrement;

            if (fearMeter.value >= 5.0f)
            {
                npc.EvacuateHouse();
                npc.Alert(GameObject.Find("HouseExit").transform.position, true, 0, false);
            }
			Clicked ();
        }
    }

    protected virtual void OnMouseOver()
    {
        if (isHouseExit) return;

        animator.SetBool("MouseOver", true);
    }

    protected virtual void OnMouseExit()
    {
        if (isHouseExit) return;

        animator.SetBool("MouseOver", false);
    }

	protected virtual void Clicked(){
	}
}
