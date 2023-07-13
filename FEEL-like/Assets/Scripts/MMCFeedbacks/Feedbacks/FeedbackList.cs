using System;
using System.Collections.Generic;
using MMCFeedbacks.Utility;
using UnityEngine;

namespace MMCFeedbacks
{
    [Serializable]
    public sealed class FeedbackList
    {
        [SerializeReference] public List<IFeedback> List;

        public FeedbackList(List<IFeedback> list)
        {
            List = list;
        }

        public void ProcessSelection(string selectedOption)
        {
            var types = TypeFinder.FindClassesImplementingInterface<IFeedback>();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                if (instance is not IFeedback custom) continue;
                var trimString = "/".ToCharArray();
                var strings = custom.MenuString.Split(trimString);
                if (strings[^1] != selectedOption) continue;
                List.Add(custom);
            }
        }
    }
}