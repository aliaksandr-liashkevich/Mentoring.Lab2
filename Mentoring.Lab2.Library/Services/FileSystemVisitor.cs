using System;
using System.Collections.Generic;
using System.IO;
using Mentoring.Lab2.Library.Common;
using Mentoring.Lab2.Library.EventArguments;
using Mentoring.Lab2.Library.Models;
using Mentoring.Lab2.Library.Services.Interfaces;
using Mentoring.Lab2.Library.Wrappers.Interfaces;

namespace Mentoring.Lab2.Library.Services
{
    public class FileSystemVisitor : IFileSystemVisitor
    {
        private readonly IFileSystemFilter _fileSystemFilter;
        private readonly IDirectoryWrapper _directoryWrapper;
        private readonly IDateTimeWrapper _dateTimeWrapper;

        public FileSystemVisitor(IFileSystemFilter fileSystemFilter,
            IDirectoryWrapper directoryWrapper,
            IDateTimeWrapper dateTimeWrapper)
        {
            _fileSystemFilter = fileSystemFilter ?? throw new ArgumentNullException(nameof(fileSystemFilter));
            _directoryWrapper = directoryWrapper ?? throw new ArgumentNullException(nameof(directoryWrapper));
            _dateTimeWrapper = dateTimeWrapper ?? throw new ArgumentNullException(nameof(dateTimeWrapper));
        }

        public event EventHandler<StartEventArgs> Start = delegate { };
        public event EventHandler<FinishEventArgs> Finish = delegate { };

        public INotifyFiltering Filtering => _fileSystemFilter;

        public IEnumerable<FileSystemObject> GetFileSystemObjects(string path)
        {
            var currentDirectory = _directoryWrapper.GetDirectoryInfo(path);

            var startEventArgs = new StartEventArgs(_dateTimeWrapper.UtcNow);
            Start(this, startEventArgs);

            foreach (var fileSystemObject in GetFileSystemObjects(currentDirectory, VisitorState.DefaultState))
            {
                yield return fileSystemObject;
            }
            
            var finishEventArgs = new FinishEventArgs(_dateTimeWrapper.UtcNow);
            Finish(this, finishEventArgs);
        }

        private IEnumerable<FileSystemObject> GetFileSystemObjects(DirectoryInfo currentDirectory, VisitorState state)
        {
            foreach (var fileSystemInfo in _directoryWrapper.FileSystemInfos(currentDirectory))
            {
                if (fileSystemInfo.Attributes != FileAttributes.Directory)
                {
                    var fileObject = new FileSystemObject
                    {
                        Type = SystemObjectType.File,
                        Name = fileSystemInfo.Name,
                        FullName = fileSystemInfo.FullName
                    };

                    state.Action = _fileSystemFilter.Filtration(fileObject);

                    if (state.Action == VisitorAction.Searching)
                    {
                        yield return fileObject;
                    }
                }

                if (fileSystemInfo is DirectoryInfo directory)
                {
                    var directoryObject = new FileSystemObject
                    {
                        Type = SystemObjectType.Directory,
                        Name = fileSystemInfo.Name,
                        FullName = fileSystemInfo.FullName
                    };

                    state.Action = _fileSystemFilter.Filtration(directoryObject);

                    if (state.Action == VisitorAction.Searching)
                    {
                        yield return directoryObject;

                        foreach (var fileSystemObject in GetFileSystemObjects(directory, state))
                        {
                            yield return fileSystemObject;
                        }
                    }
                }

                if (state.Action == VisitorAction.StopSearching)
                {
                    yield break;
                }
            }
        }

        private class VisitorState
        {
            public static readonly VisitorState DefaultState = new VisitorState();

            private VisitorAction _action = VisitorAction.Searching;

            public VisitorAction Action
            {
                get => _action;
                set
                {
                    if (_action != VisitorAction.StopSearching)
                    {
                        _action = value;
                    }
                }
            }
        }
    }
}
