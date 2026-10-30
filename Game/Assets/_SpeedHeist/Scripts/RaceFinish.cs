using UnityEngine;
namespace SpeedHeist {
[RequireComponent(typeof(BoxCollider))]
public sealed class RaceFinish:MonoBehaviour {
 void Awake(){GetComponent<BoxCollider>().isTrigger=true;}
 void OnTriggerEnter(Collider other){
  RunnerMotor runner=other.GetComponent<RunnerMotor>();
  if(runner!=null&&!runner.Finished){runner.Finish();Debug.Log("SPEED-HEIST prototype FINISHED");}
 }
}
}