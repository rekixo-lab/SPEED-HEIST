using UnityEngine;
namespace SpeedHeist {
public sealed class RunnerCamera:MonoBehaviour {
 [SerializeField] private Transform target;
 public Transform Target {get=>target;set=>target=value;}
 [SerializeField] Vector3 offset=new Vector3(0,5.2f,-9.5f);
 void LateUpdate(){
  if(target==null)return;
  transform.position=Vector3.Lerp(transform.position,target.position+offset,1f-Mathf.Exp(-8f*Time.deltaTime));
  transform.LookAt(target.position+new Vector3(0,1.4f,7f));
 }
}
}