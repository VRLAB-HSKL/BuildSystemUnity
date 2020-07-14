using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Build
{
    

    public void BuildConfiguration()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] {"Assets/Scenes/VIU-Win.unity"};
        buildPlayerOptions.locationPathName = "G:/TestBuilds/Test1.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.targetGroup = BuildTargetGroup.Standalone;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

}
