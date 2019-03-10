using System;
using Mentoring.Lab2.Library.EventArguments;

namespace Mentoring.Lab2.Library.Services.Interfaces
{
    public interface INotifyFiltering
    {
        event EventHandler<SystemObjectFoundEventArgs> FileFound;
        event EventHandler<SystemObjectFoundEventArgs> DirectoryFound;
        event EventHandler<SystemObjectFoundEventArgs> FilteredFileFound;
        event EventHandler<SystemObjectFoundEventArgs> FilteredDirectoryFound;
    }
}
