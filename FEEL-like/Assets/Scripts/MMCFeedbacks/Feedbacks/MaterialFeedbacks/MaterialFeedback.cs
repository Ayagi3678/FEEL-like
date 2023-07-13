using System;
using UnityEngine;

namespace MMCFeedbacks
{
    [Serializable]
    public class MaterialFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Material";

        public string MenuString => "Material/Material";
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
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}