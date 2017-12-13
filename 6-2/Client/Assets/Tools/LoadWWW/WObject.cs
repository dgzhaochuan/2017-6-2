using UnityEngine;
using System.Collections;

public class WObject 
{
    private WWW www;

    public WObject(string url )
    {
        www = new WWW(url);
    }

    public WWW mLoad()
    {
        while (!www.isDone)
        {
            Debug.Log(www.progress);
        }
        if (!string.IsNullOrEmpty( www.error))
        {
            Debug.Log("Loading error:" + www.url + "\n" + www.error);
            return null;
        }
        else
        {
            return www;
        }
    }

    public IEnumerator Load()   //使用协程进行加载.
    {
        while (!www.isDone)
        {
            yield return 1;
        }
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Loading error:" + www.url + "\n" + www.error);
        }
    }
}
