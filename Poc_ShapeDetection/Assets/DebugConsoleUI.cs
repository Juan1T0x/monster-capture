using System.Collections.Generic;
using UnityEngine;

public class DebugConsoleUI : MonoBehaviour
{
    [Header("UI Settings")]
    [Tooltip("Font size for the debug text")]
    public int fontSize = 20;
    [Tooltip("Color of the debug text")]
    public Color textColor = Color.white;
    [Tooltip("Maximum number of lines to display on screen")]
    public int maxLines = 20;
    [Tooltip("Duration (in seconds) each message remains on screen")]
    public float messageDuration = 4f;

    // List to store log messages with their timestamp
    private List<LogEntry> logEntries = new List<LogEntry>();

    // GUIStyle for the debug text
    private GUIStyle guiStyle;

    private void Awake()
    {
        // Set up the GUI style for the text
        guiStyle = new GUIStyle();
        guiStyle.fontSize = fontSize;
        guiStyle.normal.textColor = textColor;

        // Subscribe to Unity's log message event
        Application.logMessageReceived += HandleLog;
    }

    private void Start()
    {
        Debug.Log("TEST START");
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid issues when the object is destroyed
        Application.logMessageReceived -= HandleLog;
    }

    /// <summary>
    /// Captures console log messages and stores them.
    /// </summary>
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string msg = $"[{type}] {logString}";
        if (type == LogType.Exception)
        {
            msg += "\n" + stackTrace;
        }

        logEntries.Add(new LogEntry(msg, Time.time));

        // Remove the oldest message if the max number of lines is exceeded
        if (logEntries.Count > maxLines)
        {
            logEntries.RemoveAt(0);
        }
    }

    private void OnGUI()
    {
        // Remove messages that have exceeded their display duration
        logEntries.RemoveAll(entry => (Time.time - entry.timeStamp) > messageDuration);

        // Build the final string with each message on a new line
        string output = "";
        foreach (LogEntry entry in logEntries)
        {
            output += entry.message + "\n";
        }

        // Display the text on screen
        GUI.Label(new Rect(10, 10, Screen.width - 20, Screen.height - 20), output, guiStyle);
    }

    // Internal class to store each log entry
    private class LogEntry
    {
        public string message;
        public float timeStamp;

        public LogEntry(string message, float timeStamp)
        {
            this.message = message;
            this.timeStamp = timeStamp;
        }
    }
}
