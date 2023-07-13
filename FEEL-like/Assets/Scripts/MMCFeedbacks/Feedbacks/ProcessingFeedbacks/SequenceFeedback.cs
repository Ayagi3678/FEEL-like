using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MMCFeedbacks
{
    [Serializable]
    public class SequenceFeedback : IFeedback
    {
        [SerializeField] private bool isActive = true;
        [SerializeField] private FeedbackList list = new(new List<IFeedback>());
        private CancellationTokenSource _cancellationTokenSource;
        public string MenuString => "Processing/Sequence";
        public string Label => "Sequence";
        public Color BarColor => Color.white;

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public bool IsCompleted { get; private set; }

        public void Reset()
        {
            foreach (var element in list.List) element.Reset();
        }

        public void Play()
        {
            PlayTask().Forget();
        }

        public void Stop()
        {
            foreach (var element in list.List) element.Stop();
        }

        private async UniTaskVoid PlayTask()
        {
            _cancellationTokenSource?.Cancel();
            foreach (var element in list.List) element.Reset();
            foreach (var element in list.List)
            {
                if (!element.IsActive) continue;
                _cancellationTokenSource = new CancellationTokenSource();
                element.Play();
                await UniTask.WaitUntil(() => element.IsComplete(),
                    cancellationToken: _cancellationTokenSource.Token);
            }

            IsCompleted = true;
        }
    }
}