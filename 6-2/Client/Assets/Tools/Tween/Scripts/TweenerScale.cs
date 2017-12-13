
using UnityEngine;


    public class TweenerScale : TweenerVector
    {
        protected override void OnUpdate(Vector3 value)
        {
            transform.localScale = value;
        }

        public override void SetClose()
        {
            close = transform.localScale;
        }

        public override void SetOpen()
        {
            open = transform.localScale;
        }
    }

