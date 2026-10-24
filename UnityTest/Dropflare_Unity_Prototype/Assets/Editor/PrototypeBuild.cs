using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Shared by Unity Build Automation and optional batch-mode builds.
public sealed class PrototypeBuild : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report) { Configure(); }
    public static void Configure()
    {
        PlayerSettings.companyName = "Bravik Studios";
        PlayerSettings.productName = "Dropflare Unity Test";
        PlayerSettings.bundleVersion = "0.0.1";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android,
            "com.bravikstudios.dropflare.unitytest");
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.Android.bundleVersionCode = 1;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.SetScriptingBackend(UnityEditor.Build.NamedBuildTarget.Android,
            ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.useCustomKeystore = false;
        EditorUserBuildSettings.buildAppBundle = false;
    }
    public static void BuildAndroid()
    {
        Configure();
        Directory.CreateDirectory("Builds/Android");
        var result = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = new[] { "Assets/Scenes/Main.unity" },
            locationPathName = "Builds/Android/Dropflare-Unity-Test-01.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        });
        if (result.summary.result != BuildResult.Succeeded)
            throw new BuildFailedException("Android build failed: " + result.summary.result);
    }
}
