using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviourScript : MonoBehaviour
{
    public float speed = 5f;
    public float range = 5f;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        startPosition = transform.position;
        GenerateRandomTarget();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            GenerateRandomTarget();
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void GenerateRandomTarget()
    {
        float randomX = Random.Range(-range, range);
        float randomZ = Random.Range(-range, range);
        targetPosition = startPosition + new Vector3(randomX, 0f, randomZ);
    }
}
