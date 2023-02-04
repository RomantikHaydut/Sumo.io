using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBase : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1f;

    public float _turnSpeed = 0.5f;

    [Header("Push")]
    public GameObject _myTarget;

    public GameObject lastPusher;

    public float _pushForce = 1f;

    public int score;

    #region Static values
    public static int killCount = 0;

    public static bool isGameStarted = false;

    public static bool isGameFinished = false;
    #endregion

    void Start()
    {
        // Here we prevent collision between object and object's target.
        Physics.GetIgnoreCollision(GetComponent<Collider>(), _myTarget.GetComponent<Collider>());
        killCount = 0;
        isGameStarted = false;
        isGameFinished = false;
    }

    #region Push 

    private void Push(GameObject pushingObject, float pushMultiplier)
    {
        Rigidbody rb = pushingObject.GetComponent<Rigidbody>();

        Vector3 pushDirection = (pushingObject.transform.position - transform.position).normalized;

        float scalePower = transform.localScale.x;

        Vector3 force = pushDirection * _pushForce * scalePower * pushMultiplier;

        if (rb != null)
            rb.velocity = new Vector3(force.x, 0, force.z);

    }
    #endregion

    private void AddScore(int amount)
    {
        if (transform.CompareTag("Player"))
        {
            FindObjectOfType<ScoreManager>().AddScore(amount); // This score is for main player.
        }

        score += amount; // this score is for every player.
    }
    public void Kill()
    {
        Grow(3);
        AddScore(500);
        if (transform.CompareTag("Player"))
            killCount++;
    }

    private void Death()
    {
        if (lastPusher != null)
        {
            MovementBase movementBase = lastPusher.GetComponent<MovementBase>();

            movementBase.Kill();
        }

        CheckGameIsOver();
        Destroy(gameObject);

    }
    public void Grow(float x) => StartCoroutine(GrowObject_Coroutine(x));
    private IEnumerator GrowObject_Coroutine(float growFactor)
    {
        Vector3 growAmount = (Vector3.one / 8) * growFactor;

        float growTime = 0.1f;
        float timer = 0f;
        Vector3 startScale = transform.localScale;

        while (true)
        {
            yield return null;
            timer += Time.deltaTime;

            Vector3 lerpedScale = Vector3.Lerp(Vector3.zero, growAmount, timer * (1 / growTime));
            transform.localScale = startScale + lerpedScale;

            if (timer >= growTime)
            {
                transform.localScale = startScale + lerpedScale;
                yield break;
            }
        }
    }


    private void CheckGameIsOver()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 1)
        {
            GameManager.Instance.WinGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            Push(other.transform.root.gameObject, 2f);
            // Here we set lastpushers.
            MovementBase movementBase = other.transform.GetComponentInParent(typeof(MovementBase)) as MovementBase;
            if (movementBase != null)
            {
                movementBase.lastPusher = gameObject;
                lastPusher = movementBase.gameObject;
            }

        }
        else if (other.gameObject.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            StartCoroutine(GrowObject_Coroutine(1));
            AddScore(100);
        }
        else if (other.gameObject.CompareTag("DeadZone"))
        {
            Death();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Push(collision.gameObject, 1);
            lastPusher = collision.gameObject;
        }
    }


}
