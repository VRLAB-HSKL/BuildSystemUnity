using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class PlatformConfigurationManager : EditorWindow
{

    int index;

    List<PlatformData> platformDatas;

    string[] platFormConfigs;

    PlatformDataManager PlatformDataManager;

    internal void setPlatformDataMangager(PlatformDataManager platformDataManager)
    {
        this.PlatformDataManager = platformDataManager;
    }

    void init()
    {
        this.platFormConfigs = new string[100];
    }

    void OnEnable()
    {
        init();
    }

    void OnGUI()
    {
        ShowPlatformConfigurationManager();
        this.platFormConfigs = PlatformDataManager.getAllConfigurationNamesAsArray();
    }


    void ShowPlatformConfigurationManager()
    {
        
        GUILayout.BeginArea(new Rect(0, 0, 250, 250));
        GUILayout.Label("Platform Configuration:");
        index = EditorGUI.Popup(
            new Rect(0, 20, 250, 250),
            "Configurations:",
            index,
            platFormConfigs);

        if (GUI.Button(new Rect(0, 45, 50, 50 - 26), "Create"))
        {
            CreateConfiguration createConfigurationWindow =
            (CreateConfiguration)EditorWindow.GetWindow(typeof(CreateConfiguration), true,
            "Create Configuration");
            createConfigurationWindow.SetDataManager(this.PlatformDataManager);
            createConfigurationWindow.Show();
        }

        if (GUI.Button(new Rect(55, 45, 50, 50 - 26), "Edit"))
        {
            
            this.Close();
        }

        if (GUI.Button(new Rect(110, 45, 50, 50 - 26), "Save"))
        {
            this.Close();
        }

        if(GUI.Button(new Rect(0, 150, 50, 50 - 26), "Close"))
        {
            this.Close();
        }
        GUILayout.EndArea();
    }
}
