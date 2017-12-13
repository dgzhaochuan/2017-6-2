using UnityEngine;
using UnityEngine.EventSystems;


    public abstract class Panel : MonoBehaviour
    {
        private Tweener _tween;
        public Tweener Tweener
        {
            get
            {               
                return _tween;
            }
        }
        public bool IsOpen { get { return Tweener.IsOpen; } }
        public abstract void OnUpdate();
        /// <summary>
        /// 打开其他界面的时候是否要关闭这个界面
        /// 切换界面的时候关闭掉一些上一个界面的临时界面，比如输入数字
        /// </summary>
        protected virtual bool ChangeClose { get { return false; } }
        /// <summary>
        /// 点击到非panel的地方的时候是否要关闭panel
        /// </summary>
        protected virtual bool ClickClose { get { return false; } }
        /// <summary>
        /// 这个界面打开的时候是否要关闭其他界面
        /// </summary>
        protected virtual bool IsOnly { get { return true; } }
        public virtual void mAwake()
        {
            if (ChangeClose)
                PanelManager.Instantiate.AddChageClosePanel(this);
            StartTweener();
        }
        public virtual void Open(float delay = 0)
        {
            gameObject.SetActive(true);
            ChangePanel();
            Tweener.delay = delay;
            Tweener.OnOpen();
            transform.SetSiblingIndex(Config.SiblingIndex);
        }
        public virtual void Close(float delay)
        {
            Tweener.OnClose(delay);
        }
        public virtual void Change()
        {
            if (IsOpen)
                Close();
            else
                Open();
        }
        public void Close()
        {
            Close(0);
        }
        void StartTweener()
        {
            if (_tween == null)
            {
                _tween = GetComponent<Tweener>();
                if (_tween == null)
                    _tween = gameObject.AddComponent<TweenerAlpha>();
            }
            _tween.OnOpenEvent = OnUpdate;
            _tween.OnCloseEndEvent = delegate () { gameObject.SetActive(false); };
        }
        void ChangePanel()
        {
            if (IsOnly)
                PanelManager.Instantiate.ChangePanel();
        }
        bool ContainsPos(Vector2 pos)
        {
            
               RectTransform transform = (this.transform as RectTransform);
            Rect rect = transform.rect;
            rect.x = Screen.width * transform.pivot.x + transform.anchoredPosition.x - transform.sizeDelta.x / 2;
            rect.y = Screen.height * transform.pivot.y + transform.anchoredPosition.y + transform.sizeDelta.y / 2;
            bool b = (pos.x >= rect.x && pos.x <= rect.x + rect.width)
                && (pos.y <= rect.y && pos.y >= rect.y - rect.height);
            if (!b) Close();
            return b;
        }        
        void OnEnable()
        {
            if (ClickClose)
                EventSystem.current.AddDown(ContainsPos);
        }
        void OnDisable()
        {

            if (ClickClose)
                EventSystem.current.ReDown(ContainsPos);
        }

    }

