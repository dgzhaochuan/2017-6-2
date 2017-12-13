
using UnityEngine;

    public abstract class TweenerVector : Tweener
    {
        public Vector3 close;
        public Vector3 open;

        protected override void OnUpdate(float factor)
        {
            OnUpdate(close * (1f - factor) + open * factor);
        }

        protected abstract void OnUpdate(Vector3 value);
    }

