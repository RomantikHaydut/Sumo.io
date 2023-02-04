using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovementBase
{
    [SerializeField] private float _attackRange = 2f;

    [SerializeField] State currentState = State.SearchFood;  // Default currentstate is SearchFood.

    private GameObject _player;

    private Transform _target;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (isGameStarted && !isGameFinished)
            Rotation();
    }
    void Update()
    {
        if (isGameStarted && !isGameFinished)
        {
            StateCheck();
            StateExecute();


            if (!Pushing())
                Movement();

        }
    }


    enum State
    {
        SearchFood,
        Attack
    }

    private void StateCheck()
    {
        if (GameManager.Instance.canEnemiesAttack)
        {
            if (IsThereFood())
            {
                float distanceFromPlayer = Vector3.Distance(_player.transform.position, transform.position);

                if (distanceFromPlayer <= _attackRange)
                    currentState = State.Attack;
                else
                    currentState = State.SearchFood;
            }
            else
            {
                currentState = State.Attack;
            }

        }
        else
        {
            currentState = State.SearchFood;
        }

    }

    private void StateExecute()
    {
        switch (currentState)
        {
            case State.SearchFood:
                SearchFood();
                break;
            case State.Attack:
                Attack();
                break;
        }

    }

    private void Rotation()
    {
        if (_target == null)
            return;

        Vector3 targetDirection = (new Vector3(_target.position.x, 0, _target.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
    }

    private void Movement()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }

    private bool IsThereFood()
    {
        // We are checking is there a food in scene.
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");

        if (foods.Length > 0)
            return true;
        else
            return false;
    }

    private void SearchFood()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");

        Transform closestFood = foods[0].transform; // Default food.

        float distance = Vector3.Distance(transform.position, closestFood.position); // Default food's distance.

        //Here we calculate closest food.
        for (int i = 0; i < foods.Length; i++)
        {
            float newDistance = Vector3.Distance(transform.position, foods[i].transform.position);
            if (newDistance < distance)
            {
                distance = newDistance;
                _target = foods[i].transform;
            }
        }


    }

    private void Attack()
    {
        _target = _player.transform;
    }

}
