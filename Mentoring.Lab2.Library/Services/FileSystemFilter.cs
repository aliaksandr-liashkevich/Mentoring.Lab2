using System;
using Mentoring.Lab2.Library.Common;
using Mentoring.Lab2.Library.EventArguments;
using Mentoring.Lab2.Library.Models;
using Mentoring.Lab2.Library.Services.Interfaces;

namespace Mentoring.Lab2.Library.Services
{
    public class FileSystemFilter : IFileSystemFilter
    {
        private readonly Predicate<FileSystemObject> _filter;

        public event EventHandler<SystemObjectFoundEventArgs> FileFound = delegate { };
        public event EventHandler<SystemObjectFoundEventArgs> DirectoryFound = delegate { };
        public event EventHandler<SystemObjectFoundEventArgs> FilteredFileFound = delegate { };
        public event EventHandler<SystemObjectFoundEventArgs> FilteredDirectoryFound = delegate { };

        public FileSystemFilter(Predicate<FileSystemObject> filter = null)
        {
            _filter = filter;
        }

        public VisitorAction Filtration(FileSystemObject fileSystemObject)
        {
            if (fileSystemObject == null)
            {
                throw new ArgumentNullException(nameof(fileSystemObject));
            }

            if (fileSystemObject.Type == SystemObjectType.None)
            {
                return VisitorAction.SkipSystemObject;
            }

            var systemObjectFoundEventArgs = new SystemObjectFoundEventArgs(fileSystemObject);

            if (fileSystemObject.Type == SystemObjectType.File)
            {
                FileFound.Invoke(this, systemObjectFoundEventArgs);
            }

            if (fileSystemObject.Type == SystemObjectType.Directory)
            {
                DirectoryFound.Invoke(this, systemObjectFoundEventArgs);
            }

            if (systemObjectFoundEventArgs.Action == VisitorAction.Searching && _filter != null)
            {
                if (_filter(fileSystemObject))
                {
                    if (fileSystemObject.Type == SystemObjectType.File)
                    {
                        FilteredFileFound(this, systemObjectFoundEventArgs);
                    }

                    if (fileSystemObject.Type == SystemObjectType.Directory)
                    {
                        FilteredDirectoryFound(this, systemObjectFoundEventArgs);
                    }
                }
                else
                {
                    return VisitorAction.SkipSystemObject;
                }
            }

            return systemObjectFoundEventArgs.Action;
        }
    }
}
