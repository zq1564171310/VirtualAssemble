using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class Sound : Singleton<Sound>
{

    public string ResourceDir = "";
    private AudioSource m_BGaudio, m_Effectaudio;

    protected override void Awake()
    {
        base.Awake();

        m_BGaudio = this.gameObject.AddComponent<AudioSource>();
        m_BGaudio.loop = true;
        m_BGaudio.playOnAwake = false;

        m_Effectaudio = this.gameObject.AddComponent<AudioSource>();
        m_Effectaudio.spatialize = true;
        m_Effectaudio.loop = false;
        m_Effectaudio.playOnAwake = false;
    }


    public void PlayerBG(string audio)
    {
        string oldname = "";
        if (m_BGaudio.clip == null)
        {
            oldname = "";
        }
        else
        {
            oldname = m_BGaudio.clip.name;
        }

        if (!oldname.Equals(audio))
        {
            string path = "";
            if (string.IsNullOrEmpty(ResourceDir))
            {
                path = "";
            }
            else
            {
                path = ResourceDir + "/" + audio;
            }

            AudioClip clip = Resources.Load<AudioClip>(path);
            m_BGaudio.clip = clip;
            m_BGaudio.Play();
        }
    }

    public void StopBG()
    {
        m_BGaudio.Stop();
    }

    public void PlayerEffect(string name)
    {
        string path = "";
        if (string.IsNullOrEmpty(ResourceDir))
        {
            path = "";
        }
        else
        {
            path = ResourceDir + "/" + name;
        }

        AudioClip clip = Resources.Load<AudioClip>(path);
        m_Effectaudio.clip = clip;
        m_Effectaudio.PlayOneShot(clip);
    }

    public void StopEffect()
    {
        m_Effectaudio.Stop();
    }

	public void InteractiveEffect( bool isactive)
	{
		m_Effectaudio.enabled = isactive;
	}

	public void PlayerOneShout(AudioClip clip,Vector3 pos)
	{
		AudioSource.PlayClipAtPoint (clip,pos);
	}

}