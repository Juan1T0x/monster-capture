using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineStorageManager : MonoBehaviour
{
    [SerializeField] private List<LineRenderer> currentLines = new List<LineRenderer>();
    [SerializeField] private float removeLinesDelay = 1f;

    public void AddLine(LineRenderer line)
    {
        currentLines.Add(line);
        StartCoroutine(RemoveLineAfterDelay(line, removeLinesDelay));
    }

    private IEnumerator RemoveLineAfterDelay(LineRenderer line, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (currentLines.Contains(line))
        {
            currentLines.Remove(line);
            Destroy(line.gameObject);
        }
    }

    [ContextMenu("Clear All Lines")]
    public void ClearAllLines()
    {
        foreach (LineRenderer line in currentLines)
        {
            Destroy(line.gameObject);
        }
        currentLines.Clear();
    }
}
