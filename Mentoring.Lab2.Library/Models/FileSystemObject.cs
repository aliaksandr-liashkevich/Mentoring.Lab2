using Mentoring.Lab2.Library.Common;

namespace Mentoring.Lab2.Library.Models
{
    public class FileSystemObject
    {
        public SystemObjectType Type { get; internal set; }
        public string FullName { get; internal set; }
        public string Name { get; internal set; }
    }
}
