using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SakasoftLogo : MonoBehaviour {

    public float Cooldown;
    private float CurrentTime;

	// Use this for initialization
	void Start () {
        CurrentTime = Time.time + Cooldown;
        if (PlayerPrefs.GetInt("FirstPlay") != 1)
        {
            PlayerPrefs.SetInt("FirstPlay", 1);
            PlayerPrefs.SetFloat("MusicVolume", 1);
            PlayerPrefs.SetFloat("EffectVolume", 1);
            PlayerPrefs.SetInt("HandNow", 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if(Time.time > CurrentTime || Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
	}
}
