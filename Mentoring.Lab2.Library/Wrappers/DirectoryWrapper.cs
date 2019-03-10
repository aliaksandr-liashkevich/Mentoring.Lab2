using System;
using System.Collections.Generic;
using System.IO;
using Mentoring.Lab2.Library.Wrappers.Interfaces;

namespace Mentoring.Lab2.Library.Wrappers
{
    public class DirectoryWrapper : IDirectoryWrapper
    {
        public DirectoryInfo GetDirectoryInfo(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!Directory.Exists(path))
            {
                throw new ArgumentException($"Path: {path}. Directory not found.");
            }

            return new DirectoryInfo(path);
        }

        public IEnumerable<FileSystemInfo> FileSystemInfos(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            return directoryInfo.EnumerateFileSystemInfos();
        }
    }
}
