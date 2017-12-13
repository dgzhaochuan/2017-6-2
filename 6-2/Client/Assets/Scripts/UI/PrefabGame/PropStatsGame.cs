
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 装备 道具格子
    /// </summary>
   public class PropStatsGame:MonoBehaviour
    {
        Image IconImage;
        Text NumberText;
        public int index;
        BaseAdditionalAttribute _attribute;
        UnityAction<BaseAdditionalAttribute, int> click;
        public BaseAdditionalAttribute Attribute
        {
            get { return _attribute; }
            set
            {
                _attribute = value;
                OnUpdate();
            }
        }

        void Awake()
        {
            IconImage = transform.Find("icon").GetComponent<Image>();
            NumberText = transform.Find("number").GetComponent<Text>();
            IconImage.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public void OnInit(UnityAction<BaseAdditionalAttribute,int> click,int index)
        {
            this.click = click;
            this.index = index;
        }
        void OnUpdate()
        {
            if (gameObject.activeSelf == false)
                gameObject.SetActive(true);
            if (_attribute != null)
            {
                if (IconImage.gameObject.activeSelf == false)
                    IconImage.gameObject.SetActive(true);
                IconImage.sprite = Manage.Instance.AB.GetPropSprite(_attribute);
                if (_attribute.maxNumber > 1)
                {
                    NumberText.gameObject.SetActive(true);
                    NumberText.text = _attribute.number + "/" + _attribute.maxNumber;
                }
                else
                {
                    NumberText.gameObject.SetActive(false);
                }
            }
            else
            {
                if (IconImage.gameObject.activeSelf)
                    IconImage.gameObject.SetActive(false);
                IconImage.sprite = null;
                NumberText.gameObject.SetActive(false);
            }
        }
        void OnClick()
        {
            click(Attribute,index);
        }
    }
}
