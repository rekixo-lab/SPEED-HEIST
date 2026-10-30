using UnityEngine;
namespace SpeedHeist {
public sealed class RunnerCamera:MonoBehaviour {
 public Transform Target {get;set;}
 [SerializeField] Vector3 offset=new Vector3(0,5.2f,-9.5f);
 void LateUpdate(){
  if(Target==null)return;
  transform.position=Vector3.Lerp(transform.position,Target.position+offset,1f-Mathf.Exp(-8f*Time.deltaTime));
  transform.LookAt(Target.position+new Vector3(0,1.4f,7f));
 }
}
}