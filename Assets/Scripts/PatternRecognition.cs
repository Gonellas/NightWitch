using System.Collections.Generic;
using UnityEngine;

public class PatternRecognition : MonoBehaviour
{
    // Minimum distance between points to consider a new point
    public float minDistance = 10f;

    // List to store drawing points
    private List<Vector2> drawingPoints = new List<Vector2>();

    // Function to check if a tap is detected
    private bool IsTap()
    {
        return drawingPoints.Count < 3;
    }

    // Function to check if a circle is drawn
    private bool CheckCircle()
    {
        // Check if the shape is closed
        if (Vector2.Distance(drawingPoints[0], drawingPoints[drawingPoints.Count - 1]) < minDistance)
        {
            // Calculate the average distance from each point to the center
            Vector2 center = Vector2.zero;
            foreach (Vector2 point in drawingPoints)
            {
                center += point;
            }
            center /= drawingPoints.Count;

            float radiusSum = 0f;
            foreach (Vector2 point in drawingPoints)
            {
                radiusSum += Vector2.Distance(point, center);
            }
            float averageRadius = radiusSum / drawingPoints.Count;

            // Check if the curvature is consistent (indicating a circular shape)
            float curvatureThreshold = 0.5f; // Adjust as needed
            float curvature = averageRadius / minDistance;
            return curvature > curvatureThreshold;
        }
        else
        {
            return false;
        }
    }

    // Function to check if a line is drawn
    private bool CheckLine()
    {
        // Calculate the direction from the first to the last point
        Vector2 direction = drawingPoints[drawingPoints.Count - 1] - drawingPoints[0];

        // Check if the line is sufficiently long and relatively straight
        float angleThreshold = 10f; // Adjust as needed
        float lengthThreshold = 0.5f * minDistance; // Adjust as needed
        return direction.magnitude > lengthThreshold &&
               (Mathf.Abs(direction.x) > minDistance || Mathf.Abs(direction.y) > minDistance) &&
               Mathf.Abs(Vector2.SignedAngle(Vector2.right, direction)) < angleThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch phase is "Began" or "Moved"
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                // Add the touch position to the list of drawing points
                drawingPoints.Add(touch.position);
            }
            // Check if the touch phase is "Ended"
            else if (touch.phase == TouchPhase.Ended)
            {
                // Check if it's a tap
                if (IsTap())
                {
                    Debug.Log("This is a tap");
                }
                // Check if a circle is drawn
                else if (CheckCircle())
                {
                    Debug.Log("This is a circle");
                }
                // Check if a line is drawn
                else if (CheckLine())
                {
                    Debug.Log("This is a line");
                }
                else
                {
                    Debug.Log("Pattern not recognized");
                }

                // Clear the list of drawing points for the next drawing
                drawingPoints.Clear();
            }
        }
    }
}