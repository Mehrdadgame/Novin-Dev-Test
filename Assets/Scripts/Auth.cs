using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class Auth : MonoBehaviour
{
    public string URL, URLUserList;

    public TMP_InputField _email, _pass;

    public GameObject PageLogin;

    public Users _users;

    public short _idPage;
    public static Auth _auth;

    public UIMananger _UiMananger;

    public GameObject ListUser;

    public GameObject _loading;
    
    void Start()
    {
        _auth = this;
    }
//Action Button login
    public void Login()
    {
        _loading.SetActive(true);
        StartCoroutine(makeRequest());
    }

    string authenticate(string username, string password)
    {
        string auth = username + ":" + password;
        auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        auth = "Basic " + auth;
        return auth;
    }

    IEnumerator makeRequest()
    {
        string authorization = authenticate(_email.text, _pass.text);
        string url = URL;


        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Login", authorization);

        yield return www.Send();
        if (www.isDone)
        {
          PageLogin.SetActive(false);
            StartCoroutine(Getuser());
         
        }
        else
        {
            print("Error");
        }
    }
//Get List user and Match with calss Users
    IEnumerator Getuser()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(URLUserList+_idPage.ToString()))
        {
            var url = URLUserList + _idPage.ToString();
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    _users = JsonUtility.FromJson<Users>(webRequest.downloadHandler.text);
                    _UiMananger.enabled = true;
                    ListUser.SetActive(true);
                    break;
            }
        }
    }
}