using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTaskData : TaskData {

        Renderer _renderer;
        public override void StartTask()
        {
            base.StartTask();
        }

        public override bool? IsCompleted()
        {
            return true;
        }

}
