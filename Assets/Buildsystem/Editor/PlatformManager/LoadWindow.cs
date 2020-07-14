using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LoadWindow : EditorWindow
{

    private string loadConfigId;
    private int index;
    private string[] showLoadedConfigs;

    private void OnEnable()
    {
        
    }

    private void OnGUI()
    {
        ShowLoadWindow();
    }

    private void ShowLoadWindow()
    {
        GUI.Box(new Rect(0, 0, 320, 45), "");
        GUILayout.BeginArea(new Rect(0, 5, 250, 250));
        GUILayout.Label("Choose Configurations:");
        if (GUI.Button(new Rect(150, 0, 100, 20), "Open WebApp"))
        {
            Application.OpenURL("http://localhost:3000/unityconfigs");
        }
        GUILayout.EndArea();

        GUI.Box(new Rect(0, 50, 320, 75), "");
        GUILayout.BeginArea(new Rect(0, 50, 250, 250));
        GUILayout.Label("Load Configuration:");
        loadConfigId = EditorGUILayout.TextField("Config. ID:", loadConfigId);
        if (GUI.Button(new Rect(150, 40, 100, 20), "Load"))
        {

        }
        GUILayout.EndArea();

        GUI.Box(new Rect(0, 130, 320, 75), "");
        GUILayout.BeginArea(new Rect(0, 130, 250, 250));
        GUILayout.Label("Show Loaded Configurations:");
        index = EditorGUILayout.Popup(
           "Loaded Configs:",
           index,
           showLoadedConfigs);

        if (GUI.Button(new Rect(50, 40, 100, 20), "Store"))
        {

        }

        if (GUI.Button(new Rect(150, 40, 100, 20), "Delete"))
        {

        }
        GUILayout.EndArea();

        GUI.Box(new Rect(0, 210, 320, 40), "");
        GUILayout.BeginArea(new Rect(0, 180, 250, 250));
        if (GUI.Button(new Rect(100, 40, 100, 20), "Close"))
        {

        }
        GUILayout.EndArea();
    }
}
