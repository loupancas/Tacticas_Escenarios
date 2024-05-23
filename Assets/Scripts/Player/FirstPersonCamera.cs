using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Rotation")]
    [Range(-90, 90f)][SerializeField] private float _yMinRotation = -60f;
    [Range(-90f, 90f)][SerializeField] private float _yMaxRotation = 75f;

    private Transform _playerHead;
    private float _mouseY;

    protected Ray _ray;
    protected RaycastHit _rayHit;
    [SerializeField] protected LayerMask _layerMask;

    public void SetHead(Transform headTransform)
    {
        _playerHead = headTransform;
    }

    private void LateUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        transform.position = _playerHead.position;
    }

    public void Rotate(float xAxis, float yAxis)
    {
        _mouseY += yAxis;
        _mouseY = Mathf.Clamp(_mouseY, _yMinRotation, _yMaxRotation);
        transform.rotation = Quaternion.Euler(-_mouseY, xAxis, 0f);
    }

    public void SetFov(float FOV)
    {
        GetComponent<Camera>().DOFieldOfView(FOV, 0.25f);
    }
    
}
