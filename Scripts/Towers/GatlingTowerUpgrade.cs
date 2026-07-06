using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingTowerUpgrade : MonoBehaviour
{
    [SerializeField]
    private GameObject _dualTurret;

    public void UpgradeTurret()
    {
        TowerSpot spot = GetComponent<TowerHealth>().GetOccupiedSpot();

        GameObject turret = Instantiate(_dualTurret, transform.position, transform.rotation);
        turret.GetComponent<TowerHealth>().Placed();
        turret.GetComponent<TowerHealth>().SetOccupiedSpot(spot);

        Destroy(gameObject);
    }
}
