using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MMCFeedbacks
{
    [Serializable]
    public class LoopFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Loop";
        [SerializeField] private int loopCount = 1;

        [SerializeField] private FeedbackList list = new(new List<IFeedback>());

        public string MenuString => "Processing/Loop";
        public string Label => label;
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
            for (var i = 0; i < loopCount; i++)
            {
                var tasks = new List<UniTask>();
                foreach (var element in list.List) element.Reset();
                for (var j = 0; j < list.List.Count; j++)
                {
                    if (!list.List[j].IsActive) continue;
                    if (list.List[j] is IWaitFeedback waitFeedback)
                    {
                        waitFeedback.Wait(list.List, j);
                        await UniTask.WaitUntil(() => list.List[j].IsCompleted);
                    }

                    list.List[j].Play();
                    var j1 = j;
                    tasks.Add(UniTask.WaitUntil(() => list.List[j1].IsComplete()));
                }

                await UniTask.WhenAll(tasks);
            }

            IsCompleted = true;
        }
    }
}