using System.Collections.Generic;
using System.IO;

namespace Mentoring.Lab2.Library.Wrappers.Interfaces
{
    public interface IDirectoryWrapper
    {
        DirectoryInfo GetDirectoryInfo(string path);
        IEnumerable<FileSystemInfo> FileSystemInfos(DirectoryInfo directoryInfo);
    }
}
