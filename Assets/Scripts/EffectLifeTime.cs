using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLifeTime : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private float timeNow;
    // Start is called before the first frame update
    void Start()
    {
        timeNow = 0f;
        AudioSource auSo = GetComponent<AudioSource>();
        if (auSo)
        {
            //if (PlayerPrefs.GetFloat("EffectVolume") < 0)
            //{
                //PlayerPrefs.SetFloat("EffectVolume", 1);
            //}
            float vol = PlayerPrefs.GetFloat("EffectVolume");
            auSo.volume = vol;
            auSo.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeNow >= lifeTime)
        {
            Destroy(this.gameObject);
        }
        timeNow += Time.deltaTime;
    }
}
