using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float _camSpeed, _rotSpeed;
    [SerializeField]
    private CinemachineVirtualCamera _virtualCam;
    [SerializeField]
    private float _fov, _fovMin, _fovMax;

    void Update()
    {
        VRCamMovement();
        VRCamRot();
        Zoom();
    }

    private void VRCamMovement()
    {
        Vector3 inputDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            inputDirection.z = 1;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            inputDirection.z = -1;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            inputDirection.x = -1;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            inputDirection.x = 1;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            inputDirection.y = 1;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            inputDirection.y = -1;
        }

        Vector3 CamDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x + transform.up * inputDirection.y;
        transform.position += (CamDirection * _camSpeed) * Time.unscaledDeltaTime;

        if (transform.position.z > 25f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 25f);
        }
        else if (transform.position.z < -25f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -25f);
        }

        if (transform.position.x > -14f)
        {
            transform.position = new Vector3(-14f,transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -41f)
        {
            transform.position = new Vector3(-41f, transform.position.y, transform.position.z);
        }

        if (transform.position.y > 20f)
        {
            transform.position = new Vector3(transform.position.x, 20f, transform.position.z);
        }
        else if (transform.position.y < 3f)
        {
            transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
        }
    }

    private void VRCamRot()
    {
        float camRot = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            camRot += -1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            camRot += 1;
        }

        Vector3 NewRot = new Vector3(0, camRot * _rotSpeed * Time.unscaledDeltaTime, 0);
        transform.eulerAngles -= NewRot;
    }

    private void Zoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            _fov += -5;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            _fov += 5;
        }

        _fov = Mathf.Clamp(_fov, _fovMin, _fovMax);
        _virtualCam.m_Lens.FieldOfView = _fov;
    }
}
