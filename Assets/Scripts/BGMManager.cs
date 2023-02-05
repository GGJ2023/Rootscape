using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class BGMManager : MonoBehaviour
{
    public Sound[] sounds;

    private string[] alternative = { "Lead", "Shakes", "Slap", "Bass" };

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.Play();
            s.source.loop = true;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        StartCoroutine(FadeIn(s));
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        StartCoroutine(FadeOut(s));
    }

    public IEnumerator FadeIn(Sound s)
    {
        while (s.source.volume < 1)
        {
            s.source.volume += Time.deltaTime * 0.01f;
            yield return null;
        }
    }

    public IEnumerator FadeOut(Sound s)
    {
        while (s.source.volume > 0)
        {
            s.source.volume -= Time.deltaTime * 0.01f;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.numberOfSplits == 3)
        {
            FindObjectOfType<BGMManager>().Play("topMelody");
        }
        else if (Globals.numberOfSplits == 8)
        {
            FindObjectOfType<BGMManager>().Play("topMelodyDelay");
        }
        else if (Globals.numberOfSplits == 12)
        {
            FindObjectOfType<BGMManager>().Play("Chords");
        }
        else if (Globals.numberOfSplits == 16)
        {
            FindObjectOfType<BGMManager>().Play("Shakes");
        }
        else if (Globals.numberOfSplits >= 18)
        {
            if (Globals.numberOfSplits % 5 == 0)
            {
                int rand1 = UnityEngine.Random.Range(0, alternative.Length);
                int rand2 = UnityEngine.Random.Range(0, alternative.Length);
                FindObjectOfType<BGMManager>().Stop(alternative[rand1]);
                FindObjectOfType<BGMManager>().Play(alternative[rand2]);
            }
        }
    }
}
