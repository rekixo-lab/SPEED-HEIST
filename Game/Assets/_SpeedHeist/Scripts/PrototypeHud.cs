using UnityEngine;
namespace SpeedHeist {
public sealed class PrototypeHud:MonoBehaviour {
 public RunnerMotor Runner {get;set;}
 float started;
 void Start(){started=Time.time;}
 void OnGUI(){
  if(Runner==null)return;
  GUI.skin.label.fontSize=Mathf.RoundToInt(Mathf.Clamp(Screen.height*0.025f,16,28));
  GUI.Label(new Rect(24,18,Screen.width-30,48),"SPEED HEIST  "+Mathf.FloorToInt(Time.time-started)+"s");
  GUI.Label(new Rect(24,60,Screen.width-30,48),"Distance "+Mathf.FloorToInt(Runner.Distance)+"/160m");
  if(Runner.Finished)GUI.Box(new Rect(Screen.width*0.1f,Screen.height*0.38f,Screen.width*0.8f,100),"FINISH! Prototype complete");
  else GUI.Label(new Rect(24,Screen.height-76,Screen.width-40,70),"Steer: left drag / A,D     Jump: right tap / Space");
 }
}
}