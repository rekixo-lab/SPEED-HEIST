#if UNITY_EDITOR
using System;
using System.IO;
using SpeedHeist;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
namespace SpeedHeist.Editor {
[InitializeOnLoad]
public static class SpeedHeistSetup {
 const string ScenePath="Assets/_SpeedHeist/Scenes/PrototypeRace.unity";
 const string SettingsPath="Assets/_SpeedHeist/Settings";
 const string RendererPath=SettingsPath+"/SH_ForwardRenderer.asset";
 const string PipelinePath=SettingsPath+"/SH_URP.asset";
 static SpeedHeistSetup(){EditorApplication.delayCall+=TryFirstImport;}
 static void TryFirstImport(){
  if(Application.isBatchMode||EditorApplication.isPlayingOrWillChangePlaymode||
     EditorApplication.isCompiling||EditorApplication.isUpdating)return;
  if(!File.Exists(ScenePath))GenerateStarterScene();
 }
 [MenuItem("Tools/SPEED-HEIST/Generate Starter Scene")]
 public static void GenerateStarterScene(){
  if(EditorApplication.isPlaying)throw new InvalidOperationException("Exit Play Mode.");
  Directory.CreateDirectory("Assets/_SpeedHeist/Scenes");
  Directory.CreateDirectory("Assets/_SpeedHeist/Materials");
  Directory.CreateDirectory(SettingsPath);
  PlayerSettings.companyName="Rekixo Lab";
  PlayerSettings.productName="SPEED-HEIST";
  PlayerSettings.bundleVersion="0.1.0";
  PlayerSettings.defaultInterfaceOrientation=UIOrientation.Portrait;
  PlayerSettings.colorSpace=ColorSpace.Linear;
  PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android,"com.rekixo.speedheist");
  PlayerSettings.Android.minSdkVersion=AndroidSdkVersions.AndroidApiLevel26;
  EditorSettings.serializationMode=SerializationMode.ForceText;
  SetupURP();
  Scene scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
  CreateTrack();
  GameObject runner=CreateRunner();
  CreateCamera(runner.transform);
  CreateLight();
  CreateFinish();
  new GameObject("Prototype HUD").AddComponent<PrototypeHud>().Runner=runner.GetComponent<RunnerMotor>();
  EditorSceneManager.SaveScene(scene,ScenePath);
  EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(ScenePath,true)};
  AssetDatabase.SaveAssets();AssetDatabase.Refresh();
  Debug.Log("[SPEED-HEIST] Scene generated: "+ScenePath);
 }
 static void SetupURP(){
  UniversalRendererData rd=AssetDatabase.LoadAssetAtPath<UniversalRendererData>(RendererPath);
  if(rd==null){rd=ScriptableObject.CreateInstance<UniversalRendererData>();AssetDatabase.CreateAsset(rd,RendererPath);}
  UniversalRenderPipelineAsset urp=AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PipelinePath);
  if(urp==null){urp=UniversalRenderPipelineAsset.Create(rd);AssetDatabase.CreateAsset(urp,PipelinePath);}
  GraphicsSettings.defaultRenderPipeline=urp;
  for(int i=0;i<QualitySettings.names.Length;i++)QualitySettings.SetRenderPipelineAssetAt(i,urp);
 }
 static Material Mat(string name,Color color){
  string path="Assets/_SpeedHeist/Materials/"+name+".mat";
  Material m=AssetDatabase.LoadAssetAtPath<Material>(path);
  if(m!=null)return m;
  Shader shader=Shader.Find("Universal Render Pipeline/Lit");
  if(shader==null)throw new Exception("URP Lit shader missing. Check package resolution.");
  m=new Material(shader);m.color=color;AssetDatabase.CreateAsset(m,path);return m;
 }
 static GameObject Block(string name,Vector3 pos,Vector3 scale,Material mat){
  GameObject go=GameObject.CreatePrimitive(PrimitiveType.Cube);
  go.name=name;go.transform.position=pos;go.transform.localScale=scale;
  go.GetComponent<Renderer>().sharedMaterial=mat;return go;
 }
 static void CreateTrack(){
  Material road=Mat("Road",new Color(0.14f,0.25f,0.4f));
  Material wall=Mat("Guard",new Color(0.04f,0.79f,0.98f));
  Material marking=Mat("Markings",new Color(1f,0.71f,0.22f));
  Material hazard=Mat("Hazard",new Color(1f,0.26f,0.27f));
  Block("Wide road",new Vector3(0,-0.5f,82),new Vector3(18,1,190),road);
  Block("Left edge",new Vector3(-8.8f,0.6f,82),new Vector3(0.5f,1.2f,190),wall);
  Block("Right edge",new Vector3(8.8f,0.6f,82),new Vector3(0.5f,1.2f,190),wall);
  for(int i=0;i<26;i++)Block("Mark "+i,new Vector3(0,0.03f,i*7f),new Vector3(0.2f,0.04f,3),marking);
  Block("Hazard A",new Vector3(-3.5f,0.8f,30),new Vector3(2.5f,1.6f,1.5f),hazard);
  Block("Hazard B",new Vector3(3.5f,0.8f,60),new Vector3(2.5f,1.6f,1.5f),hazard);
  Block("Hazard C",new Vector3(0,0.8f,96),new Vector3(2.5f,1.6f,1.5f),hazard);
 }
 static GameObject CreateRunner(){
  GameObject go=GameObject.CreatePrimitive(PrimitiveType.Capsule);
  go.name="Runner";go.transform.position=new Vector3(0,1.2f,0);
  UnityEngine.Object.DestroyImmediate(go.GetComponent<CapsuleCollider>());
  CharacterController cc=go.AddComponent<CharacterController>();
  cc.height=2;cc.radius=0.48f;cc.stepOffset=0.28f;
  go.GetComponent<Renderer>().sharedMaterial=Mat("Runner",new Color(0.95f,0.18f,0.24f));
  go.AddComponent<RunnerMotor>();go.AddComponent<RunnerInput>();return go;
 }
 static void CreateCamera(Transform player){
  GameObject go=new GameObject("Main Camera",typeof(Camera),typeof(AudioListener));
  go.tag="MainCamera";Camera c=go.GetComponent<Camera>();
  c.fieldOfView=68;c.nearClipPlane=0.1f;c.farClipPlane=300;
  c.backgroundColor=new Color(0.48f,0.76f,0.97f);
  go.transform.position=new Vector3(0,5,-9);
  go.AddComponent<RunnerCamera>().Target=player;
 }
 static void CreateLight(){
  GameObject go=new GameObject("Sun",typeof(Light));
  go.GetComponent<Light>().type=LightType.Directional;
  go.GetComponent<Light>().intensity=1.25f;
  go.transform.rotation=Quaternion.Euler(42,-35,0);
  RenderSettings.ambientLight=new Color(0.5f,0.58f,0.7f);
 }
 static void CreateFinish(){
  Material m=Mat("Finish",new Color(0.15f,0.95f,0.34f));
  GameObject gate=Block("Finish trigger",new Vector3(0,2.5f,160),new Vector3(17,5,0.35f),m);
  gate.GetComponent<MeshRenderer>().enabled=false;
  gate.GetComponent<BoxCollider>().isTrigger=true;
  gate.AddComponent<RaceFinish>();
  Block("Finish left post",new Vector3(-7.8f,2.5f,160),new Vector3(0.4f,5,0.6f),m);
  Block("Finish right post",new Vector3(7.8f,2.5f,160),new Vector3(0.4f,5,0.6f),m);
  Block("Finish bar",new Vector3(0,5.1f,160),new Vector3(16,0.7f,0.6f),m);
 }
}
}
#endif
