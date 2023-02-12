using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float defaultSpread, defaultDoppler, defaultMaxDistance;
    
    public SoundInfo[] soundsInfo;
    public Dictionary<string, AudioSource[]> soundList = new Dictionary<string, AudioSource[]>();
    private void Start()
    {
        
        foreach (var item in soundsInfo)
        {
            soundList.Add(item.name, item.audio);
        }
        MainGame.instance.soundManager = this;
    }

    public void PlaySound(string soundName)
    {
        AudioSource[] Arr = soundList[soundName];
        AudioSource chosen = Arr[Random.Range(0, Arr.Length)];
        chosen.spread = 0;
        chosen.dopplerLevel = 0;
        chosen.maxDistance = 500f;
        chosen.Play();
    }
    public void PlaySound(string soundName,Vector3 pos)
    {
        AudioSource[] Arr = soundList[soundName];
        AudioSource chosen = Arr[Random.Range(0, Arr.Length)];
        chosen.transform.position = pos;
        chosen.Play();
    }
    public void PlaySound(string soundName, Vector3 pos, bool isDefault)
    {
        AudioSource[] Arr = soundList[soundName];
        AudioSource chosen = Arr[Random.Range(0, Arr.Length)];
        chosen.transform.position = pos;
        if(isDefault)
        {
            chosen.spread = defaultSpread;
            chosen.dopplerLevel = defaultDoppler;
            chosen.maxDistance = defaultMaxDistance;
        }
        chosen.Play();
    }
    public void PlaySound(string soundName, Vector3 pos, float _dopplerLevel)
    {
        AudioSource[] Arr = soundList[soundName];
        AudioSource chosen = Arr[Random.Range(0, Arr.Length)];
        chosen.transform.position = pos;
        chosen.dopplerLevel = _dopplerLevel;
        chosen.Play();
    }
    public void PlaySound(string soundName, Vector3 pos, float _dopplerLevel, float _spread)
    {
        AudioSource[] Arr = soundList[soundName];
        AudioSource chosen = Arr[Random.Range(0, Arr.Length)];
        chosen.transform.position = pos;
        chosen.dopplerLevel = _dopplerLevel;
        chosen.spread = _spread;
        chosen.Play();
    }
    public void PlaySound(string soundName, Vector3 pos, float _dopplerLevel, float _spread, float _maxDistance)
    {
        AudioSource[] Arr = soundList[soundName];
        AudioSource chosen = Arr[Random.Range(0, Arr.Length)];
        chosen.transform.position = pos;
        chosen.dopplerLevel = _dopplerLevel;
        chosen.spread = _spread;
        chosen.maxDistance = _maxDistance;
        chosen.Play();
    }

}

[System.Serializable]
public class SoundInfo
{
    public AudioSource[] audio;
    public string name;
}
