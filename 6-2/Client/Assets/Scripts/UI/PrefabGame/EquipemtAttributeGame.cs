using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 装备道具 属性显示条
    /// </summary>
    public class EquipemtAttributeGame : MonoBehaviour
    {
        Text nameText;
        Text valueText;
        //Image icon;
        GameObject upIcon;
        GameObject reduceIcon;


       public void mAwake()
        {
            nameText = transform.Find("name").GetComponent<Text>();
            valueText = transform.Find("value").GetComponent<Text>();
            //icon= transform.Find("icon/icon").GetComponent<Image>();
            upIcon = transform.Find("up/up").gameObject;
            reduceIcon = transform.Find("reduce/reduce").gameObject;
        }
        public void OnUpdate(string name, string value)
        {
            Open();
            nameText.text = name;
            valueText.text = value;
            valueText.gameObject.SetActive(true);
            upIcon.SetActive(false);
            reduceIcon.SetActive(false);
        }
        public void OnUpdate(string name, string value, bool up)
        {
            Open();
            nameText.text = name;
            valueText.gameObject.SetActive(true);
            valueText.text = value;
            valueText.color = up ? Config.UpColor : Config.DownColor;
            upIcon.SetActive(up);
            reduceIcon.SetActive(!up);
        }
        public void OnUpdate(string data)
        {
            Open();
            nameText.text = data;
            valueText.gameObject.SetActive(false);
            upIcon.SetActive(false);
            reduceIcon.SetActive(false);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }

    }
}
