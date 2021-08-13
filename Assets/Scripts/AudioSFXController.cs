using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXController : MonoBehaviour
{
   public static readonly Dictionary<string, AudioClip> DictionaryAudioClips = new Dictionary<string, AudioClip>();
   public AudioClip[] audioClips;

   private string[] _audioClipNames;
   private int _index;
   
   private static AudioSource _audioSource;

   private void Awake()
   {
       AssignAndGetComponent();
       DictionaryAddKey();
   }

   private void AssignAndGetComponent()
   {
       _audioClipNames = new[]
       {
           "Ship_Instantiated",
           "Rocket_Thruster",
           "Coin_Selected",
           "Fuel_Selected",
           "Death_Explosion",
           "Landing_Pad_Reached",
       };

       _audioSource = GetComponent<AudioSource>();
   }

   private void DictionaryAddKey()
   {
       foreach (var clipName in _audioClipNames)
       {
           DictionaryAudioClips.Add(clipName, audioClips[_index]);
           _index++;
       }
   }

   private void OnDisable()
   {
       DictionaryRemoveKey();
   }

   private void DictionaryRemoveKey()
   {
       foreach (var clipName in _audioClipNames)
       {
           DictionaryAudioClips.Remove(clipName);
       }
   }

   public static void Main_Play_One_Shot_Audio(string audioName)
   {
       _audioSource.PlayOneShot(DictionaryAudioClips[audioName]);
   }
   
}
 