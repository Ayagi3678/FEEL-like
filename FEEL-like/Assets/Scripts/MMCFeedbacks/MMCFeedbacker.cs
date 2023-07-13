using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MMCFeedbacks.Singleton;
using UnityEngine;

namespace MMCFeedbacks
{
    public class MMCFeedbacker : MonoBehaviour
    {
        [SerializeField] private FeedbackList list = new(new List<IFeedback>());

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
            foreach (var element in list.List) element.Reset();
            for (var i = 0; i < list.List.Count; i++)
            {
                if (!list.List[i].IsActive) continue;
                if (list.List[i] is IWaitFeedback waitFeedback)
                {
                    waitFeedback.Wait(list.List, i);
                    await UniTask.WaitUntil(() => list.List[i].IsCompleted);
                }

                list.List[i].Play();
            }
        }
    }
}