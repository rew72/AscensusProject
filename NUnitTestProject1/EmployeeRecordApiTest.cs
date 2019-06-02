using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using api;
using api.Controllers;
using api.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using FileHelper = Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.FileHelper;

namespace NUnitTestProject1
{
    public class EmployeeRecordApiTest
    { 
        private readonly Contact _rob = new Contact()
        {
            firstName = "Rob",
            lastName = "Wilson",
            email = "rob.wilson@ascensus.com"
        };
        private readonly Contact _joe = new Contact()
        {
            firstName = "Joe",
            lastName = "Rue",
            email = "joe.rue@ascensus.com"
        };
        private readonly Contact _mark = new Contact()
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
        public void TestGetEmployees()
        {
            // Arrange
            var fakeHelper = A.Fake<IFileHelper>();

            List<Contact> exampleList = new List<Contact>()
            {
                _rob,
                _joe,
                _mark
            };

            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).Returns(exampleList);

            var value = new ValuesController(fakeHelper);

            // Act
            var result = value.GetEmployees();
            string jsonString = JsonConvert.SerializeObject(result.Value);
            List<Contact> employees = JsonConvert.DeserializeObject<List<Contact>>(jsonString);



            //Assert
            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.IsNotEmpty(jsonString);
            Assert.AreEqual(3, employees.Count);
            Assert.AreEqual(exampleList[0].email, employees[0].email);
            Assert.AreEqual(exampleList[1].email, employees[1].email);
            Assert.AreEqual(exampleList[2].email, employees[2].email);
            Assert.Pass();
        }

        [TestCase("rob.wilson@ascensus.com", 1)]
        [TestCase("joe.rue@ascensus.com", 2)]
        [TestCase("", 3)]
        [TestCase("NotRealEmail@ascensus.com", 3)]
        public void TestGetEmployee(string id, int occurence)
        {
            // Arrange
            var fakeHelper = A.Fake<IFileHelper>();

            List<Contact> exampleList = new List<Contact>()
            {
                _rob,
                _joe,
                _mark
            };

            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).Returns(exampleList);

            var value = new ValuesController(fakeHelper);

            // Act
            var result = value.Get(id);
            string JsonString = JsonConvert.SerializeObject(result.Value);
            Contact employee = new Contact();
            if (occurence == 3)
            {
                JsonString = JsonConvert.DeserializeObject<string>(JsonString);
            }
            else
            {
                employee = JsonConvert.DeserializeObject<Contact>(JsonString);
            }

            //Assert
            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.IsNotEmpty(JsonString);
            if (occurence == 1 || occurence == 2)
            {
                Contact testPerson = new Contact();

                if (occurence == 1)
                {
                    testPerson = _rob;
                }
                else if (occurence == 2)
                {
                    testPerson = _joe;
                }
                Assert.AreEqual(testPerson.email, employee.email);
                Assert.AreEqual(testPerson.firstName, employee.firstName);
                Assert.AreEqual(testPerson.lastName, employee.lastName);
            }
            else
            {
                Assert.AreEqual("ID NOT FOUND", JsonString);
            }
            Assert.Pass();
        }

        [TestCase()]
        [TestCase()]
        [TestCase()]
        public void Given_When_Then()
        {
            // Arrange
            var fakeHelper = A.Fake<IFileHelper>();

            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).Returns(new List<Contact>());

            var value = new ValuesController(fakeHelper);

            // Act
            var result = value.Get("Email");

            //Assert
            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Pass();
        }

        [Test]
        public void TestDelete()
        {
            // Arrange
            var fakeHelper = A.Fake<IFileHelper>();
            List<Contact> exampleList = new List<Contact>()
            {
                _rob,
                _joe,
                _mark
            };

            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).Returns(exampleList);
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.Ignored)).DoesNothing();

            

            var value = new ValuesController(fakeHelper);

            // Act
            value.DeleteEmployee("joe.rue@ascensus.com");

            //Assert
            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.That.Matches(x => x.Count==2))).MustHaveHappened();
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.That.Matches(x => x.Any(f => f.email == "joe@ascensus.com") == false))).MustHaveHappened();
            Assert.Pass();
        }

        [Test]
        public void TestCreate()
        {
            // Arrange
            var fakeHelper = A.Fake<IFileHelper>();
            List<Contact> exampleList = new List<Contact>()
            {
                _rob,
                _mark
            };

            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).Returns(exampleList);
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.Ignored)).DoesNothing();



            var value = new ValuesController(fakeHelper);

            // Act

            value.CreateEmployee(_joe);

            //Assert
            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.That.Matches(x => x.Count == 3))).MustHaveHappened();
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.That.Matches(x => x.Any(f => f.email == "joe.rue@ascensus.com") == true))).MustHaveHappened();
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.That.Matches(x => x.Any(f => f.firstName == "Joe") == true))).MustHaveHappened();
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.That.Matches(x => x.Any(f => f.lastName == "Rue") == true))).MustHaveHappened();
            Assert.Pass();
        }

        [Test]
        public void TestUpdate()
        {
            // Arrange
            var fakeHelper = A.Fake<IFileHelper>();
            List<Contact> exampleList = new List<Contact>()
            {
                _rob,
                _joe,
                _mark
            };

            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).Returns(exampleList);
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.Ignored)).DoesNothing();



            var value = new ValuesController(fakeHelper);

            _joe.lastName = "Faker";

            // Act

            value.UpdateEmployee(_joe);

            //Assert
            A.CallTo(() => fakeHelper.GetData<Contact>(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.That.Matches(x => x.Count == 3))).MustHaveHappened();
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.That.Matches(x => x.Any(f => f.email == "joe.rue@ascensus.com") == true))).MustHaveHappened();
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.That.Matches(x => x.Any(f => f.firstName == "Joe") == true))).MustHaveHappened();
            A.CallTo(() => fakeHelper.SetData<Contact>(A<string>.Ignored, A<List<Contact>>.That.Matches(x => x.Any(f => f.lastName == "Faker") == true))).MustHaveHappened();
            Assert.Pass();
        }

    }
}