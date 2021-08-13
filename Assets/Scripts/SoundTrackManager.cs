using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrackManager : MonoBehaviour
{
    public static readonly Dictionary<string, AudioClip> DictionarySoundTracks = new Dictionary<string, AudioClip>();
    public AudioClip[] soundTracks;

    private string[] _soundTrackNames;
    private int _index;

    private AudioSource _audioSource;

    private void Awake()
    {
            AssignAndGetComponent();
            DictionaryRemoveKey();
            DictionaryAddKey();
    }
    
    public void PlaySoundTrackOnStart()
    {
        switch (DataManager.Instance.currentLevelIndex)
        {
            case 0:
                //PlaySoundTrack_One_Shot("SoundTrack_0");
                PlaySoundTrack("SoundTrack_0");
                break;
            case 1:
                PlaySoundTrack("SoundTrack_1");
                //PlaySoundTrack_One_Shot("SoundTrack_1");
                break;
            case 2:
                PlaySoundTrack("SoundTrack_2");
                //PlaySoundTrack_One_Shot("SoundTrack_2");
                break;
            case 3:
                PlaySoundTrack("SoundTrack_3");
                //PlaySoundTrack_One_Shot("SoundTrack_3");
                break;
            case 4:
                PlaySoundTrack("SoundTrack_4");
                //PlaySoundTrack_One_Shot("SoundTrack_4");
                break;
        }
    }


    private void AssignAndGetComponent()
    {
        _soundTrackNames = new[]
        {
            "SoundTrack_0",
            "SoundTrack_1",
            "SoundTrack_2",
            "SoundTrack_3",
            "SoundTrack_4",
        };

        _audioSource = GetComponent<AudioSource>();
    }
    
    private void DictionaryAddKey()
    {
        foreach (var soundTrackName in _soundTrackNames)
        {
            DictionarySoundTracks.Add(soundTrackName, soundTracks[_index]);
            _index++;
        }
    }

    private void DictionaryRemoveKey()
    {
        foreach (var soundTrackName in _soundTrackNames)
        {
            DictionarySoundTracks.Remove(soundTrackName);
        }
    }
    
    private void OnDisable()
    {
        DictionaryRemoveKey();
    }

    //This is another solution to play soundTracks
    private void PlaySoundTrack(string soundTrackName)
    {
        _audioSource.Stop();
        _audioSource.clip = DictionarySoundTracks[soundTrackName];
        _audioSource.Play();
        _audioSource.loop = true;
    }
    
    private void PlaySoundTrack_One_Shot(string soundTrackName)
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(DictionarySoundTracks[soundTrackName]);
        _audioSource.loop = true;
    }



}
