using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    public Text massgeError;
    void Start()
    {
        _auth = this;
    }
//Action Button login
    public void Login()
    {
        if (_email.text == "" || _pass.text == "")
        {
            StartCoroutine(ShowError("Fill in the fields "));
            return;
            
        }
        _loading.SetActive(true);
        StartCoroutine(makeRequest());
    }

    IEnumerator ShowError(string error)
    {
        massgeError.text = error;
        yield return  new WaitForSeconds(2);
        massgeError.text = "";
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
        WWWForm form = new WWWForm();
        form.AddField("email", _email.text.Trim());
        form.AddField("password", _pass.text.Trim());
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        www.SetRequestHeader("token", "QpwL5tke4Pnpja7X4");
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
           _loading.SetActive(false);
            StartCoroutine(ShowError(www.error));
        }
        else
        {
            StartCoroutine(Getuser());
            
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