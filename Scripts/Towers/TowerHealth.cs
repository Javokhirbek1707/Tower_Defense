using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [SerializeField]
    private float _health = 100f;

    [SerializeField]
    private TowerSpot _occupiedSpot;

    public bool isPlaced = false;

    public void Placed()
    {
        gameObject.tag = "Turret";
        isPlaced = true;
    }

    public void SetOccupiedSpot(TowerSpot spot)
    {
        _occupiedSpot = spot;
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;

        if (_health < 1)
        {
            FreeSpot();
            Destroy(gameObject);
        }
    }

    private void FreeSpot()
    {
        if (_occupiedSpot != null)
        {
            _occupiedSpot.SetEmpty();
        }
    }

    void OnDestroy()
    {
        FreeSpot();
    }
    public TowerSpot GetOccupiedSpot()
    {
        return _occupiedSpot;
    }
}