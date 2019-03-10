using System;
using Mentoring.Lab2.Library.Common;
using Mentoring.Lab2.Library.Models;
using Mentoring.Lab2.Library.Services;
using Mentoring.Lab2.Library.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mentoring.Lab2.Tests.Services
{
    [TestClass]
    public class FileSystemFilterTests
    {
        public IFileSystemFilter CreateService(Predicate<FileSystemObject> filter = null)
        {
            return new FileSystemFilter(filter);
        }

        [TestMethod]
        public void Filtration_InputNull_ArgumentNullException()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => CreateService().Filtration(null));
        }

        [TestMethod]
        public void Filtration_SearchingResult()
        {
            var result = CreateService((f) => true).Filtration(new FileSystemObject()
            {
                Type = SystemObjectType.File
            });

            Assert.AreEqual(VisitorAction.Searching, result);
        }

        [TestMethod]
        public void Filtration_FilterReturnFalse_SkipResult()
        {
            var result = CreateService((f) => false).Filtration(new FileSystemObject()
            {
                Type = SystemObjectType.Directory
            });

            Assert.AreEqual(VisitorAction.SkipSystemObject, result);
        }

        [TestMethod]
        public void Filtration_SystemObjectTypeNone_SkipResult()
        {
            var result = CreateService((f) => true).Filtration(new FileSystemObject()
            {
                Type = SystemObjectType.None
            });

            Assert.AreEqual(VisitorAction.SkipSystemObject, result);
        }

        [TestMethod]
        public void Filtration_FileFoundEvent()
        {
            var fileFoundDispatched = false;
            var service = CreateService();
            service.FileFound += (sender, args) => { fileFoundDispatched = true; };

            service.Filtration(new FileSystemObject()
            {
                Type = SystemObjectType.File
            });


            Assert.IsTrue(fileFoundDispatched);
        }

        [TestMethod]
        public void Filtration_FilteredDirectoryFoundEvent()
        {
            var expected = new FileSystemObject()
            {
                Type = SystemObjectType.Directory
            };
            var service = CreateService((f) => true);
            FileSystemObject actual = null;
            service.FilteredDirectoryFound += (sender, args) => { actual = args.Item; };

            service.Filtration(expected);

            Assert.AreEqual(expected, actual);
        }
    }
}
