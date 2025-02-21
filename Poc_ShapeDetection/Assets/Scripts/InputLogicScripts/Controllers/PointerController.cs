using UnityEngine;

public class PointerController : MonoBehaviour
{
    [Header("Pointer Prefab")]
    public GameObject pointerPrefab;
    private GameObject pointerInstance;

    void Start()
    {
        pointerInstance = Instantiate(pointerPrefab);
        pointerInstance.name = "PointerInstance";
    }

    public void UpdatePointerPosition(Vector2 worldPos)
    {
        if (pointerInstance != null)
        {
            pointerInstance.transform.position = new Vector3(worldPos.x, worldPos.y, pointerInstance.transform.position.z);
            if (!pointerInstance.activeSelf)
                pointerInstance.SetActive(true);
        }
    }

    public void HidePointer()
    {
        if (pointerInstance != null && pointerInstance.activeSelf)
            pointerInstance.SetActive(false);
    }
}
