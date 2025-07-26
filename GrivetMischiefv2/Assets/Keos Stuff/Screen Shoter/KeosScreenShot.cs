#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class KeosScreenShot : EditorWindow
{
    public enum ResolutionPreset
    {
        HD_720p,
        FullHD_1080p,
        QuadHD_1440p,
        UltraHD_4K,
        Custom
    }

    public enum FileTypes
    {
        PNG,
        JPG,
        JPEG
    }

    private ResolutionPreset ResPreset = ResolutionPreset.FullHD_1080p;
    private FileTypes FileType = FileTypes.PNG;
    private int CustomWidth = 1920;
    private int CustomHeight = 1080;

    private const string SavePath = "Assets/Keos Stuff/Screen Shoter/Screenshots";

    [MenuItem("Keos stuff/Screenshot  tool")]
    public static void ShowWindow()
    {
        GetWindow<KeosScreenShot>("Screenshot tool");
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(-100);
        GUILayout.Label(EditorGUIUtility.IconContent("Assets/Keos Stuff/Icons/Keos Screenshot Tool.png").image, GUILayout.Width(position.width), GUILayout.Height(401));
        GUILayout.Space(-100);
        GUILayout.EndVertical();

        GUILayout.Label("Screenshot Settings", EditorStyles.boldLabel);

        ResPreset = (ResolutionPreset)EditorGUILayout.EnumPopup("Resolution Preset", ResPreset);

        if (ResPreset == ResolutionPreset.Custom)
        {
            CustomWidth = EditorGUILayout.IntField("Custom Width", CustomWidth);
            CustomHeight = EditorGUILayout.IntField("Custom Height", CustomHeight);
        }

        FileType = (FileTypes)EditorGUILayout.EnumPopup("File Type", FileType);

        EditorGUILayout.Space();

        if (GUILayout.Button("Take Screenshot"))
        {
            DoScreenshot();
        }
    }

    private void DoScreenshot()
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        Vector2 res = GetRes();
        string ext = FileType.ToString().ToLower();
        string name = $"{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{ext}";
        string p = Path.Combine(SavePath, name);

        Camera sceneCamera = SceneView.lastActiveSceneView?.camera;

        if (!sceneCamera)
            return;

        RenderTexture rt = new RenderTexture((int)res.x, (int)res.y, 24);
        RenderTexture currentRT = RenderTexture.active;

        sceneCamera.targetTexture = rt;
        RenderTexture.active = rt;

        sceneCamera.Render();

        Texture2D screenshot = new Texture2D((int)res.x, (int)res.y, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        screenshot.Apply();

        sceneCamera.targetTexture = null;
        RenderTexture.active = currentRT;

        DestroyImmediate(rt);

        byte[] bytes = FileType switch
        {
            FileTypes.PNG => screenshot.EncodeToPNG(),
            FileTypes.JPG => screenshot.EncodeToJPG(),
            FileTypes.JPEG => screenshot.EncodeToJPG(),
            _ => screenshot.EncodeToPNG(),
        };

        File.WriteAllBytes(p, bytes);
        Debug.Log($"Screenshot saved to: {p}");

        AssetDatabase.Refresh();
    }

    private Vector2 GetRes()
    {
        return ResPreset switch
        {
            ResolutionPreset.HD_720p => new Vector2(1280, 720),
            ResolutionPreset.FullHD_1080p => new Vector2(1920, 1080),
            ResolutionPreset.QuadHD_1440p => new Vector2(2560, 1440),
            ResolutionPreset.UltraHD_4K => new Vector2(3840, 2160),
            ResolutionPreset.Custom => new Vector2(CustomWidth, CustomHeight),
            _ => new Vector2(1920, 1080),
        };
    }
}
#endif