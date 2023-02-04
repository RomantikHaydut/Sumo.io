using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _target;

    private Vector3 _offset;

    [SerializeField] private float _smootFollowTime;
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _offset = _target.position - transform.position;
    }

    void LateUpdate() => FollowTarget();


    private void FollowTarget()
    {
        if (_target == null)
            return;

        Vector3 newOffset = (_offset * _target.transform.localScale.x); // offset changes with player's scale.

        Vector3 targetPosition = _target.position - newOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _smootFollowTime);
    }

}
