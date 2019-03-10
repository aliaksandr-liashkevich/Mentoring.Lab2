using System;

namespace Mentoring.Lab2.Library.EventArguments
{
    public class FinishEventArgs : EventArgs
    {
        public DateTime CreationDate { get; }

        public FinishEventArgs(DateTime creationDate)
        {
            CreationDate = creationDate;
        }
    }
}
