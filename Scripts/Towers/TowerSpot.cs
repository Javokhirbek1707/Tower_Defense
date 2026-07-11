using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private static bool _particlesActive;

    [SerializeField]
    private bool _isOccupied = false;
    [SerializeField]
    private GameObject _fxObject;

    void Update()
    {
        if (_fxObject == null)
        {
            return;
        }

        bool shouldShow = _particlesActive == true && _isOccupied == false;

        if (_fxObject.activeSelf != shouldShow)
        {
            _fxObject.SetActive(shouldShow);
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
