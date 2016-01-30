using UnityEngine;
using System.Collections;

public class ClickableObject : MonoBehaviour {

	[SerializeField] private float cooldown = 5f;
	[SerializeField] private Sprite hoverSprite;
	[SerializeField] private Sprite idleSprite;
	private float clickTime = 0f;
	private AudioSource audioSource;
	private SpriteRenderer spriteRenderer;

	void Start(){
		audioSource = GetComponent<AudioSource> ();
	}

	void OnMouseUpAsButton(){
		if(Time.time > clickTime){
			
			//Trigger animation here

			audioSource.Play();

			//Don't allow players to activate until cooldown is finished
			clickTime = Time.time + cooldown;
		}
	}

	void OnMouseOver(){
		spriteRenderer.sprite = hoverSprite;
	}

	void OnMouseExit(){
		spriteRenderer.sprite = idleSprite;
	}
}
