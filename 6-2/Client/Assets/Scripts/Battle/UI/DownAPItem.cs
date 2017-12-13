

using UnityEngine;
using UnityEngine.UI;

public class DownAPItem : MonoBehaviour
{
    Image icon;
    public void Init()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
    }

    /// <summary>
    /// 0 colose
    /// 1 green 有
    /// 2 red   消耗
    /// 3 grey  空
    /// </summary>
    /// <param name="state"></param>
    public void upddate(int state)
    {
        if (state == 0)
        {
            Close();
            return;
        }
        Open();
        switch (state)
        {
            case 1:
                icon.color = Color.green;
                break;
            case 2:
                icon.color = Color.red;
                break;
            case 3:
                icon.color = Color.grey;
                break;
        }
    }

    public void Close() { gameObject.SetActive(false); }
    public void Open()
    {
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);
    }
}