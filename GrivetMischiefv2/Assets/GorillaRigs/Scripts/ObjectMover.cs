using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public Transform[] points; // Assign the points in the Inspector
    public float speed = 2f;
    private int currentPointIndex = 0;
    private bool movingForward = true;

    void Start()
    {
        if (points.Length > 0)
        {
            transform.position = points[0].position;
        }
    }

    void Update()
    {
        if (points.Length < 2)
            return;

        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        Transform targetPoint = points[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (movingForward)
            {
                currentPointIndex++;
                if (currentPointIndex >= points.Length)
                {
                    currentPointIndex = points.Length - 2;
                    movingForward = false;
                }
            }
            else
            {
                currentPointIndex--;
                if (currentPointIndex < 0)
                {
                    currentPointIndex = 1;
                    movingForward = true;
                }
            }
        }
    }
}