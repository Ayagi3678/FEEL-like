using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MMCFeedbacks.Attribute;
using UnityEngine;

namespace MMCFeedbacks
{
    [Serializable]
    public class WaitFeedback : IFeedback, IWaitFeedback
    {
        [SerializeField] private bool isActive = true;

        [Header("Wait")] [SerializeField] private WaitType waitType;

        [SerializeField] [ConditionalDisable("waitType", (int)WaitType.Time, false, true)]
        private float waitTime;

        public string MenuString => "Processing/Wait";
        public string Label => "Wait";
        public Color BarColor => Color.white;

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public bool IsCompleted { get; private set; }

        public void Reset()
        {
            IsCompleted = false;
        }

        public void Play()
        {
        }

        public void Stop()
        {
        }

        public void Wait(List<IFeedback> list, int index)
        {
            WaitAsync(list, index).Forget();
        }

        private async UniTaskVoid WaitAsync(IReadOnlyList<IFeedback> list, int index)
        {
            switch (waitType)
            {
                case WaitType.Time:
                    await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
                    break;
                case WaitType.Complete:
                    if (index != 0) await UniTask.WaitUntil(() => list[index - 1].IsCompleted);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            IsCompleted = true;
        }

        private enum WaitType
        {
            Time,
            Complete
        }
    }
}