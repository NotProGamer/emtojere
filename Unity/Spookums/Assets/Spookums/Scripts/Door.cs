using UnityEngine;
using System.Collections;

public class Door : ClickBase {

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit");
        if (other.transform.position.x < transform.position.x)
        {
            animator.SetTrigger("OpenRight");
            audioSources[1].Play();
        }
        else {
            animator.SetTrigger("OpenLeft");
            audioSources[1].Play();
        }
    }

}
