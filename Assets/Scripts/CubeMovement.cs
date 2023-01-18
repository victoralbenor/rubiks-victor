using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float speed = 1.0f; // adjust to control movement speed

    private Vector3 _mouseDelta;
    private Vector3 _mousePos;
    private Vector3 _previousMousePosition;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _mouseDelta = Input.mousePosition - _previousMousePosition;
            _mouseDelta *= 0.1f * speed;
            transform.Rotate(_mouseDelta.y, -_mouseDelta.x, 0, Space.World);
        }

        _previousMousePosition = Input.mousePosition;
    }
}