using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {
    
    public float timer = 2f;
    public string levelToLoad = "Scene1";

    // Use this for initialization
    void Start()
    {
        StartCoroutine("DisplayScene");

    }

    // Update is called once per frame
    void Update() {
        //timer -= Time.deltaTime;
    }

    IEnumerator DisplayScene()
    {
        yield return new WaitForSeconds(timer);
        Application.LoadLevel(levelToLoad);
    }
}
