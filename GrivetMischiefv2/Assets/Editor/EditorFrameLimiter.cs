#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using System.Threading;

[InitializeOnLoad]
public class EditorFrameLimiter
{
    static EditorFrameLimiter()
    {
        EditorApplication.update += () =>
        {
            Thread.Sleep(15); // ~60 FPS
        };
    }
}
#endif
