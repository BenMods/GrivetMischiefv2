using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    void Awake()
    {
        // Disable VSync so Application.targetFrameRate is respected
        QualitySettings.vSyncCount = 0;

        // Set the target frame rate to 72 FPS
        Application.targetFrameRate = 72;

        Debug.Log("[PerformanceManager] Frame rate capped at 72 FPS.");
    }
}
