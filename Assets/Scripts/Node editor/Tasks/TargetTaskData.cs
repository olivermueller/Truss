using UnityEngine;
public class TargetTaskData:TaskData
{
        Renderer _renderer;
        public override void StartTask()
        {
            Debug.Log("<color=green> Started "+ _title +"</color>");
            base.StartTask();
            _renderer = _baseObject.GetComponentInChildren(typeof(Renderer), true) as Renderer;
        }

        public override bool? IsCompleted()
        {
//            Debug.Log("checking is completed" + _title);

            return IsImageTargetActive();
        }
        bool IsImageTargetActive()
        {
            return _renderer.enabled;
        }

}