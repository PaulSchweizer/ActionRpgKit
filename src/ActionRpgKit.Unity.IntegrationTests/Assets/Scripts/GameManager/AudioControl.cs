using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour
{
    public AudioMixerSnapshot Background;
    public AudioMixerSnapshot Combat;
    public AudioSource mainAudioSource;
    public AudioSource textAudioSource;

    public float bpm = 128;

    private float m_TransitionIn;
    private float m_TransitionOut;
    private float m_QuarterNote;

    /// <summary>
    /// The Singleton instance.</summary>
    public static AudioControl Instance;

    /// <summary>
    /// Keep the Controller a Singleton.</summary>
    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        m_QuarterNote = 60 / bpm;
        m_TransitionIn = m_QuarterNote * 4;
        m_TransitionOut = m_QuarterNote * 32;
    }

    public void StartBackgroundMusic()
    {
        Background.TransitionTo(m_TransitionOut);
    }

    public void StartCombatMusic()
    {
        Combat.TransitionTo(m_TransitionIn);
    }

    public void PlaySound(AudioClip clip)
    {
        mainAudioSource.PlayOneShot(clip);
    }

    public void PlayText(AudioClip clip)
    {
        textAudioSource.PlayOneShot(clip);
    }

    public void StopText()
    {
        textAudioSource.Stop();
    }

}