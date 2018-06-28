using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    // Use this for initialization
    public bool uniqueUsers = false;
    public InputField username, email;
	void Start () 
    {

        Debug.Log("Name: " + PlayerPrefs.GetString("Name"));
        Debug.Log("Email: " + PlayerPrefs.GetString("Email"));
        if (uniqueUsers) PlayerPrefs.DeleteAll();

        if (PlayerPrefs.GetString("Name").Length > 0 && PlayerPrefs.GetString("Email").Length > 0)
        {
            username.text = PlayerPrefs.GetString("Name");
            email.text = PlayerPrefs.GetString("Email");
        }


		
	}

    public void Submit()
    {
        PlayerPrefs.SetString("Name", username.text);
        PlayerPrefs.SetString("Email", email.text);

    }

}
