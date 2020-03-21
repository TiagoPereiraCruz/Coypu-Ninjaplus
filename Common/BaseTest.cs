using System;
using System.IO;
using Coypu;
using Coypu.Drivers.Selenium;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace NinjaPlus.Common
{
  public class BaseTest
  {
    protected BrowserSession Browser;

    [SetUp]
    public void BaseSetup()
    {
      var config = new ConfigurationBuilder().AddJsonFile("config.json").Build();

      var sessionConfig = new SessionConfiguration
      {
        AppHost = "http://ninjaplus-web",
        Port = 5000,
        SSL = false,
        Driver = typeof(SeleniumWebDriver),
        Timeout = TimeSpan.FromSeconds(10)
      };

      if (config["browser"].Equals("Chrome"))
      {
        sessionConfig.Browser = Coypu.Drivers.Browser.Chrome;
      }

      if (config["browser"].Equals("Firefox"))
      {
        sessionConfig.Browser = Coypu.Drivers.Browser.Firefox;
      }

      Browser = new BrowserSession(sessionConfig);
      Browser.MaximiseWindow();
    }

    public string CoverPath()
    {
      var outputPath = Environment.CurrentDirectory;
      return outputPath + "/Images/";
    }

    public void TakeScreenshot()
    {
      var resultId = TestContext.CurrentContext.Test.ID;
      var shotPath = Environment.CurrentDirectory + "/screenshots";

      if(!Directory.Exists(shotPath))
      {
        Directory.CreateDirectory(shotPath);
      }

      var screenshot = $"{shotPath}/{resultId}.png";

      Browser.SaveScreenshot(screenshot);
      TestContext.AddTestAttachment(screenshot);
    }

    [TearDown]
    public void Finish()
    {
      try
      {
        this.TakeScreenshot();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Ocorreu um erro ao capturar o screenshot :(");
        throw new Exception(ex.Message);
      }
      finally
      {
        Browser.Dispose();
      }
    }
  }
}

// publica => public (ela pode ser acessada por qualquer código ou projeto)
// privada => private (ela pode ser acessada somente pela classe que ela está) 
// protejida => protected (ela pode ser acessada somente por ela ou por um filho herdado)