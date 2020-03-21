using NUnit.Framework;
using NinjaPlus.Pages;
using NinjaPlus.Common;

namespace NinjaPlus.Tests
{
  public class LoginTests : BaseTest
  {
    private LoginPage _login;
    private Sidebar _side;

    [SetUp]
    public void Setup()
    {

      _login = new LoginPage(Browser);
      _side = new Sidebar(Browser);
    }

    [Test]
    [Category("Critical")]
    public void ShouldSeeLoggedUser()
    {
      _login.With("tiago@ninjaplus.com", "pwd123");
      Assert.AreEqual("Tiago", _side.LoggedUser());
    }

    [TestCase("tiago@ninjaplus.com", "123456", "Usuário e/ou senha inválidos")]
    [TestCase("404@ninjaplus.com", "123456", "Usuário e/ou senha inválidos")]
    [TestCase("", "123456", "Opps. Cadê o email?")]
    [TestCase("tiago@ninjaplus.com", "", "Opps. Cadê a senha?")]
    public void ShouldSeeAlertMessage(string email, string pass, string expectMessage)
    {
      _login.With(email, pass);
      Assert.AreEqual(expectMessage, _login.AlertMessage());
    }
  }
}