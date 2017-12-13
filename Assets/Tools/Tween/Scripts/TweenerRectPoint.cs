
using UnityEngine;


    public class TweenerRectPoint : TweenerVector
    {
        RectTransform _transform;
        new RectTransform transform
        {
            get
            {
                if (_transform == null)
                    _transform = base.transform as RectTransform;
                return _transform;
            }
        }
        protected override void OnUpdate(Vector3 value)
        {
            transform.anchoredPosition = value;
        }
        public override void SetClose()
        {
            close = transform.anchoredPosition;
        }

        public override void SetOpen()
        {
            open = transform.anchoredPosition;
        }
    }

