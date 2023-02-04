using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovementBase
{
    #region Movement
    [SerializeField] private Joystick _joystick;

    private float turnSmootVelocity;

    private float scale;
    #endregion

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (isGameStarted && !isGameFinished)
            Rotation();
    }
    void Update()
    {
        if (isGameStarted && !isGameFinished)
            if (!Pushing())
                Movement();

    }

    #region Movement
    private void Rotation()
    {
        //We are taking our inputs from joystick here.
        float horizontalInput = _joystick.Horizontal;
        float verticalInput = _joystick.Vertical;

        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        //This condition keeps us stay in idle and prevents little touches.
        if (direction.magnitude >= 0.05f)
        {
            //Here we convert the incoming values ​​to a direction and we rotate player to this direction smoothly.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmootVelocity, _turnSpeed);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

        }
    }

    private void Movement()
    {
        scale = transform.localScale.x;

        transform.position += transform.forward * Time.deltaTime * moveSpeed * scale;
        AnimationSpeed();
    }

    private void AnimationSpeed()
    {
        _anim.SetFloat("Speed", moveSpeed);
    }
    #endregion

    private void OnDestroy()
    {
        GameManager.Instance.LoseGame();
    }
}
