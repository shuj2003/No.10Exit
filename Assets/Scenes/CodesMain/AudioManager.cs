using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channleIndex;

    public enum Sfx { 
        Run = 0,
        OpenDoor,
        CloseDoor,
        Select,
        Decision,
        Laughter,
        Stab,
        HIT,
        CALL,
        CALL_FAIL,
    }

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int index = 0 ; index < channels ; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }

    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            if(!bgmPlayer.isPlaying)
                bgmPlayer.Play();
        }
        else
        {
            if (bgmPlayer.isPlaying)
                bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx, Action callback = null)
    {

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channleIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            channleIndex = loopIndex;

            sfxPlayers[channleIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[channleIndex].Play();

            if(callback != null)
            {
                StartCoroutine(Checking(sfxPlayers[channleIndex], callback));
            }

            break;

        }
            
    }

    public void PlaySfxOnlyOne(Sfx sfx)
    {

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            if (sfxPlayers[index].clip == sfxClips[(int)sfx] && sfxPlayers[index].isPlaying)
            {
                return;
            }

        }

        PlaySfx(sfx);

    }
    public void StopSfxOnlyOne(Sfx sfx)
    {

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            if (sfxPlayers[index].clip == sfxClips[(int)sfx] && sfxPlayers[index].isPlaying)
            {
                sfxPlayers[index].Stop();
                return;
            }
        }
            
    }

    private void Update()
    {
        bgmPlayer.volume = bgmVolume;
    }

    private IEnumerator Checking(AudioSource audio, Action callback)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!audio.isPlaying)
            {
                callback();
                break;
            }
        }
    }

}
