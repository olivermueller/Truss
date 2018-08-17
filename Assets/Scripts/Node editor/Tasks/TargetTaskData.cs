using TMPro;
using UnityEngine;
public class TargetTaskData:TaskData
{
        Renderer _renderer;
        public override void StartTask()
        {
            _uiObject = GameObject.FindObjectOfType<Canvas>().gameObject;
            
            Debug.Log("<color=green> Started "+ _title +"</color>");
            base.StartTask();
            _renderer = _baseObject.GetComponentInChildren(typeof(Renderer), true) as Renderer;
        }

        public override bool? IsCompleted()
        {
//            Debug.Log("checking is completed" + _title);

            return IsImageTargetActive();
        }
        bool? IsImageTargetActive()
        {
            if (_renderer.enabled)
                return true;
            return null;
        }

}