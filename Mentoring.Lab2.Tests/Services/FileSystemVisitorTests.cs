using System;
using System.IO;
using System.Linq;
using Mentoring.Lab2.Library.Common;
using Mentoring.Lab2.Library.Models;
using Mentoring.Lab2.Library.Services;
using Mentoring.Lab2.Library.Services.Interfaces;
using Mentoring.Lab2.Library.Wrappers;
using Mentoring.Lab2.Library.Wrappers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mentoring.Lab2.Tests.Services
{
    [TestClass]
    public class FileSystemVisitorTests
    {
        private static Mock<IFileSystemFilter> _fileSystemFilterMock;
        private static Mock<IDirectoryWrapper> _directoryWrapperMock;
        private static Mock<IDateTimeWrapper> _dateTimeWrapperMock;

        public string TestDataPath = Path.Combine(Environment.CurrentDirectory, "TestData");

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _fileSystemFilterMock = new Mock<IFileSystemFilter>();
            _directoryWrapperMock = new Mock<IDirectoryWrapper>();
            _dateTimeWrapperMock = new Mock<IDateTimeWrapper>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _fileSystemFilterMock.Reset();
            _directoryWrapperMock.Reset();
            _dateTimeWrapperMock.Reset();
        }

        public IFileSystemVisitor CreateService(IDirectoryWrapper directoryWrapper = null)
        {
            return new FileSystemVisitor(_fileSystemFilterMock.Object,
                directoryWrapper ?? _directoryWrapperMock.Object,
                _dateTimeWrapperMock.Object);
        }

        [TestMethod]
        public void GetFileSystemObjects_StartEvent()
        {
            const string path = "path";
            var startDispatched = false;
            var service = CreateService();
            service.Start += (sender, args) => { startDispatched = true; };

            _dateTimeWrapperMock.Setup(x => x.UtcNow);
            _directoryWrapperMock.Setup(x => x.GetDirectoryInfo(path));

            service.GetFileSystemObjects(path).ToList();

            _dateTimeWrapperMock.Verify(x => x.UtcNow, Times.Exactly(2));
            _directoryWrapperMock.Verify(x => x.GetDirectoryInfo(path), Times.Once);

            Assert.IsTrue(startDispatched);
        }

        [TestMethod]
        public void GetFileSystemObjects_3FileSystemObjects()
        {
            const int expectedCount = 3;
            var service = CreateService(new DirectoryWrapper());

            _fileSystemFilterMock.Setup(x => x.Filtration(It.IsAny<FileSystemObject>()))
                .Returns(VisitorAction.Searching);

            var actualCount = service.GetFileSystemObjects(TestDataPath).Count();

            _fileSystemFilterMock.Verify(x => x.Filtration(It.IsAny<FileSystemObject>()), Times.Exactly(expectedCount));

            Assert.IsTrue(expectedCount == actualCount);
        }

        [TestMethod]
        public void GetFileSystemObjects_DirectoryNotFound_ArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => CreateService(new DirectoryWrapper()).GetFileSystemObjects("DIRECTORY_NOT_FOUND").ToList());
        }

        [TestMethod]
        public void GetFileSystemObjects_FinishEvent()
        {
            const string path = "path";
            var finishDispatched = false;
            var service = CreateService();
            service.Finish += (sender, args) => { finishDispatched = true; };

            _dateTimeWrapperMock.Setup(x => x.UtcNow);
            _directoryWrapperMock.Setup(x => x.GetDirectoryInfo(path));

            service.GetFileSystemObjects(path).ToList();

            _dateTimeWrapperMock.Verify(x => x.UtcNow, Times.Exactly(2));
            _directoryWrapperMock.Verify(x => x.GetDirectoryInfo(path), Times.Once);

            Assert.IsTrue(finishDispatched);
        }
    }
}
