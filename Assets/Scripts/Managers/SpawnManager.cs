using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;

    [SerializeField] private int maxFoodCount = 10;

    private int foodCount = 0;

    private float _groundRadius;
    void Start()
    {
        _groundRadius = (GameObject.FindGameObjectWithTag("Ground").transform.localScale.x / 2) - 1.5f; // Grounds radius.

        InvokeRepeating("SpawnFoods", 1f, 1f);
        SpawnFoods();
    }

    private void SpawnFoods()
    {
        // We count the number of active foods and round it up to the maximum count.
        foodCount = GameObject.FindGameObjectsWithTag("Food").Length;

        int spawnCount = maxFoodCount - foodCount;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject foodClone = Instantiate(foodPrefab, RandomPosition(), Quaternion.identity);

            RotateRandom(foodClone);
        }
    }

    

    private Vector3 RandomPosition()
    {
        // Random position in radius.
        Vector3 randomPos = Random.insideUnitSphere * _groundRadius;

        randomPos.y = 0;

        return randomPos;
    }

    private void RotateRandom(GameObject rotatingObject)
    {
        float randomAngle = Random.Range(0, 360);

        rotatingObject.transform.Rotate(Vector3.up * randomAngle);
    }
}
