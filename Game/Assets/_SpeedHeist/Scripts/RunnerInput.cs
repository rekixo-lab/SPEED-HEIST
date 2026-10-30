using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch=UnityEngine.InputSystem.EnhancedTouch.Touch;
namespace SpeedHeist {
[RequireComponent(typeof(RunnerMotor))]
public sealed class RunnerInput:MonoBehaviour {
 RunnerMotor motor;
 void Awake(){motor=GetComponent<RunnerMotor>();}
 void OnEnable(){EnhancedTouchSupport.Enable();}
 void OnDisable(){EnhancedTouchSupport.Disable();}
 void Update(){
  float steer=0f;bool jump=false;
  Keyboard k=Keyboard.current;
  if(k!=null){
   if(k.aKey.isPressed||k.leftArrowKey.isPressed)steer-=1f;
   if(k.dKey.isPressed||k.rightArrowKey.isPressed)steer+=1f;
   jump=k.spaceKey.wasPressedThisFrame;
  }
  foreach(Touch t in Touch.activeTouches){
   if(t.startScreenPosition.x < Screen.width*0.55f)
    steer=Mathf.Clamp((t.screenPosition.x-t.startScreenPosition.x)/Mathf.Max(72f,Screen.width*0.16f),-1f,1f);
   else if(t.phase==UnityEngine.InputSystem.TouchPhase.Began)jump=true;
  }
  motor.SetInput(steer,jump);
 }
}
}