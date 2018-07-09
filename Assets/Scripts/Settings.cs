using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour { 
   /* public static string POSTAddUserURL = "https://competenceanalytics.com/data/xAPI/statements";
    public static string AUTHORIZATION_KEY = "MjgxOTUyZjAzZDRlNDU1NDBmYTY3ZTQ2M2ZmZGJhZGE1NDViMjg3NTo1MTVhN2I2OWNlYjdkZTAyYWU4Yzk2ZTA1ODk3M2YwMmQ5ZTI5N2I0";

    public static Settings instance = null;
    public string tableToLoad;
    public string currentScenario = "";
    public string gameScene = "DiningTable";
    public string mainMenuScene = "MainScene";
    public static string username = "";
    public static string email="";
    public static bool showEmailGUI = true;
    private XAPIStatement statement;
    public static List<string> languageBase;
    public ExtensionMethod.Language  currentLang = ExtensionMethod.Language.en;
    public  bool uniqueUsers = true;
    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            if(instance.uniqueUsers) PlayerPrefs.DeleteAll();
            if (PlayerPrefs.GetString("Name").Length > 0 && PlayerPrefs.GetString("Email").Length > 0)
            {
                username = PlayerPrefs.GetString("Name");
                email = PlayerPrefs.GetString("Email");
                XAPIStatement statement = new XAPIStatement(username, "mailto:" + email, "started", "http:∕∕adlnet.gov∕expapi∕verbs∕initialized", "http:∕∕adlnet.gov∕expapi∕activities∕DinnerTable", "Dinner Table", "Started Dinner Table");
                SEND(statement);
            }
            else
            {
                GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>().enabled = false;
                GameObject.FindGameObjectWithTag("UsernameCanvas").GetComponent<Canvas>().enabled = true;
            }
            
        }
        else if (instance != this)

            Destroy(gameObject);
        InitLang();
        DontDestroyOnLoad(gameObject);

    }
    private void InitLang()
    {
        GameObject langMenuObj = GameObject.FindGameObjectWithTag("LanguageMenu");
        
        LanguageButton currentlyFocused = langMenuObj.transform.GetChild(0).GetComponent<LanguageButton>();
        if (instance.currentLang != currentlyFocused.buttonLanguage)
        {
            for (int i = 0; i < langMenuObj.transform.GetChild(1).transform.childCount; i++)
            {
                if (langMenuObj.transform.GetChild(1).GetChild(i).GetComponent<LanguageButton>().buttonLanguage == instance.currentLang)
                {

                    GameObject replacementLang = langMenuObj.transform.GetChild(1).GetChild(i).gameObject;
                    ExtensionMethod.Language oldLang = currentlyFocused.buttonLanguage;
                    Sprite oldTexture = currentlyFocused.GetComponent<Image>().sprite;

                    currentlyFocused.GetComponent<LanguageButton>().buttonLanguage = replacementLang.GetComponent<LanguageButton>().buttonLanguage;
                    currentlyFocused.GetComponent<Image>().sprite = replacementLang.GetComponent<Image>().sprite;

                    replacementLang.GetComponent<LanguageButton>().buttonLanguage = oldLang;
                    replacementLang.GetComponent<Image>().sprite = oldTexture;
                    break;
                }
            }
        }
        langMenuObj.transform.GetChild(1).gameObject.SetActive(false);
    }
    bool once = true;
    public void ChangeLanguage()
    {
        Text[] allTextObjects = GameObject.FindObjectsOfType<Text>();
        if (ExtensionMethod.currentLanguage != ExtensionMethod.Language.en && once)
        {
            languageBase = new List<string>();
            once = false;
            foreach (Text t in GameObject.FindObjectsOfType<Text>())
                languageBase.Add(t.text);
        }

        for (int i=0; i< allTextObjects.Length; i++)
        {
            allTextObjects[i].text = languageBase[i].Translate();
        }
        currentLang = ExtensionMethod.currentLanguage;
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
    */
}
