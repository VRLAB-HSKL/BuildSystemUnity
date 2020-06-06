using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;



public class SceneConfManager 
{

    private List<SceneData> sceneDatas;

    public SceneConfig sceneConfig;
    public SceneConfig loadConfig;

    public SceneConfManager()
    {
        sceneDatas = new List<SceneData>();
        sceneConfig = new SceneConfig();
    }

    public void addSceneData(SceneData sceneData)
    {
        bool exists = false;
        foreach(SceneData scene in sceneDatas)
        {
            if(scene.sceneName == sceneData.sceneName)
            {
                exists = true;
            }
            else
            {
                exists = false;
            }
        }

        if(exists == false)
        {
            sceneDatas.Add(sceneData);
        }
        
    }

    public void saveData()
    { 
        sceneConfig.sceneConfigs = sceneDatas;
        XmlSerializer serializer = new XmlSerializer(typeof(SceneConfig));
        using (FileStream stream = new FileStream(Application.dataPath +
            "/Buildsystem/StreamingFiles/XML/save_config.xml", FileMode.Create))
        {
            serializer.Serialize(stream, sceneConfig);
            stream.Close();
        }
        Debug.Log("Stream save Closed");
    } 

    public void loadData()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SceneConfig));
        using (FileStream stream = new FileStream(Application.dataPath +
            "/Buildsystem/StreamingFiles/XML/save_config.xml", FileMode.Open))
        {
            loadConfig = new SceneConfig();
            loadConfig = serializer.Deserialize(stream) as SceneConfig;
            stream.Close();
        }
        Debug.Log("Stream load Closed");
    }




    public string[] getSceneDataNameArray()
    {
        List<string> sceneDataNames = new List<string>();

        foreach(SceneData sceneData in sceneDatas)
        {
            sceneDataNames.Add(sceneData.sceneName);
        }

        return sceneDataNames.ToArray();
    }

    public string[] getSceneDataLoadAsArray()
    {
        loadData();
        sceneDatas = loadConfig.sceneConfigs;
        List<string> sceneDataNames = new List<string>();

        foreach (SceneData sceneData in sceneDatas)
        {
            sceneDataNames.Add(sceneData.sceneName);
        }

        return sceneDataNames.ToArray();
    }

    public SceneData getSceneDataConfiguration(string sceneName)
    {
        SceneData sceneDataToSend = new SceneData();

        foreach (SceneData sceneData in sceneDatas)
        {
            if(sceneData.sceneName == sceneName)
            {
                sceneDataToSend = sceneData;
            }
        }
        return sceneDataToSend;
    }

    public void showSceneDataCount()
    {
        Debug.Log("saved scene configurations: "+sceneDatas.Count);

        foreach(SceneData scenedata in sceneDatas)
        {
            Debug.Log("SceneName: " + scenedata.sceneName + 
                ", BuildTarget:" + scenedata.buildtarget + 
                ", BuildTargetGroup: "+ scenedata.buildtargetGroup +
                ", GVR: " + scenedata.gvr + 
                ", VIU: " + scenedata.viu + 
                ", WaveSDK: " + scenedata.wavevr);
        }
    }
}

