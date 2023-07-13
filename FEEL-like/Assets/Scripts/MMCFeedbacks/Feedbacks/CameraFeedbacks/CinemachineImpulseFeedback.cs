using System;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MMCFeedbacks
{
    [Serializable]
    public class CinemachineImpulseFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Cinemachine Impulse";

        [Header("Cinemachine Impulse")] [SerializeField]
        public CinemachineImpulseDefinition mImpulseDefinition = new();

        public bool isVelocityRandom = true;
        public Vector3 velocity;
        public string MenuString => "Camera/Cinemachine Impulse";
        public string Label => label;
        public Color BarColor => Color.red;

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
            if (mImpulseDefinition == null) return;
            if (Camera.main != null)
                mImpulseDefinition.CreateEvent(Camera.main.transform.position,
                    !isVelocityRandom ? velocity : Random.onUnitSphere);
        }

        public void Stop()
        {
        }
    }
}