using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource layer1;
    public AudioSource layer2;
    public AudioSource layer3;
    public AudioSource layer4;
    public AudioSource layer5;
    public AudioSource layer6;
    public AudioSource layer7;
    public AudioSource layer8;
    // Start is called before the first frame update
    void Start()
    {
        layer1 = gameObject.AddComponent<AudioSource>();
        layer2 = gameObject.AddComponent<AudioSource>();
        layer2.mute = true;
        layer3 = gameObject.AddComponent<AudioSource>();
        layer3.mute = true;
        layer4 = gameObject.AddComponent<AudioSource>();
        layer4.mute = true;
        layer5 = gameObject.AddComponent<AudioSource>();
        layer5.mute = true;
        layer6 = gameObject.AddComponent<AudioSource>();
        layer6.mute = true;
        layer7 = gameObject.AddComponent<AudioSource>();
        layer7.mute = true;
        layer8 = gameObject.AddComponent<AudioSource>();
        layer8.mute = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
