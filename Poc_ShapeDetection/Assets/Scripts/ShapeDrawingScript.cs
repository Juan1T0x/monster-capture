using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShapeDrawingScript : MonoBehaviour
{

    [Header("Debug Settings")]
    public bool debugPoints = true;

    public LineRenderer lineRenderer;
    private List<Vector2> points = new List<Vector2>();
    private bool isDrawing = false;

    [Header("Circle Settings")]
    public int minimumPoints = 10;

    public GameObject pointer;

    public GameObject pointPrefab;
    private List<GameObject> pointList = new List<GameObject>();

    private bool isFirstPoint = true;

    void Update()
    {

        // If the mouse is pressed down
        if (Input.GetMouseButtonDown(0))
        {

            isDrawing = true;
            points.Clear();
            lineRenderer.positionCount = 0;

            foreach (var p in pointList)
            {
                Destroy(p);
            }

            pointList.Clear();

        }

        if (isDrawing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 pointerPos = new Vector3(mousePos.x, mousePos.y, pointer.transform.position.z);
            pointer.transform.position = pointerPos;

            if (!pointer.activeSelf)
                pointer.SetActive(true);

            if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], mousePos) > 0.1f)
            {
                points.Add(mousePos);
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPosition(points.Count - 1, mousePos);


                // If it's the first point, the point will be 2x bigger and red

                if(debugPoints)
                {
                    if (isFirstPoint)
                    {

                        GameObject pointInstance = Instantiate(pointPrefab, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity);
                        pointInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        pointInstance.GetComponent<SpriteRenderer>().color = Color.red;
                        pointList.Add(pointInstance);
                        isFirstPoint = false;

                    }

                    else
                    {

                        GameObject pointInstance = Instantiate(pointPrefab, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity);
                        pointList.Add(pointInstance);

                    }
                }




            }
        }

        if(Input.GetMouseButtonUp(0) && isDrawing)
        {
            isDrawing = false;
            isFirstPoint = true;

            if (pointer.activeSelf)
                pointer.SetActive(false);

            // Print if it's a circle or not in the screen

            if (isCircle(points))
            {
                Debug.Log("Circle");


            }
            else
            {
                Debug.Log("Not a circle");

            }

            points.Clear();
            lineRenderer.positionCount = 0;

            if(debugPoints)
            {
                foreach (var p in pointList)
                {
                    Destroy(p);
                }
            }



            pointList.Clear();
        }
    }

    private bool isCircle(List<Vector2> pts)
    {
        // Check a minimum number of points
        if (pts.Count < minimumPoints)
            return false;

        // Check if the first and last points are close enough (closed shape)
        if (Vector2.Distance(pts[0], pts[pts.Count - 1]) > 0.5f)
            return false;

        // Calculate the center of the shape by averaging all points
        Vector2 center = Vector2.zero;
        foreach (var p in pts)
        {
            center += p;
        }
        center /= pts.Count;

        // Calculate the average radius of the shape by averaging the distances from the center
        float sumRadius = 0;
        List<float> radios = new List<float>();
        foreach (var p in pts)
        {
            float d = Vector2.Distance(p, center);
            radios.Add(d);
            sumRadius += d;
        }
        float avgRadius = sumRadius / pts.Count;

        // Calculate the variance of the radius to check if the shape is a circle
        float variance = 0;
        foreach (float r in radios)
        {
            variance += Mathf.Abs(r - avgRadius);
        }
        variance /= pts.Count;

        // If the variance is less than a threshold, the shape is a circle
        return variance < 0.5f;
    }

}

