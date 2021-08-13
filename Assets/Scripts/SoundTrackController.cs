using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrackController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Play sound track for each scene
        DataManager.Instance.GetComponent<SoundTrackManager>().PlaySoundTrackOnStart();
    }
}
