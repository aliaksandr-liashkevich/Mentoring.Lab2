using System;
using System.Collections.Generic;
using Mentoring.Lab2.Library.EventArguments;
using Mentoring.Lab2.Library.Models;

namespace Mentoring.Lab2.Library.Services.Interfaces
{
    public interface IFileSystemVisitor
    {
        event EventHandler<StartEventArgs> Start;
        event EventHandler<FinishEventArgs> Finish;
        INotifyFiltering Filtering { get; }
        IEnumerable<FileSystemObject> GetFileSystemObjects(string path);
    }
}
