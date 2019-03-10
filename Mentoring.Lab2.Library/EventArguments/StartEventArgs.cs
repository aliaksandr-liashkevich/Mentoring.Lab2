using System;

namespace Mentoring.Lab2.Library.EventArguments
{
    public class StartEventArgs : EventArgs
    {
        public DateTime CreationDate { get; }

        public StartEventArgs(DateTime creationDate)
        {
            CreationDate = creationDate;
        }
    }
}
