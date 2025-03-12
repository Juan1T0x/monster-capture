using UnityEngine;

public class StylusController : MonoBehaviour
{
    [Header("Stylus Prefab")]
    public GameObject stylusPrefab;
    private GameObject stylusInstance;

    private void Start()
    {
        stylusPrefab.SetActive(false);
        stylusInstance = Instantiate(stylusPrefab);
        stylusInstance.name = "StylusInstance";
    }

    public void UpdatePointerPosition(Vector2 worldPos)
    {
        if (stylusInstance != null)
        {
            stylusInstance.transform.position = new Vector3(worldPos.x, worldPos.y, stylusInstance.transform.position.z);
            if (!stylusInstance.activeSelf)
                stylusInstance.SetActive(true);
        }
    }

    public void HidePointer()
    {
        if (stylusInstance != null && stylusInstance.activeSelf)
            stylusInstance.SetActive(false);
    }
}
