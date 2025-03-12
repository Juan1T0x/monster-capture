using UnityEngine;
using System.Collections.Generic;

public class VisualPointsController : MonoBehaviour
{
    [Header("Point Prefab")]
    public GameObject pointPrefab;
    private GameObject firstPointInstance;


    public void CreateFirstPoint(Vector2 startCoordinates)
    {
        if (firstPointInstance == null)
        {
            firstPointInstance = Instantiate(pointPrefab, new Vector3(startCoordinates.x, startCoordinates.y, 0), Quaternion.identity);
            firstPointInstance.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void UpdateFirstPoint(IReadOnlyList<Vector2> points)
    {
        if (points.Count > 0)
        {
            firstPointInstance.transform.position = new Vector3(points[0].x, points[0].y, 0);
        }
    }

    public void Clear()
    {
        if (firstPointInstance != null)
        {
            Destroy(firstPointInstance);
            firstPointInstance = null;
        }
    }
}
