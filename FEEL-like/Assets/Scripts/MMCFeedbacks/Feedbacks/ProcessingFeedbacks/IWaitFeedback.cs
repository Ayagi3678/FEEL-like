using System.Collections.Generic;

namespace MMCFeedbacks
{
    public interface IWaitFeedback
    {
        void Wait(List<IFeedback> list, int index);
    }
}