using UnityEngine;

    public class TweenerTransform : TweenerVector
    {
        public enum TweenType
        {
            position,
            rotation,
            scale,
        }
        public TweenType type;

        public bool IsLocal;
        public override void SetClose()
        {
            update(close);
        }

        public override void SetOpen()
        {
            update(open);
        }

        protected override void OnUpdate(Vector3 value)
        {
            update(value);
        }

        public  void update(Vector3 value)
        {
            if (IsLocal)
            {
                switch (type)
                {
                    case TweenType.position:
                        transform.localPosition = value;
                        break;
                    case TweenType.rotation:
                        transform.localRotation = Quaternion.Euler(value);
                        break;
                    case TweenType.scale:
                        transform.localScale = value;
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case TweenType.position:
                        transform.position = value;
                        break;
                    case TweenType.rotation:
                        transform.rotation = Quaternion.Euler(value);
                        break;
                    case TweenType.scale:
                        transform.localScale = value;
                        break;
                }
            }
        }
    }
