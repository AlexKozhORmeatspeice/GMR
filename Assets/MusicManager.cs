using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioSource _audioS;

    private float startV;
    // Start is called before the first frame update
    void Start()
    {
        _audioS = GetComponent<AudioSource>();
        startV = _audioS.volume;
    }

    // Update is called once per frame
    void Update()
    {
        print(PlayerPrefs.GetFloat("Volume"));
        _audioS.volume = startV * PlayerPrefs.GetFloat("Volume");
    }
}
