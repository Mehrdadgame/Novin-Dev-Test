using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class UIMananger : MonoBehaviour
{
    public GameObject PrefabUI;

    public Transform Parent;

    public Datum _cloneUser;

    public GameObject PageDetails;

    public Image _avatarIcon;
    public Text _firstName,_lastName,_email;
    // Start is called before the first frame update
    void Start()
    {
        //Set Data user in UI
        for (int i = 0; i < Auth._auth._users.data.Count; i++)
        {
            GameObject prefab = Instantiate<GameObject>(PrefabUI, Parent);
            prefab.GetComponentsInChildren<Text>()[0].text = Auth._auth._users.data[i].first_name;
         
            prefab.GetComponentInChildren<SetIdUser>()._Id =(short) Auth._auth._users.data[i].id;
            StartCoroutine(GetTextureRequest(Auth._auth._users.data[i].avatar, (response) => {
                prefab.GetComponentsInChildren<Image>()[2].sprite = response;
            
              
            })); 
            //Add action to Button Details
            prefab.GetComponentInChildren<Button>().onClick.AddListener(delegate
                {
                    PageDetails.SetActive(true);
                    _cloneUser = Auth._auth._users.data.Find(r =>
                        r.id == (short) prefab.GetComponentInChildren<SetIdUser>()._Id);
                    _firstName.text = _cloneUser.first_name;
                    _lastName.text = _cloneUser.last_name;
                    _email.text = _cloneUser.email;
                    _avatarIcon.sprite = prefab.GetComponentsInChildren<Image>()[2].sprite;
                });
            Auth._auth._loading.SetActive(false);
        }   
              
       
    }

  

    IEnumerator GetTextureRequest(string url, System.Action<Sprite> callback)
    {
        using (var www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    var texture = DownloadHandlerTexture.GetContent(www);
                    var rect = new Rect(0, 0, texture.width, texture.height);
                    var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                    callback(sprite);
                }
            }
        }
    }



}
