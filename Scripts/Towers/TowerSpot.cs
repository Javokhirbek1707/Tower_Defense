using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private static bool _particlesActive;

    [SerializeField]
    private bool _isOccupied = false;

    private ParticleSystem _particles;

    void Start()
    {
        _particles = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (_particlesActive == true && _isOccupied == false)
        {
            if (_particles.isPlaying == false)
            {
                _particles.Play();
            }
        }
        else
        {
            if (_particles.isPlaying == true)
            {
                _particles.Stop();
            }
        }
    }

    public void SetOccupied()
    {
        _isOccupied = true;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public void SetEmpty()
    {
        _isOccupied = false;
        gameObject.layer = LayerMask.NameToLayer("PlacementSpot");
    }

    public bool IsOccupied()
    {
        return _isOccupied;
    }

    public static void ToggleParticles(bool active)
    {
        _particlesActive = active;
    }
}
