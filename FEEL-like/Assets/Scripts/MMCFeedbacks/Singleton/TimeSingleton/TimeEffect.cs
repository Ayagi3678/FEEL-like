using Cysharp.Threading.Tasks;

namespace MMCFeedbacks.Singleton
{
    public sealed class TimeEffect
    {
        public delegate void OnDiscardDelegate();

        public readonly int Priority;
        public readonly float TimeScale;
        public OnDiscardDelegate OnDiscardCallback;

        public TimeEffect(int priority, float timeScale, int discordFrame)
        {
            Priority = priority;
            TimeScale = timeScale;
            DiscardAsync(discordFrame).Forget();
        }

        private async UniTaskVoid DiscardAsync(int discordFrame)
        {
            await UniTask.DelayFrame(discordFrame);
            OnDiscardCallback();
        }
    }
}