using System;
using Mentoring.Lab2.Library.Common;
using Mentoring.Lab2.Library.Models;

namespace Mentoring.Lab2.Library.EventArguments
{
    public class SystemObjectFoundEventArgs : EventArgs
    {
        public FileSystemObject Item { get; }
        public VisitorAction Action { get; set; }

        public SystemObjectFoundEventArgs(FileSystemObject item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
            Action = VisitorAction.Searching;
        }
    }
}
