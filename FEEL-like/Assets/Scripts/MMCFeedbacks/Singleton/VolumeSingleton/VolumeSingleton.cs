using AnnulusGames.LucidTools.Inspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace MMCFeedbacks.Singleton
{
    [RequireComponent(typeof(Volume))]
    public class VolumeSingleton : SingletonBehaviour<VolumeSingleton>
    {
        public Volume volume;

        private void Reset()
        {
            ReferenceFind();
        }

        [Button]
        private void ReferenceFind()
        {
            TryGetComponent(out volume);
        }

        protected override void OnInitialize()
        {
            ReferenceFind();
        }
    }
}