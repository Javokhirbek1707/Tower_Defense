using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField]
    private Camera _playerCam;

    [SerializeField]
    private GameObject _currentTower;

    [SerializeField]
    private LayerMask _placementLayer;

    [SerializeField]
    private GameObject _gatlingPrefab;

    [SerializeField]
    private GameObject _missilePrefab;

    [SerializeField]
    private GameObject _placementRadius;

    private TowerSpot _selectedSpot;

    private bool _canPlace = false;

    private int _towerCost = 0;

    void Update()
    {
        if (_currentTower != null)
        {
            MoveTowerWithMouse();

            if (_canPlace == true && Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }
    }

    private void MoveTowerWithMouse()
    {
        Ray cameraRay = _playerCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out RaycastHit info, 100f, _placementLayer))
        {
            _currentTower.transform.position = info.point;
            _placementRadius.transform.position = info.point;

            if (info.collider.CompareTag("PlacementSpot"))
            {
                _selectedSpot = info.collider.GetComponent<TowerSpot>();

                if (_selectedSpot.IsOccupied() == false && UIManager.Instance.warFunds >= _towerCost)
                {
                    _canPlace = true;
                    _placementRadius.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.green);
                }
                else
                {
                    _canPlace = false;
                    _placementRadius.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
                }
            }
            else
            {
                _selectedSpot = null;
                _canPlace = false;
                _placementRadius.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
            }
        }
    }

    private void PlaceTower()
    {
        _currentTower.transform.position = _selectedSpot.transform.position;
        _selectedSpot.SetOccupied();
        UIManager.Instance.SpendWarFunds(_towerCost);
        TowerSpot.ToggleParticles(false);
        _placementRadius.SetActive(false);
        _currentTower.GetComponent<TowerHealth>().Placed();
        _currentTower.GetComponent<TowerHealth>().SetOccupiedSpot(_selectedSpot);
        _currentTower = null;
        _selectedSpot = null;
        _canPlace = false;
    }

    private void CancelPlacement()
    {
        Destroy(_currentTower);
        TowerSpot.ToggleParticles(false);
        _placementRadius.SetActive(false);
        _currentTower = null;
        _selectedSpot = null;
        _canPlace = false;
    }

    public void SelectGatling()
    {
        if (_currentTower != null)
        {
            Destroy(_currentTower);
        }
        _towerCost = 200;
        _currentTower = Instantiate(_gatlingPrefab, Vector3.zero, Quaternion.identity);
        _placementRadius.SetActive(true);
        TowerSpot.ToggleParticles(true);
    }

    public void SelectMissile()
    {
        if (_currentTower != null)
        {
            Destroy(_currentTower);
        }
        _towerCost = 500;
        _currentTower = Instantiate(_missilePrefab, Vector3.zero, Quaternion.identity);
        _placementRadius.SetActive(true);
        TowerSpot.ToggleParticles(true);
    }
}
