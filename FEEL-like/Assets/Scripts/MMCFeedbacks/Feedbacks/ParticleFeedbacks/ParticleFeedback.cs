using System;
using UnityEngine;

namespace MMCFeedbacks
{
    [Serializable]
    public class ParticleFeedback : IFeedback
    {
        public bool isActive = true;

        [Header("Particle")] public string label = "Particle Play";

        [SerializeField] private ParticleSystem particleSystem;
        public string MenuString => "Particle/Particle Play";
        public string Label => label;
        public Color BarColor => Color.Lerp(Color.blue, Color.cyan, .5f);

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public bool IsCompleted => true;

        public void Reset()
        {
            particleSystem.Stop();
        }

        public void Play()
        {
            particleSystem.Play();
        }

        public void Stop()
        {
            if (!IsActive) return;
            particleSystem.Pause();
        }
    }
}