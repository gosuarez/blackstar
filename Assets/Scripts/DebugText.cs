using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{

    public TMP_Text text;

    public void OnResize()
    {
        text.text = "abc";
    }


}
