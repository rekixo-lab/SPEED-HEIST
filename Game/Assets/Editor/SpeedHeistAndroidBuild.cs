#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace SpeedHeist.Editor {
public static class SpeedHeistAndroidBuild {
 const string ScenePath="Assets/_SpeedHeist/Scenes/PrototypeRace.unity";
 [MenuItem("Tools/SPEED-HEIST/Build Android Test APK")]
 public static void BuildAndroid(){
  if(!File.Exists(ScenePath))throw new FileNotFoundException("Generate Starter Scene first.",ScenePath);
  if(!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android,BuildTarget.Android))
   throw new InvalidOperationException("Install Android Build Support, SDK/NDK and OpenJDK in Unity Hub.");
  if(!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android,BuildTarget.Android))
   throw new InvalidOperationException("Could not select Android build target.");
  PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android,ScriptingImplementation.IL2CPP);
  PlayerSettings.Android.targetArchitectures=AndroidArchitecture.ARM64;
  EditorUserBuildSettings.buildAppBundle=false;
  string path=Path.GetFullPath("Builds/SpeedHeist-Debug.apk");
  Directory.CreateDirectory(Path.GetDirectoryName(path));
  BuildReport report=BuildPipeline.BuildPlayer(new BuildPlayerOptions {
   scenes=new[]{ScenePath},locationPathName=path,target=BuildTarget.Android,options=BuildOptions.Development
  });
  if(report.summary.result!=BuildResult.Succeeded)
   throw new Exception("APK build unsuccessful. Check Editor.log.");
  Debug.Log("[SPEED-HEIST] APK created: "+path);
 }
}
}
#endif
