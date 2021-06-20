using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Users
{
    public int page;
    public int per_page;
    public int total;
    public int total_pages;
    public List<Datum> data;
    public Support support;
}

[System.Serializable]
public class Datum
{
    public int id;
    public string email;
    public string first_name;
    public string last_name;
    public string avatar;
}
[System.Serializable]
public class Support
{
    public string url;
    public string text;
}

