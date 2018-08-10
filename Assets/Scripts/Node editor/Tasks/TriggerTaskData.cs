using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTaskData : TaskData {

        Renderer _renderer;
        public override void StartTask()
        {
            base.StartTask();
            _renderer = _baseObject.GetComponent<Renderer>();
        }

        public override bool? IsCompleted()
        {
            return IsTriggerEntered();
        }
        bool IsTriggerEntered()
        {
            return _renderer.enabled;
        }
}
