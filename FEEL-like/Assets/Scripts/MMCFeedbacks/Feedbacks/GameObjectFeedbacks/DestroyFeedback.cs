using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MMCFeedbacks
{
    [Serializable]
    public class DestroyFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Destroy";

        [Header("GameObject")] [SerializeField]
        private GameObject destroyObject;

        public string MenuString => "GameObject/Destroy";
        public string Label => label;
        public Color BarColor => Color.Lerp(Color.blue, Color.white, .3f);

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public bool IsCompleted => true;

        public void Reset()
        {
        }

        public void Play()
        {
            Object.Destroy(destroyObject);
        }

        public void Stop()
        {
        }
    }
}