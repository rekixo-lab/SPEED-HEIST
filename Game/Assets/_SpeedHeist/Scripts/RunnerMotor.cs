using UnityEngine;
namespace SpeedHeist {
[RequireComponent(typeof(CharacterController))]
public sealed class RunnerMotor : MonoBehaviour {
 [SerializeField] private float forwardSpeed=7f, sidewaysSpeed=7f, jumpHeight=1.7f, gravity=-24f;
 CharacterController controller; float steering,verticalSpeed; bool jump;
 public bool Finished {get;private set;}
 public float Distance => Mathf.Max(0,transform.position.z);
 void Awake(){controller=GetComponent<CharacterController>();}
 public void SetInput(float steer,bool pressed){steering=Mathf.Clamp(steer,-1,1);jump|=pressed;}
 void Update(){
  if(Finished)return;
  if(controller.isGrounded && verticalSpeed<0)verticalSpeed=-2f;
  if(jump && controller.isGrounded)verticalSpeed=Mathf.Sqrt(jumpHeight*-2f*gravity);
  jump=false; verticalSpeed+=gravity*Time.deltaTime;
  controller.Move(new Vector3(steering*sidewaysSpeed,verticalSpeed,forwardSpeed)*Time.deltaTime);
  Vector3 p=transform.position;
  if(p.x < -7f || p.x > 7f){p.x=Mathf.Clamp(p.x,-7f,7f);controller.enabled=false;transform.position=p;controller.enabled=true;}
  if(p.y < -8f){controller.enabled=false;transform.position=new Vector3(0,1.2f,Mathf.Max(0,p.z-8f));controller.enabled=true;verticalSpeed=0f;}
 }
 public void Finish(){Finished=true;steering=0;jump=false;}
}
}