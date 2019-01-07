using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XAPIStatement
    {
       

        public Actor actor;
        //public Verb verb;
        public Obj _object;

        public XAPIStatement(string actorName, string actorMbox, string verbDisplay, string verbID, string objectID, string objectDefinitionName, string objectDefinitionDescription)
        {
            this.actor = new Actor(actorName, actorMbox);
            //this.verb = new Verb(verbID, verbDisplay);
            this._object = new Obj(objectID, objectDefinitionName, objectDefinitionDescription);
        }

       
    }
