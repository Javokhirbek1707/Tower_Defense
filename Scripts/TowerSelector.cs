using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelector : MonoBehaviour
{
    [SerializeField]
    private Camera _playerCam;

    [SerializeField]
    private LayerMask _towerLayer;

    [SerializeField]
    private GameObject _upgradeGunPopup;

    [SerializeField]
    private GameObject _upgradeMissilePopup;

    [SerializeField]
    private GameObject _dismantlePopup;

    private GameObject _selectedTower;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray cameraRay = _playerCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(cameraRay, out RaycastHit info, 100f, _towerLayer))
            {
                Transform hitRoot = info.collider.transform.root;

                if (hitRoot.CompareTag("Turret"))
                {
                    _selectedTower = hitRoot.gameObject;
                    ShowPopupForTower(_selectedTower);
                }
            }
        }
    }

    private void ShowPopupForTower(GameObject tower)
    {
        HideAllPopups();

        if (tower.GetComponent<GatlingTowerUpgrade>() != null)
        {
            _upgradeGunPopup.SetActive(true);
        }
        else if (tower.GetComponent<MissileTowerUpgrade>() != null)
        {
            _upgradeMissilePopup.SetActive(true);
        }
        else
        {
            _dismantlePopup.SetActive(true);
        }
    }

    private void HideAllPopups()
    {
        _upgradeGunPopup.SetActive(false);
        _upgradeMissilePopup.SetActive(false);
        _dismantlePopup.SetActive(false);
    }

    public void ConfirmUpgrade()
    {
        if (_selectedTower == null)
        {
            return;
        }

        int cost = 0;

        if (_selectedTower.GetComponent<GatlingTowerUpgrade>() != null)
        {
            cost = 500;
        }
        else if (_selectedTower.GetComponent<MissileTowerUpgrade>() != null)
        {
            cost = 750;
        }

        if (UIManager.Instance.warFunds < cost)
        {
            CancelPopup();
            return;
        }

        UIManager.Instance.SpendWarFunds(cost);

        if (_selectedTower.GetComponent<GatlingTowerUpgrade>() != null)
        {
            _selectedTower.GetComponent<GatlingTowerUpgrade>().UpgradeTurret();
        }
        else if (_selectedTower.GetComponent<MissileTowerUpgrade>() != null)
        {
            _selectedTower.GetComponent<MissileTowerUpgrade>().UpgradeTurret();
        }

        _selectedTower = null;
        HideAllPopups();
    }

    public void ConfirmDismantle()
    {
        if (_selectedTower == null)
        {
            return;
        }

        UIManager.Instance.AddWarFunds(250);
        Destroy(_selectedTower);

        _selectedTower = null;
        HideAllPopups();
    }

    public void CancelPopup()
    {
        _selectedTower = null;
        HideAllPopups();
    }
}