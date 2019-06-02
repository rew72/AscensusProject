using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using web;
using web.Controllers;
using web.Models;
using web.ViewModels;


namespace NUnitTestProject1
{
    public class WebControllerTests
    {

        private readonly EmployeeModel _rob = new EmployeeModel()
        {
            firstName = "Rob",
            lastName = "Wilson",
            email = "rob.wilson@ascensus.com"
        };
        private readonly EmployeeModel _joe = new EmployeeModel()
        {
            firstName = "Joe",
            lastName = "Rue",
            email = "joe.rue@ascensus.com"
        };
        private readonly EmployeeModel _mark = new EmployeeModel()
        {
            firstName = "Mark",
            lastName = "Little",
            email = "mark.little@ascensus.com"
        };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestIndex()
        {
            // Arrange
            List<EmployeeModel> expectedUsers = new List<EmployeeModel>()
            {
                _rob,
                _joe,
                _mark
            };

            var fakeHelper = A.Fake<IEmployeeRecordsControllerHelper>();
            A.CallTo(() => fakeHelper.GetEmployees()).Returns(expectedUsers);
            var value = new EmployeeRecordsController(fakeHelper);

            // Act
            var result = value.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult> (result);
            var x = ((ViewResult) result).ViewData.Model;
            Assert.AreEqual(_rob.email, ((RecordViewModel)x).employees[0].email);
            Assert.AreEqual(_joe.email, ((RecordViewModel)x).employees[1].email);
            Assert.AreEqual(_mark.email, ((RecordViewModel)x).employees[2].email);
        }

        [Test]
        public void TestEmployeeView()
        {
            // Arrange
            var fakeHelper = A.Fake<IEmployeeRecordsControllerHelper>();
            var value = new EmployeeRecordsController(fakeHelper);
            A.CallTo(() => fakeHelper.GetSpecificEmployee(A<string>.Ignored)).Returns(_rob);

            // Act
            var result = value.EmployeeView("rob.wilson@ascensus.com");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var x = ((ViewResult)result).ViewData.Model;
            Assert.AreEqual(_rob.email, ((EmployeeModel)x).email);
            Assert.AreEqual(_rob.firstName, ((EmployeeModel)x).firstName);
            Assert.AreEqual(_rob.lastName, ((EmployeeModel)x).lastName);
        }

        [Test]
        public void TestCreate()
        {
            // Arrange
            var fakeHelper = A.Fake<IEmployeeRecordsControllerHelper>();
            var value = new EmployeeRecordsController(fakeHelper);

            // Act
            var result = value.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestCreateSent()
        {
            List<EmployeeModel> expectedUsers = new List<EmployeeModel>()
            {
                _rob,
                _joe
            };

            var fakeHelper = A.Fake<IEmployeeRecordsControllerHelper>();
            A.CallTo(() => fakeHelper.GetEmployees()).Returns(expectedUsers);
            A.CallTo(() => fakeHelper.SetOrUpdateEmployee(A<EmployeeModel>.Ignored, A<string>.Ignored)).DoesNothing();
            var value = new EmployeeRecordsController(fakeHelper);

            // Act
            var result = value.CreateSent(_mark);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var x = ((RedirectToActionResult) result);
            Assert.AreEqual("index", x.ActionName);
            A.CallTo(() => fakeHelper.SetOrUpdateEmployee(A<EmployeeModel>.That.Matches(f => f.email == "mark.little@ascensus.com" == true), "create")).MustHaveHappened();
            A.CallTo(() => fakeHelper.SetOrUpdateEmployee(A<EmployeeModel>.That.Matches(f => f.firstName == "Mark" == true), "create")).MustHaveHappened();
            A.CallTo(() => fakeHelper.SetOrUpdateEmployee(A<EmployeeModel>.That.Matches(f => f.lastName == "Little" == true), "create")).MustHaveHappened();
        }

        [Test]
        public void TestEdit()
        {
            // Arrange
            var fakeHelper = A.Fake<IEmployeeRecordsControllerHelper>();
            var value = new EmployeeRecordsController(fakeHelper);
            A.CallTo(() => fakeHelper.GetSpecificEmployee(A<string>.Ignored)).Returns(_rob);

            // Act
            var result = value.Edit("rob.wilson@ascensus.com");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var x = ((ViewResult)result).ViewData.Model;
            Assert.AreEqual(_rob.email, ((EmployeeModel)x).email);
            Assert.AreEqual(_rob.firstName, ((EmployeeModel)x).firstName);
            Assert.AreEqual(_rob.lastName, ((EmployeeModel)x).lastName);
        }

        [Test]
        public void TestEditSent()
        {
            var fakeHelper = A.Fake<IEmployeeRecordsControllerHelper>();
            A.CallTo(() => fakeHelper.SetOrUpdateEmployee(A<EmployeeModel>.Ignored, A<string>.Ignored)).DoesNothing();
            var value = new EmployeeRecordsController(fakeHelper);

            // Act
            var result = value.EditSent(_mark);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var x = ((RedirectToActionResult)result);
            Assert.AreEqual("index", x.ActionName);
            A.CallTo(() => fakeHelper.SetOrUpdateEmployee(A<EmployeeModel>.That.Matches(f => f.email == "mark.little@ascensus.com" == true), "update")).MustHaveHappened();
            A.CallTo(() => fakeHelper.SetOrUpdateEmployee(A<EmployeeModel>.That.Matches(f => f.firstName == "Mark" == true), "update")).MustHaveHappened();
            A.CallTo(() => fakeHelper.SetOrUpdateEmployee(A<EmployeeModel>.That.Matches(f => f.lastName == "Little" == true), "update")).MustHaveHappened();
        }

        [Test]
        public void TestDelete()
        {
            var fakeHelper = A.Fake<IEmployeeRecordsControllerHelper>();
            A.CallTo(() => fakeHelper.DeleteEmployee(A<string>.Ignored)).DoesNothing();
            var value = new EmployeeRecordsController(fakeHelper);

            // Act
            var result = value.Delete("rob.wilson@ascensus.com");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var x = ((RedirectToActionResult)result);
            Assert.AreEqual("index", x.ActionName);
        }

        [Test]
        public void TestViewSalaries()
        {
            // Arrange
            var fakeHelper = A.Fake<IEmployeeRecordsControllerHelper>();
            var value = new EmployeeRecordsController(fakeHelper);
            A.CallTo(() => fakeHelper.GetSpecificEmployee(A<string>.Ignored)).Returns(_rob);

            // Act
            var result = value.SalaryView("rob.wilson@ascensus.com");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var x = ((ViewResult)result).ViewData.Model;
            Assert.AreEqual(_rob.salary, ((EmployeeModel)x).salary);
        }
    }
}