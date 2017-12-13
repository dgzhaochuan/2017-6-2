using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace UI
{
    /// <summary>
    /// 输入数量
    /// </summary>
    public class InputNumberPanel : Panel
    {
        Text moneyText;
        InputField numberInput;
        Action<int> EnterEvent;
        int max;
        int min = 1;
        int number = 1;
        int price;
        Button EnterBtn;

        protected override bool ChangeClose
        {
            get
            {
                return true;
            }
        }
        protected override bool IsOnly
        {
            get
            {
                return false;
            }
        }
        public override void mAwake()
        {
            base.mAwake();
            EnterBtn = transform.Find("Button/Enter").GetComponent<Button>();
            EnterBtn.onClick.AddListener(Enter);
            numberInput = transform.Find("Input/Input").GetComponent<InputField>();
            numberInput.onValueChanged.AddListener(onValueChanged);
            moneyText = transform.Find("Input/money").GetComponent<Text>();
            transform.Find("Input/Max").GetComponent<Button>().onClick.AddListener(SetMax);
            transform.Find("Input/Add").GetComponent<Button>().onClick.AddListener(Add);
            transform.Find("Input/Min").GetComponent<Button>().onClick.AddListener(SetMin);
            transform.Find("Input/Cut").GetComponent<Button>().onClick.AddListener(Cut);
            transform.Find("Button/Close").GetComponent<Button>().onClick.AddListener(close);
        }
        void SetMax()
        {
            number = max;
            OnUpdate();
        }
        void SetMin()
        {
            number = 1;
            OnUpdate();
        }
        void Add()
        {
            number++;
            number = Mathf.Clamp(number,min,max);
            OnUpdate();
        }
        void Cut()
        {
            number--;
            number = Mathf.Clamp(number, min, max);
            OnUpdate();
        }
        void Enter()
        {
            EnterEvent(int.Parse(numberInput.text));
            Close();
        }
        void close()
        {
            EnterEvent = null;
            Close();
        }
        void onValueChanged(string value)
        {
            number=int.Parse(value);
            OnUpdate();
        }
        public void Open(Action<int> EnterEvent,int max, int price=-1, int number = 1)
        {
            this.EnterEvent = EnterEvent;
            this.max = max;
            this.number = number;
            this.price = price;
            moneyText.gameObject.SetActive(price>0);
            base.Open();
        }

        public override void OnUpdate()
        {
            numberInput.text = number.ToString();
            if (price != -1)
            {
                moneyText.text = (number * price).ToString();
                EnterBtn.interactable = number * price <= SaveSprite.Model.money;
            }
        }
    }
}
