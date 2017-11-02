using System.Web.Mvc;
using GildedRose.Api.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GildedRose.Api;

namespace GildedRose.Api.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
