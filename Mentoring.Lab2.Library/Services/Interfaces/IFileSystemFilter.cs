using Mentoring.Lab2.Library.Common;
using Mentoring.Lab2.Library.Models;

namespace Mentoring.Lab2.Library.Services.Interfaces
{
    public interface IFileSystemFilter : INotifyFiltering
    {
        VisitorAction Filtration(FileSystemObject fileSystemObject);
    }
}
