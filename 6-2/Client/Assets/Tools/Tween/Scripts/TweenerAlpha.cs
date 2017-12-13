
using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class TweenerAlpha : Tweener
    {
        public float close = 0;
        public float open = 1;

        CanvasGroup _group;
        CanvasGroup group
        {
            get
            {
                if (_group == null)
                    _group = GetComponent<CanvasGroup>();
                return _group;
            }
        }
        protected override void OnUpdate(float factor)
        {
            group.alpha = close * (1f - factor) + open * factor;
        }

        public override void SetClose()
        {
            close = group.alpha;
        }

        public override void SetOpen()
        {
            open = group.alpha;
        }
    }

