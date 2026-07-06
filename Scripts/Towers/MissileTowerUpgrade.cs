using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTowerUpgrade : MonoBehaviour
{
    [SerializeField]
    private GameObject _upgradedMissileTurret;

    public void UpgradeTurret()
    {
        TowerSpot spot = GetComponent<TowerHealth>().GetOccupiedSpot();

        GameObject turret = Instantiate(_upgradedMissileTurret, transform.position, transform.rotation);
        turret.GetComponent<TowerHealth>().Placed();
        turret.GetComponent<TowerHealth>().SetOccupiedSpot(spot);

        Destroy(gameObject);
    }
}