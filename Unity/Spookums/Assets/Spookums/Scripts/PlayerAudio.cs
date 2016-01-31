using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAudio : MonoBehaviour {

	public AudioSource movement;
	public AudioSource vocals;
	public AudioClip[] huh;
	public AudioClip[] scream;
	public AudioClip[] screamLong;
    public AudioClip[] screamExtraLong;
	public AudioClip stairs;
	public Slider fearMeter;

	private Rigidbody2D body;
	private AudioSource moveSound;

	void Start(){
		body = GetComponent<Rigidbody2D> ();
	}

	public void Update(){
		/*if (body.velocity.x > 2) {
			if(walk.isPlaying){
				walk.Stop();
			}
			if(!run.isPlaying){
				run.Play ();
			}
		}
		else if(body.velocity.x > Mathf.Epsilon){
			if(run.isPlaying){
				run.Stop();
			}
			if(!walk.isPlaying){
				walk.Play();
			}
		}
		else if(walk1.isPlaying || run1.isPlaying){
			walk.Stop();
			run.Stop();
		}*/
	}

	public void Stairs(){
		movement.clip = stairs;
		movement.Play ();
	}

    public void React()
    {
        switch (Mathf.RoundToInt(fearMeter.value))
        {
            case 0:
            case 1:
            case 2:
                vocals.clip = huh[Random.Range(0, huh.Length)];
                vocals.PlayDelayed(0.75f);
                break;
            case 3:
            case 4:
                vocals.clip = scream[Random.Range(0, scream.Length)];
                vocals.PlayDelayed(0.5f);
                break;
            case 5:
                vocals.clip = screamLong[Random.Range(0, scream.Length)];
                vocals.PlayDelayed(0.25f);
                break;
        }
    }
    public void Fleeing() {
        vocals.clip = screamExtraLong[Random.Range(0, screamExtraLong.Length)];
        vocals.PlayDelayed(0.25f);
	}
}