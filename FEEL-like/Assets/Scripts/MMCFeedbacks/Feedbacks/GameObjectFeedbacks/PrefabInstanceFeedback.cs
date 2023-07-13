using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MMCFeedbacks
{
    [Serializable]
    public class PrefabInstanceFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Prefab Instance";

        [Header("GameObject")] [SerializeField]
        private GameObject prefab;

        public Transform rootTransform;
        public bool isQuaternionIdentity;
        public Vector3 position;
        public string MenuString => "GameObject/Prefab Instance";
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
            Object.Instantiate(prefab, rootTransform.position + position,
                isQuaternionIdentity ? Quaternion.identity : rootTransform.rotation);
        }

        public void Stop()
        {
        }
    }
}