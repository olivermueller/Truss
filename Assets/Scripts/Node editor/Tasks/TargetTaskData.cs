using TMPro;
using UnityEngine;
using Vuforia;

public class TargetTaskData:TaskData
{
//        Renderer _renderer;
    public bool? finished = null;
        public override void StartTask()
        {
            
            Debug.Log("<color=green> Started "+ _title +"</color>");
            _baseObject.GetComponent<MissionTrackableEventHandler>().OnTrackableStateChange.AddListener(IsTargetActive);
            base.StartTask();
        }

        public override bool? IsCompleted()
        {
//            Debug.Log("checking is completed" + _title);
                if (finished.HasValue&&!finished.Value)
                {
                        return null;
                }
                return finished;
        }
//        bool? IsImageTargetActive()
//        {
//            if (_renderer.enabled)
//                return true;
//            return null;
//        }
        public void IsTargetActive(bool active)
        {
                finished = active;
                Debug.LogWarning("Callback made on " + _title + "Value: " + active);
        }
}