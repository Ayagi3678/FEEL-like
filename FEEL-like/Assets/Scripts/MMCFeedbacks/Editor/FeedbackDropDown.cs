using System;
using System.Collections.Generic;
using MMCFeedbacks.Utility;
using UnityEditor.IMGUI.Controls;

namespace MMCFeedbacks
{
    public class FeedbackDropDown : AdvancedDropdown
    {
        private readonly List<string> _feedbackList = new();

        public FeedbackDropDown(AdvancedDropdownState state) : base(state)
        {
            var types = TypeFinder.FindClassesImplementingInterface<IFeedback>();
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                if (instance is IFeedback custom) _feedbackList.Add(custom.MenuString);
            }
        }

        public event Action<AdvancedDropdownItem> OnSelect;

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("Feedback");
            foreach (var t in _feedbackList)
            {
                var trimString = "/".ToCharArray();
                var strings = t.Split(trimString);
                var item = new AdvancedDropdownItem(strings[0]);
                AdvancedDropdownItem rootElement = null;
                using var e = root.children.GetEnumerator();
                while (e.MoveNext())
                    if (strings[0] == e.Current?.name)
                        rootElement = e.Current;

                if (rootElement != null)
                {
                    rootElement.AddChild(new AdvancedDropdownItem(strings[1]));
                }
                else
                {
                    if (strings[1] != string.Empty) item.AddChild(new AdvancedDropdownItem(strings[1]));
                    root.AddChild(item);
                }
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            OnSelect?.Invoke(item);
        }
    }
}