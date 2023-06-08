using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopPlayerRotation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private bool _isLeftDirection;

    [SerializeField]
    private float _rotateSpeed;

    [SerializeField]
    private Transform _rotateTransform;

    private bool _isRotating = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isRotating = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isRotating = false;
    }

    private void Update()
    {
        if (!_isRotating)
            return;

        _rotateTransform.Rotate(Vector3.up, Time.unscaledDeltaTime * _rotateSpeed * (_isLeftDirection ? 1 : -1), Space.Self);
    }
}
