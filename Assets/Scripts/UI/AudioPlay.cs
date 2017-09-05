using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioPlay : MonoBehaviour {

    private AudioSource source;
    private AudioClip[] Clips;
    private Dictionary<string, AudioClip> AudioClip = new Dictionary<string, AudioClip>();

    // Use this for initialization
    void Start()
    {
        Clips = Resources.LoadAll<AudioClip>("Audios");//调用Resources方法加载AudioClip资源
        for (int i = 0; i < Clips.Length; i++)
        {
            AudioClip Clip = Clips[i];
            string name = Clip.name;
            AudioClip.Add(name, Clip);
        }
        source = (AudioSource)gameObject.GetComponent("AudioSource");
        if (source == null)
            source = GameObject.Find("Audio").GetComponent<AudioSource>();
    }

    public void Play(string str)
    {
        PlayAudioClip(AudioClip[str]);
    }
    public void Play(string str,Vector3 position)
    {                                                                                                        
        PlayAudioClip(AudioClip[str],position);
    }

    private void PlayAudioClip(AudioClip clip)
    {
        source.clip = clip;
        source.minDistance = 1f;
        source.maxDistance = 50f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.Play();
    }

    private void PlayAudioClip(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }

    
	// Update is called once per frame
	void Update ()
    {
		
	}
}
