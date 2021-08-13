using System.Collections;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    #region Field Declarations

    private Camera _currentCamera;
    private bool _shouldPulse;

    #endregion

    #region Start & Update

    private void Start()
    {
        _currentCamera = GetComponent<Camera>();
    }
    private void Update()
    {
        if (_shouldPulse)
            _currentCamera.backgroundColor = Color.Lerp(Color.black, Color.magenta, Mathf.PingPong(Time.time, .2f));
    }

    #endregion

    #region Public Methods

    public void StartPulsing()
    {
        StartCoroutine(pulseTimer());
    }

    public void StopPulsing()
    {
        _shouldPulse = false;
        _currentCamera.backgroundColor = Color.black;
    }

    #endregion

    private IEnumerator pulseTimer()
    {
        _shouldPulse = true;
        yield return new WaitForSeconds(3);
        StopPulsing();
    }
}
