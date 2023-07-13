using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace MMCFeedbacks.Singleton
{
    public class TimeEffectSingleton : SingletonBehaviour<TimeEffectSingleton>
    {
        private readonly ReactiveCollection<TimeEffect> _timeEffects = new(new List<TimeEffect>());

        protected override void OnInitialize()
        {
            _timeEffects
                .ObserveCountChanged()
                .Where(count => count != 0)
                .Subscribe(_ =>
                {
                    var highestEffect = _timeEffects
                        .OrderByDescending(item => item.Priority)
                        .FirstOrDefault();
                    if (highestEffect != null) Time.timeScale = highestEffect.TimeScale;
                })
                .AddTo(this);
            _timeEffects.ObserveCountChanged().Where(count => count == 0).Subscribe(_ => Time.timeScale = 1)
                .AddTo(this);
        }

        public void SetTimeEffect(TimeEffect timeEffect)
        {
            _timeEffects.Add(timeEffect);
            timeEffect.OnDiscardCallback = () => _timeEffects.Remove(timeEffect);
        }
    }
}