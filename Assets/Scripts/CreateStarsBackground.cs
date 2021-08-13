using UnityEngine;

/// <summary>
/// Adds a simple star-filled background
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class CreateStarsBackground : MonoBehaviour
{
    public int		starCount = 300;
    ParticleSystem  PS;
    ParticleSystem.Particle[] _particles;

    private void Awake ()
    {
        _particles = new ParticleSystem.Particle[ starCount ];
        PS = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        for (int i = 0; i < starCount; i++)
        {
            _particles[i].position = GameScreenBounds.GetRandomPositionFullScreen();
            _particles[i].startSize = Random.Range(.5f, .30f);
            _particles[i].startColor = Random.ColorHSV(0.2f, 1f, 0f, 1f, 0.25f, .65f);
        }
        PS.SetParticles(_particles, _particles.Length);
    }
}