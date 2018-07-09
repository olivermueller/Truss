using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

    public List<TargetElement> targets;
    public GameObject currentlyActive;
    public Canvas canvas, usernameCanvas;

    public static string POSTAddUserURL = "https://competenceanalytics.com/data/xAPI/statements";
    public static string AUTHORIZATION_KEY = "MjgxOTUyZjAzZDRlNDU1NDBmYTY3ZTQ2M2ZmZGJhZGE1NDViMjg3NTo1MTVhN2I2OWNlYjdkZTAyYWU4Yzk2ZTA1ODk3M2YwMmQ5ZTI5N2I0";
    public string email;
    public string username;
    public bool uniqueUsers = true;

    private static TargetManager _instance;
    public static TargetManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {

            _instance = this;
            targets = new List<TargetElement>();
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("CylinderTarget"))
                targets.Add(g.GetComponent<TargetElement>());
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("ImageTarget"))
                targets.Add(g.GetComponent<TargetElement>());
            //canvas = GameObject.FindObjectOfType<Canvas>();
        }

        username = PlayerPrefs.GetString("Name");
        email = PlayerPrefs.GetString("Email");

    }

    public WWW SEND(XAPIStatement statement)
    {
        string json = JsonUtility.ToJson(statement);
        json = json.Replace("_", string.Empty);
        Debug.Log(json.ToString());
        WWW www;
        Dictionary<string, string> postHeader = new Dictionary<string, string>();
        postHeader.Add("Content-Type", "application/json");
        postHeader.Add("Authorization", "Basic " + AUTHORIZATION_KEY);
        postHeader.Add("X-Experience-API-Version", "1.0.1");
        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        www = new WWW(POSTAddUserURL, formData, postHeader);
        StartCoroutine(WaitForRequest(www));
        return www;
    }

    IEnumerator WaitForRequest(WWW data)
    {
        yield return data;

    }

    
}
