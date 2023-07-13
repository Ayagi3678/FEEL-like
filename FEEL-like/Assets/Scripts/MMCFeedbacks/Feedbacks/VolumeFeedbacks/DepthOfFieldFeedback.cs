namespace MimicFeedBacker.Feedbacks.Task.VolumeFeedbacks
{
    //TODO:DepthOfFieldFeedback作る
    /*[Serializable]
    public class DepthOfFieldFeedback : ITestTask
    {
        public bool isActive = true;
        public string label = "Depth of Field";

        [Header("PostProcess")] [SerializeField]
        private float duration = 1;

        [SerializeField] private AnimationCurve easeCurve = AnimationCurve.Linear(0, 1, 1, 0);
        [SerializeField] private float start = 1;
        [SerializeField] private float end;

        [Header("Depth of Field")] public float focusDistance = 10;
        public DepthOfFieldMode mode;
        
        public float gaussianStart = 10;
        public float gaussianEnd = 30;
        [Range(-1.5f, 1.5f)] public float gaussianMaxRadius = 1;
        public bool highQualitySampling;
        private Tween _tween;
        private Volume _volume;
        public string MenuString => "Volume/Depth of Field";
        public string Label => label;
        public Color BarColor => Color.green;

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public bool IsCompleted { get; private set; }

        public void Reset()
        {
            _tween.Complete();
            IsCompleted = false;
        }

        public void Play()
        {
            _tween.Complete();
            _volume = VolumeSingleton.Instance.volume;
            var depthOfField = VolumeComponentUtil.AddVolumeComponent<DepthOfField>(_volume);
            VolumeComponentUtil.ActiveAllProperty(depthOfField);
            _tween.OnComplete(null);
            /*_tween = DOVirtual.Float(endIntensity, intensity, duration, value => depthOfField.intensity.value = value)
                .SetEase(easeCurve)
                .OnComplete(() => Volume.profile.components.Remove(depthOfField));#1#
        }

        public void Stop()
        {
            _tween.Pause();
        }
        public enum DepthOfFieldMode
        {
            Gaussian,
            Bokeh
        }
    }*/
}