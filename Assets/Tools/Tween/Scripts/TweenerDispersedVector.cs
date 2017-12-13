using System;
using UnityEngine;


    public class TweenerDispersedVector : MonoBehaviour
    {
        public Action OpendEvent;
        public Action CloseEvent;
        public enum AnimatorType
        {
            one,
            loop,
            pingpong
        }

        public AnimationCurve XCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
        public AnimationCurve YCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(1f, 1f, 2f, 2f));
        public AnimationCurve ZCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        public AnimatorType animatorType;
        public float speed = 10.0f;
        public bool StartPlay;
        public Vector3 close;
        public Vector3 open;

        bool IsOpen;
        bool IsPlay;
        int direction = 1;
        float time;
        public void OnOpen()
        {
            IsOpen = true;
            IsPlay = true;
            direction = 1;
            time = 0;
        }
        public void OnClose()
        {
            IsOpen = false;
            IsPlay = true;
            direction = -1;
            time = 1;
        }
        public void OnChange()
        {
            if (IsOpen) OnClose();
            else OnOpen();
        }
        public void OnPlay()
        {
            if (IsOpen) OnOpen();
            else OnClose();
        }

        public void OnStop()
        {
            IsPlay = false;
            float time = IsOpen ? 1 : 0;
            OnUpdate(XCurve.Evaluate(time), YCurve.Evaluate(time), ZCurve.Evaluate(time));
        }

        private void Update()
        {
            if (!IsPlay) return;
            time += speed * Time.deltaTime * direction;
            OnUpdate(XCurve.Evaluate(time), YCurve.Evaluate(time), ZCurve.Evaluate(time));
            if (time > 1 || time < 0)
            {
                switch (animatorType)
                {
                    case AnimatorType.one:
                        IsPlay = false;
                        break;
                    case AnimatorType.loop:
                        OnPlay();
                        break;
                    case AnimatorType.pingpong:
                        OnChange();
                        break;
                }
                if (IsOpen)
                {
                    if (OpendEvent != null)
                        OpendEvent();
                }
                else
                {
                    if (CloseEvent != null)
                        CloseEvent();
                }
            }
        }

        void Start()
        {
            if (StartPlay)
                OnPlay();
        }
        void OnUpdate(float x, float y, float z)
        {
            Vector3 point = Vector3.zero;
            float value = close.x * (1f - x) + open.x * x;
            point.x = value;

            value = close.y * (1f - y) + open.y * y;
            point.y = value;

            value = close.z * (1f - z) + open.z * z;
            point.z = value;
            transform.position = point;
        }


//#if UNITY_EDITOR

        public void SetClose()
        {
            close = transform.position;
        }
        public void SetOpen()
        {
            open = transform.position;
        }
        public void ToClose()
        {
            IsOpen = false;
            float time = IsOpen ? 1 : 0;
            OnUpdate(XCurve.Evaluate(time), YCurve.Evaluate(time), ZCurve.Evaluate(time));
        }
        public void ToOpen()
        {
            IsOpen = true;
            float time = IsOpen ? 1 : 0;
            OnUpdate(XCurve.Evaluate(time), YCurve.Evaluate(time), ZCurve.Evaluate(time));
        }
        public void UpdateSlider(float time)
        {
            OnUpdate(XCurve.Evaluate(time), YCurve.Evaluate(time), ZCurve.Evaluate(time));
        }
    }
