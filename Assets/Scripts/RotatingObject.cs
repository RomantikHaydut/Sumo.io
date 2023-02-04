using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private float _rotateAngle = 120f; // In one second.
    void Update()
    {
        Rotate();
    }

    private void Rotate() => transform.Rotate(Vector3.up * _rotateAngle * Time.deltaTime);
}
