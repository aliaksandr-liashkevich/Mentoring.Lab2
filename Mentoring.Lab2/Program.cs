using System;
using Mentoring.Lab2.Library.Common;
using Mentoring.Lab2.Library.Services;
using Mentoring.Lab2.Library.Wrappers;

namespace Mentoring.Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileSystemFiler = new FileSystemFilter((f) =>
            {
                if (f.FullName.Contains("Mono"))
                {
                    return false;
                }

                return true;
            });
            var directoryWrapper = new DirectoryWrapper();
            var dateTimeWrapper = new DateTimeWrapper();

            var fileSystemVisitor = new FileSystemVisitor(fileSystemFiler, directoryWrapper, dateTimeWrapper);

            fileSystemVisitor.Filtering.FileFound += (sender, eventArgs) =>
            {
                if (eventArgs.Item.FullName.Contains("1"))
                {
                    eventArgs.Action = VisitorAction.SkipSystemObject;
                }
            };

            foreach (var fileSystemObject in fileSystemVisitor.GetFileSystemObjects(@"D:\Mentoring"))
            {
                Console.WriteLine(fileSystemObject.FullName);
            }

            Console.ReadKey();
        }
    }
}
