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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
// using System.Web;
using Newtonsoft.Json.Linq;
using TinCan.Documents;
using TinCan.LRSResponses;
using UnityEngine.Networking;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TinCan
{

    // refernce: https://forum.unity.com/threads/singleton-monobehaviour-script.99971/
    // This class is used to get access to MonoBehaviour allowing us to use IEmumerator 
    public class MyClass : MonoBehaviour
    {
        static MyClass mInstance;

        public static MyClass Instance
        {
            get
            {
                if(mInstance)
                {
                    return mInstance;
                } else
                {
                    return (mInstance = (new GameObject("MyClassContainer")).AddComponent<MyClass>());
                }
            }
        }
    }

    /*
    // reference: https://stackoverflow.com/questions/239725/cannot-set-some-http-headers-when-using-system-net-webrequest
    public static class HttpWebRequestExtensions
    {
        static string[] RestrictedHeaders = new string[] {
            "Accept",
            "Connection",
            "Content-Length",
            "Content-Type",
            "Date",
            "Expect",
            "Host",
            "If-Modified-Since",
            "Keep-Alive",
            "Proxy-Connection",
            "Range",
            "Referer",
            "Transfer-Encoding",
            "User-Agent"
    };

        static Dictionary<string, PropertyInfo> HeaderProperties = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

        static HttpWebRequestExtensions()
        {
            Type type = typeof(HttpWebRequest);
            foreach (string header in RestrictedHeaders)
            {
                string propertyName = header.Replace("-", "");
                PropertyInfo headerProperty = type.GetProperty(propertyName);
                HeaderProperties[header] = headerProperty;
            }
        }

        public static void SetRawHeader(this HttpWebRequest request, string name, string value)
        {
            if (HeaderProperties.ContainsKey(name))
            {
                PropertyInfo property = HeaderProperties[name];
                if (property.PropertyType == typeof(DateTime))
                    property.SetValue(request, DateTime.Parse(value), null);
                else if (property.PropertyType == typeof(bool))
                    property.SetValue(request, Boolean.Parse(value), null);
                else if (property.PropertyType == typeof(long))
                    property.SetValue(request, Int64.Parse(value), null);
                else
                    property.SetValue(request, value, null);
            }
            else
            {
                request.Headers[name] = value;
            }
        }
    }
    */

    public class RemoteUnityLRS : ILRS
    {


        public Uri endpoint { get; set; }
        public TCAPIVersion version { get; set; }
        public String auth { get; set; }
        public Dictionary<String, String> extended { get; set; }

        public void SetAuth(String username, String password)
        {
            auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
        }

        public RemoteUnityLRS() { }
        public RemoteUnityLRS(Uri endpoint, TCAPIVersion version, String username, String password)
        {
            this.endpoint = endpoint;
            this.version = version;
            this.SetAuth(username, password);
        }
        public RemoteUnityLRS(String endpoint, TCAPIVersion version, String username, String password) : this(new Uri(endpoint), version, username, password) { }
        public RemoteUnityLRS(String endpoint, String username, String password) : this(endpoint, TCAPIVersion.latest(), username, password) { }

        private class MyHTTPRequest
        {
            public String method { get; set; }
            public String resource { get; set; }
            public Dictionary<String, String> queryParams { get; set; }
            public Dictionary<String, String> headers { get; set; }
            public String contentType { get; set; }
            public byte[] content { get; set; }
        }

        private class MyHTTPResponse
        {
            public HttpStatusCode status { get; set; }
            public String contentType { get; set; }
            public byte[] content { get; set; }
            public DateTime lastModified { get; set; }
            public String etag { get; set; }
            public Exception ex { get; set; }

            public MyHTTPResponse() { }
            public MyHTTPResponse(HttpWebResponse webResp)
            {
                status = webResp.StatusCode;
                contentType = webResp.ContentType;
                // Replaced: etag = webResp.Headers.Get("Etag");  // Unity build error
                etag = webResp.Headers["Etag"];

                // Replaced:
                // lastModified = webResp.LastModified; // Unity build error
                // ... with ...
                lastModified = Convert.ToDateTime( webResp.Headers["Last-Modified"] );

                using (var stream = webResp.GetResponseStream())
                {
                    content = ReadFully(stream, Int32.Parse(webResp.Headers["Content-Length"]));
                }
            }


            public MyHTTPResponse(UnityWebRequest webReq)
            {
                status = (HttpStatusCode)webReq.responseCode;
                contentType = webReq.GetResponseHeader("Content-type");
                etag = webReq.GetResponseHeader("Etag");
                lastModified = Convert.ToDateTime(webReq.GetResponseHeader("Last-Modified"));

                content = webReq.downloadHandler.data;

              //  webReq.
              //  using (var stream = webResp.GetResponseStream())
              //  {
             //       content = ReadFully(stream, Int32.Parse(webResp.Headers["Content-Length"]));
             //   }

            }
        }

        /*
        IEnumerator Post(UnityWebRequest request)
        {

            yield return request.SendWebRequest();
         }//*/

        /*
        IEnumerator MakeASyncRequest(MyHTTPRequest req)
        {
            String url;
            // Replaced: if (req.resource.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)) // Unity build error
            // With.... 
            if (req.resource.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                url = req.resource;
            }
            else
            {
                url = endpoint.ToString();
                if (!url.EndsWith("/") && !req.resource.StartsWith("/")) {
                    url += "/";
                }
                url += req.resource;
            }
            ///////////
            ///////////
            ///////////
            if (req.queryParams != null)
            {
                String qs = "";
                foreach (KeyValuePair<String, String> entry in req.queryParams)
                {
                    if (qs != "")
                    {
                        qs += "&";
                    }
                    qs += WebUtility.UrlEncode(entry.Key) + "=" + WebUtility.UrlEncode(entry.Value);
                }
                if (qs != "")
                {
                    url += "?" + qs;
                }
            }
            /////////
            ////////
            ////////
            // TODO: handle special properties we recognize, such as content type, modified since, etc.
            var unityWebReq = new UnityWebRequest();
            unityWebReq.url = url;

            unityWebReq.method = req.method;
            unityWebReq.SetRequestHeader("X-Experience-API-Version", version.ToString());
            
            if (auth != null)
            {
                unityWebReq.SetRequestHeader("Authorization", auth);
            }
            if (req.headers != null)
            {
                foreach (KeyValuePair<String, String> entry in req.headers)
                {
                    unityWebReq.SetRequestHeader(entry.Key, entry.Value);
                }
            }

            if (req.contentType != null)
            {
                unityWebReq.SetRequestHeader("Content-Type", req.contentType);
            }
            else
            {
                unityWebReq.SetRequestHeader("Content-type", "application/octet-stream");
            }
          
            if (req.content != null)
            {
                unityWebReq.SetRequestHeader("content-length", req.content.Length.ToString());

                unityWebReq.uploadHandler = (UploadHandler)new UploadHandlerRaw(req.content);
                //MyClass.Instance.StartCoroutine(Post(unityWebReq));

                Debug.Log("Hello world");
                unityWebReq.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                //  return test.SendWebRequest();
                unityWebReq.chunkedTransfer = false;
                Debug.Log("okay world");
                yield return unityWebReq.SendWebRequest();


                Debug.Log("bye world");
                Debug.Log("X-S-S-Status Code: " + unityWebReq.responseCode);

       
            }

            MyHTTPResponse resp;

            try
            {
                /*
            
            using (var webResp = (HttpWebResponse)webReq.GetResponse())  // Unity build error
            {
                resp = new MyHTTPResponse(webResp);
            }
            
            // 
               // resp = null;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var webResp = (HttpWebResponse)ex.Response)
                    {
                        resp = new MyHTTPResponse(webResp);
                    }
                }
                else
                {
                    resp = new MyHTTPResponse();
                    resp.content = Encoding.UTF8.GetBytes("Web exception without '.Response'");
                }
                resp.ex = ex;
            } //  
            resp = null;
           // return resp;
        }
        */


        private MyHTTPResponse MakeSyncRequest(MyHTTPRequest req)
        {
            String url;
            // Replaced: if (req.resource.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)) // Unity build error
            // With.... 
            if (req.resource.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                url = req.resource;
            }
            else
            {
                url = endpoint.ToString();
                if (!url.EndsWith("/") && !req.resource.StartsWith("/"))
                {
                    url += "/";
                }
                url += req.resource;
            }
            ///////////
            ///////////
            ///////////
            if (req.queryParams != null)
            {
                String qs = "";
                foreach (KeyValuePair<String, String> entry in req.queryParams)
                {
                    if (qs != "")
                    {
                        qs += "&";
                    }
                    qs += WebUtility.UrlEncode(entry.Key) + "=" + WebUtility.UrlEncode(entry.Value);
                }
                if (qs != "")
                {
                    url += "?" + qs;
                }
            }

            /////////
            ////////
            ////////
            // TODO: handle special properties we recognize, such as content type, modified since, etc.
            var unityWebReq = new UnityWebRequest();
            unityWebReq.url = url;

            unityWebReq.method = req.method;
            unityWebReq.SetRequestHeader("X-Experience-API-Version", version.ToString());

            if (auth != null)
            {
                unityWebReq.SetRequestHeader("Authorization", auth);
            }
            if (req.headers != null)
            {
                foreach (KeyValuePair<String, String> entry in req.headers)
                {
                    unityWebReq.SetRequestHeader(entry.Key, entry.Value);
                }
            }

            if (req.contentType != null)
            {
                unityWebReq.SetRequestHeader("Content-Type", req.contentType);
            }
            else
            {
                unityWebReq.SetRequestHeader("Content-type", "application/octet-stream");
            }

            if (req.content != null)
            {
                unityWebReq.SetRequestHeader("content-length", req.content.Length.ToString());

                unityWebReq.uploadHandler = (UploadHandler)new UploadHandlerRaw(req.content);
                unityWebReq.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                unityWebReq.chunkedTransfer = false; // Is this needed? 
                unityWebReq.SendWebRequest();

                while (!unityWebReq.isDone) /// erghhhh, is this safe? Make sure there is a timeout attached in this class somewhere...
                {
                    // Wait. Makes it sync in Unity. Think this is fineeeee
                }
            }


            // Do we need to use a "while(!request.downloadHandler.isDone)" ????
            MyHTTPResponse resp = new MyHTTPResponse(unityWebReq);

            // LAAR TODO: Ensure that all functionality / safety guards from below are contained.

            /*
            try
            {
            
            using (var webResp = (HttpWebResponse)webReq.GetResponse())  // Unity build error
            {
                resp = new MyHTTPResponse(webResp);
            }
            
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var webResp = (HttpWebResponse)ex.Response)
                    {
                        resp = new MyHTTPResponse(webResp);
                    }
                }
                else
                {
                    resp = new MyHTTPResponse();
                    resp.content = Encoding.UTF8.GetBytes("Web exception without '.Response'");
                }
                resp.ex = ex;
            } 
            // */

            return resp;
        }

        /// <summary>
        /// See http://www.yoda.arachsys.com/csharp/readbinary.html no license found
        /// 
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>/
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        private static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        private MyHTTPResponse GetDocument(String resource, Dictionary<String, String> queryParams, Document document)
        {
            var req = new MyHTTPRequest();
            req.method = "GET";
            req.resource = resource;
            req.queryParams = queryParams;

            var res = MakeSyncRequest(req);
            if (res.status == HttpStatusCode.OK)
            {
                document.content = res.content;
                document.contentType = res.contentType;
                document.timestamp = res.lastModified;
                document.etag = res.etag;
            }

            return res;
        }

        private ProfileKeysLRSResponse GetProfileKeys(String resource, Dictionary<String, String> queryParams)
        {
            var r = new ProfileKeysLRSResponse();

            var req = new MyHTTPRequest();
            req.method = "GET";
            req.resource = resource;
            req.queryParams = queryParams;

            var res = MakeSyncRequest(req);
            if (res.status != HttpStatusCode.OK)
            {
                r.success = false;
                r.httpException = res.ex;
                r.SetErrMsgFromBytes(res.content);
                return r;
            }

            r.success = true;

            var keys = JArray.Parse(Encoding.UTF8.GetString(res.content));
            if (keys.Count > 0) {
                r.content = new List<String>();
                foreach (JToken key in keys) {
                    r.content.Add((String)key);
                }
            }

            return r;
        }

        private LRSResponse SaveDocument(String resource, Dictionary<String, String> queryParams, Document document)
        {
            var r = new LRSResponse();

            var req = new MyHTTPRequest();
            req.method = "PUT";
            req.resource = resource;
            req.queryParams = queryParams;
            req.contentType = document.contentType;
            req.content = document.content;

            var res = MakeSyncRequest(req);
            if (res.status != HttpStatusCode.NoContent)
            {
                r.success = false;
                r.httpException = res.ex;
                r.SetErrMsgFromBytes(res.content);
                return r;
            }

            r.success = true;

            return r;
        }

        private LRSResponse DeleteDocument(String resource, Dictionary<String, String> queryParams)
        {
            var r = new LRSResponse();

            var req = new MyHTTPRequest();
            req.method = "DELETE";
            req.resource = resource;
            req.queryParams = queryParams;

            var res = MakeSyncRequest(req);
            if (res.status != HttpStatusCode.NoContent)
            {
                r.success = false;
                r.httpException = res.ex;
                r.SetErrMsgFromBytes(res.content);
                return r;
            }

            r.success = true;

            return r;
        }

        private StatementLRSResponse GetStatement(Dictionary<String, String> queryParams)
        {
            var r = new StatementLRSResponse();

            var req = new MyHTTPRequest();
            req.method = "GET";
            req.resource = "statements";
            req.queryParams = queryParams;

            var res = MakeSyncRequest(req);
            if (res.status != HttpStatusCode.OK)
            {
                r.success = false;
                r.httpException = res.ex;
                r.SetErrMsgFromBytes(res.content);
                return r;
            }

            r.success = true;
            r.content = new Statement(new Json.StringOfJSON(Encoding.UTF8.GetString(res.content)));

            return r;
        }

        public AboutLRSResponse About()
        {
            var r = new AboutLRSResponse();

            var req = new MyHTTPRequest();
            req.method = "GET";
            req.resource = "about";

            var res = MakeSyncRequest(req);
            if (res.status != HttpStatusCode.OK)
            {
                r.success = false;
                r.httpException = res.ex;
                r.SetErrMsgFromBytes(res.content);
                return r;
            }

            r.success = true;
            r.content = new About(Encoding.UTF8.GetString(res.content));

            return r;
        }
    
        public StatementLRSResponse SaveStatement(Statement statement)
        {
            var r = new StatementLRSResponse();
            var req = new MyHTTPRequest();
            req.queryParams = new Dictionary<String, String>();
            req.resource = "statements";

            if (statement.id == null)
            {
                req.method = "POST";
            }
            else
            {
                req.method = "PUT";
                req.queryParams.Add("statementId", statement.id.ToString());
            }

            req.contentType = "application/json";
            req.content = Encoding.UTF8.GetBytes(statement.ToJSON(version));

            var res = MakeSyncRequest(req);

            if (statement.id == null)
            {
                if (res.status != HttpStatusCode.OK)
                {
                    r.success = false;
                    r.httpException = res.ex;
                    r.SetErrMsgFromBytes(res.content);
                    return r;
                }

                var ids = JArray.Parse(Encoding.UTF8.GetString(res.content));
                statement.id = new Guid((String)ids[0]);
            }
            else {
                if (res.status != HttpStatusCode.NoContent)
                {
                    r.success = false;
                    r.httpException = res.ex;
                    r.SetErrMsgFromBytes(res.content);
                    return r;
                }
            }

            r.success = true;
            r.content = statement;

            return r;
        }

        public StatementLRSResponse VoidStatement(Guid id, Agent agent)
        {
            var voidStatement = new Statement
            {
                actor = agent,
                verb = new Verb
                {
                    id = new Uri("http://adlnet.gov/expapi/verbs/voided"),
                    display = new LanguageMap()
                },
                target = new StatementRef { id = id }
            };
            voidStatement.verb.display.Add("en-US", "voided");

            return SaveStatement(voidStatement);
        }

        public StatementsResultLRSResponse SaveStatements(List<Statement> statements)
        {
            var r = new StatementsResultLRSResponse();

            var req = new MyHTTPRequest();
            req.resource = "statements";
            req.method = "POST";
            req.contentType = "application/json";

            var jarray = new JArray();
            foreach (Statement st in statements)
            {
                jarray.Add(st.ToJObject(version));
            }
            req.content = Encoding.UTF8.GetBytes(jarray.ToString());

            var res = MakeSyncRequest(req);
            if (res.status != HttpStatusCode.OK)
            {
                r.success = false;
                r.httpException = res.ex;
                r.SetErrMsgFromBytes(res.content);
                return r;
            }

            var ids = JArray.Parse(Encoding.UTF8.GetString(res.content));
            for (int i = 0; i < ids.Count; i++)
            {
                statements[i].id = new Guid((String)ids[i]);
            }

            r.success = true;
            r.content = new StatementsResult(statements);

            return r;
        }
        public StatementLRSResponse RetrieveStatement(Guid id)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("statementId", id.ToString());

            return GetStatement(queryParams);
        }
        public StatementLRSResponse RetrieveVoidedStatement(Guid id)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("voidedStatementId", id.ToString());

            return GetStatement(queryParams);
        }
        public StatementsResultLRSResponse QueryStatements(StatementsQuery query)
        {
            var r = new StatementsResultLRSResponse();

            var req = new MyHTTPRequest();
            req.method = "GET";
            req.resource = "statements";
            req.queryParams = query.ToParameterMap(version);

            var res = MakeSyncRequest(req);
            if (res.status != HttpStatusCode.OK)
            {
                r.success = false;
                r.httpException = res.ex;
                r.SetErrMsgFromBytes(res.content);
                return r;
            }

            r.success = true;
            r.content = new StatementsResult(new Json.StringOfJSON(Encoding.UTF8.GetString(res.content)));

            return r;
        }
        public StatementsResultLRSResponse MoreStatements(StatementsResult result)
        {
            var r = new StatementsResultLRSResponse();

            var req = new MyHTTPRequest();
            req.method = "GET";

            // Replaced:
            // req.resource = endpoint.GetLeftPart(UriPartial.Authority);  // Unity build error

            req.resource = "http";

            if (! req.resource.EndsWith("/")) {
                req.resource += "/";
            }
            req.resource += result.more;

            var res = MakeSyncRequest(req);
            if (res.status != HttpStatusCode.OK)
            {
                r.success = false;
                r.httpException = res.ex;
                r.SetErrMsgFromBytes(res.content);
                return r;
            }

            r.success = true;
            r.content = new StatementsResult(new Json.StringOfJSON(Encoding.UTF8.GetString(res.content)));

            return r;
        }

        // TODO: since param
        public ProfileKeysLRSResponse RetrieveStateIds(Activity activity, Agent agent, Nullable<Guid> registration = null)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("activityId", activity.id.ToString());
            queryParams.Add("agent", agent.ToJSON(version));
            if (registration != null)
            {
                queryParams.Add("registration", registration.ToString());
            }

            return GetProfileKeys("activities/state", queryParams);
        }
        public StateLRSResponse RetrieveState(String id, Activity activity, Agent agent, Nullable<Guid> registration = null)
        {
            var r = new StateLRSResponse();

            var queryParams = new Dictionary<String, String>();
            queryParams.Add("stateId", id);
            queryParams.Add("activityId", activity.id.ToString());
            queryParams.Add("agent", agent.ToJSON(version));

            var state = new StateDocument();
            state.id = id;
            state.activity = activity;
            state.agent = agent;

            if (registration != null)
            {
                queryParams.Add("registration", registration.ToString());
                state.registration = registration;
            }

            var resp = GetDocument("activities/state", queryParams, state);
            if (resp.status != HttpStatusCode.OK && resp.status != HttpStatusCode.NotFound)
            {
                r.success = false;
                r.httpException = resp.ex;
                r.SetErrMsgFromBytes(resp.content);
                return r;
            }
            r.success = true;
            r.content = state;

            return r;
        }
        public LRSResponse SaveState(StateDocument state)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("stateId", state.id);
            queryParams.Add("activityId", state.activity.id.ToString());
            queryParams.Add("agent", state.agent.ToJSON(version));
            if (state.registration != null)
            {
                queryParams.Add("registration", state.registration.ToString());
            }

            return SaveDocument("activities/state", queryParams, state);
        }
        public LRSResponse DeleteState(StateDocument state)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("stateId", state.id);
            queryParams.Add("activityId", state.activity.id.ToString());
            queryParams.Add("agent", state.agent.ToJSON(version));
            if (state.registration != null)
            {
                queryParams.Add("registration", state.registration.ToString());
            }

            return DeleteDocument("activities/state", queryParams);
        }
        public LRSResponse ClearState(Activity activity, Agent agent, Nullable<Guid> registration = null)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("activityId", activity.id.ToString());
            queryParams.Add("agent", agent.ToJSON(version));
            if (registration != null)
            {
                queryParams.Add("registration", registration.ToString());
            }

            return DeleteDocument("activities/state", queryParams);
        }

        // TODO: since param
        public ProfileKeysLRSResponse RetrieveActivityProfileIds(Activity activity)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("activityId", activity.id.ToString());

            return GetProfileKeys("activities/profile", queryParams);
        }
        public ActivityProfileLRSResponse RetrieveActivityProfile(String id, Activity activity)
        {
            var r = new ActivityProfileLRSResponse();

            var queryParams = new Dictionary<String, String>();
            queryParams.Add("profileId", id);
            queryParams.Add("activityId", activity.id.ToString());

            var profile = new ActivityProfileDocument();
            profile.id = id;
            profile.activity = activity;

            var resp = GetDocument("activities/profile", queryParams, profile);
            if (resp.status != HttpStatusCode.OK && resp.status != HttpStatusCode.NotFound)
            {
                r.success = false;
                r.httpException = resp.ex;
                r.SetErrMsgFromBytes(resp.content);
                return r;
            }
            r.success = true;
            r.content = profile;

            return r;
        }
        public LRSResponse SaveActivityProfile(ActivityProfileDocument profile)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("profileId", profile.id);
            queryParams.Add("activityId", profile.activity.id.ToString());

            return SaveDocument("activities/profile", queryParams, profile);
        }
        public LRSResponse DeleteActivityProfile(ActivityProfileDocument profile)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("profileId", profile.id);
            queryParams.Add("activityId", profile.activity.id.ToString());
            // TODO: need to pass Etag?

            return DeleteDocument("activities/profile", queryParams);
        }

        // TODO: since param
        public ProfileKeysLRSResponse RetrieveAgentProfileIds(Agent agent)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("agent", agent.ToJSON(version));

            return GetProfileKeys("agents/profile", queryParams);
        }
        public AgentProfileLRSResponse RetrieveAgentProfile(String id, Agent agent)
        {
            var r = new AgentProfileLRSResponse();

            var queryParams = new Dictionary<String, String>();
            queryParams.Add("profileId", id);
            queryParams.Add("agent", agent.ToJSON(version));

            var profile = new AgentProfileDocument();
            profile.id = id;
            profile.agent = agent;

            var resp = GetDocument("agents/profile", queryParams, profile);
            if (resp.status != HttpStatusCode.OK && resp.status != HttpStatusCode.NotFound)
            {
                r.success = false;
                r.httpException = resp.ex;
                r.SetErrMsgFromBytes(resp.content);
                return r;
            }
            r.success = true;
            r.content = profile;

            return r;
        }
        public LRSResponse SaveAgentProfile(AgentProfileDocument profile)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("profileId", profile.id);
            queryParams.Add("agent", profile.agent.ToJSON(version));

            return SaveDocument("agents/profile", queryParams, profile);
        }
        public LRSResponse DeleteAgentProfile(AgentProfileDocument profile)
        {
            var queryParams = new Dictionary<String, String>();
            queryParams.Add("profileId", profile.id);
            queryParams.Add("agent", profile.agent.ToJSON(version));
            // TODO: need to pass Etag?

            return DeleteDocument("agents/profile", queryParams);
        }
    }
}
