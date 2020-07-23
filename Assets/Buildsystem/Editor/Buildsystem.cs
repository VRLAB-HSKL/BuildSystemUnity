using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.PackageManager.UI;

/// <summary>
/// The Buildsystem Editor Menu
/// </summary>
public class Buildsystem : MonoBehaviour
{
    /// <summary>
    /// <see cref="SceneConfManager"/>SceneConfManager
    /// </summary>
    public static SceneConfManager sceneConfManager = new SceneConfManager();

    /// <summary>
    /// <see cref="PlatformDataManager"/>SceneConfManager
    /// </summary>
    public static PlatformDataManager platformDataManager = new PlatformDataManager();

    /// <summary>
    /// shows new platform configuration
    /// </summary>
    [MenuItem("Buildsystem/Platform/Platform Manager")]
    static void ShowPlatformConfigurationManager()
    {
        PlatformConfigurationManager platformConfigurationManager =
            (PlatformConfigurationManager)EditorWindow.GetWindow(typeof(PlatformConfigurationManager), true,
            "Platform Manager");
        platformConfigurationManager.SetPlatformDataMangager(platformDataManager);
        platformConfigurationManager.Show();
    }

    /// <summary>
    /// sync unity project with buildsystem server
    /// </summary>
    [MenuItem("Buildsystem/Sync Buildsystem")]
    static void SyncWithServer()
    {
        SyncServerWindow syncServerWindow =
            (SyncServerWindow)EditorWindow.GetWindow(typeof(SyncServerWindow),
            true, "Sync with Buildsystem");
        syncServerWindow.SetConfigManager(sceneConfManager);
        syncServerWindow.Show();
    }
}
