using Node_editor;
using UnityEngine;
public class TargetTaskData:TaskData
{
        public void Initialize(string title, string description, GameObject animationObject, GameObject imageTargetObject)
        {
            base.Initialize(title, description, animationObject, imageTargetObject);
        }
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

        public TargetTaskData(string title, string description, GameObject animationObject, GameObject imageTargetObject) : base(title, description, animationObject, imageTargetObject)
        {
        
        }
        bool IsImageTargetActive()
        {
            return _renderer.enabled;
        }

}