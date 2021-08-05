using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmissionIntensity : MonoBehaviour
{
    
    [SerializeField] private float intensityValue;
    [SerializeField] private float minimumValue;
    [SerializeField] private float maximumValue = 3f;
    [SerializeField] private float lerpMultiplier = 0.7f;

    private Renderer _renderer;
    private Color _emissionColorValue;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private float _startingLerpValue;
    private float _endLerpValue = 1.3f;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _emissionColorValue = _renderer.material.color;
        StartCoroutine(SelectRandomIntensity());
    }

    // Update is called once per frame
    void Update()
    {
        _startingLerpValue += lerpMultiplier * Time.deltaTime;
        _renderer.material.SetColor(EmissionColor, _emissionColorValue * intensityValue);
    }

    private IEnumerator SelectRandomIntensity()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            intensityValue = Mathf.Lerp(minimumValue, maximumValue, _startingLerpValue);

            if (_startingLerpValue > _endLerpValue)
            {
                float temp = maximumValue;
                maximumValue = minimumValue;
                minimumValue = temp;
                _startingLerpValue = 0.0f;
            }
        }
    }
}
