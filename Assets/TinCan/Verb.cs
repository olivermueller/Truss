﻿/*
    Copyright 2014 Rustici Software

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using Newtonsoft.Json.Linq;
using TinCan.Json;
using UnityEngine;

namespace TinCan
{
    [Serializable]
    public class Verb : JsonModel
    {
        public Uri id { get; set; }
        public LanguageMap display { get; set; }

        public Verb() {}

        public Verb(StringOfJSON json): this(json.toJObject()) {}

        public Verb(JObject jobj)
        {
            if (jobj["id"] != null)
            {
                id = new Uri(jobj.Value<String>("id"));
            }
            if (jobj["display"] != null)
            {
                display = (LanguageMap)jobj.Value<JObject>("display");
            }
        }

        public Verb(Uri uri)
        {
            id = uri;
        }

        public Verb(String str)
        {
            id = new Uri (str);
        }

        public override JObject ToJObject(TCAPIVersion version) {
            JObject result = new JObject();
            if (id != null)
            {
                result.Add("id", id.ToString());
            }

            if (display != null && ! display.isEmpty())
            {
                result.Add("display", display.ToJObject(version));
            }

            return result;
        }

        public static explicit operator Verb(JObject jobj)
        {
            return new Verb(jobj);
        }
    }
}
