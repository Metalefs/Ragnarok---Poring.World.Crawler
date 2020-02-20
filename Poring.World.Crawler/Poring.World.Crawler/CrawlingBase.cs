using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Persistency.Database;

namespace CrawlerBase
{
    public class CrawlerSelenium
    {
        public IWebDriver Driver;
        protected DBConnection MongoConnection = new DBConnection();

        public CrawlerSelenium()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
        }

        public CrawlerSelenium(string driverChromePath, string DefaultDownloadpath, bool hideBrowser)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", DefaultDownloadpath);
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(driverChromePath);

            if (hideBrowser)
            {
                chromeOptions.AddArgument("headless");
                service.HideCommandPromptWindow = true;
                Driver = new ChromeDriver(service, chromeOptions);
            }
            else
            {
                Driver = new ChromeDriver(driverChromePath, chromeOptions);
            }

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);
            Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(60);
        }

        public void GoToUrl(String url)
        {
            Driver.Navigate().GoToUrl(url);
            Console.WriteLine("Navigated to \"" + url + "\n");
        }

        public void GoToUrlWithWait(String url, string idElementeWait)
        {
            try
            {
                Driver.Navigate().GoToUrl(url);
            }
            catch (Exception ex)
            {
                WaitElement(By.Id(idElementeWait), 60);
            }
        }

        public void SetFocusDefaultContent()
        {
            Driver.SwitchTo().DefaultContent();
        }

        public void GoToIframe(String idIframe)
        {
            Driver.SwitchTo().Frame(idIframe);
            Thread.Sleep(3000);
        }

        public void SetValueById(String id, String value)
        {
            Driver.FindElement(By.Id(id)).SendKeys(value);
            Console.WriteLine($"Set the value {value} to element with id = {id}\n");
        }

        public void SetValueByName(String name, String value)
        {
            Driver.FindElement(By.Name(name)).SendKeys(value);
            Console.WriteLine($"Set the value {value} to element with name = {name}\n");
        }

        public void SetAttributeValueById(String attribute, String value, String ID)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            string Script = String.Format("document.getElementById(\"{0}\").setAttribute(\"1}\", \"{2}\");",
                ID, attribute, value);
            js.ExecuteScript(Script);
            Console.WriteLine($"Set the {attribute} to element with id = {ID} to {value}\n");
        }

        public void SetValueByIdWithJS(String id, String value)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            string Script = $"document.getElementById('{id}').value = '{value}';";
            js.ExecuteScript(Script);
            Console.WriteLine($"Set the value to element with id = {id} to {value}\n");
        }

        public void SetValueBySelectorWithJS(String selector, String value)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            string Script = $"document.querySelector(\"{selector}\").value = \"{value}\";";
            js.ExecuteScript(Script);
            Console.WriteLine($"Set the value to element with selector = {selector} to {value}\n");
        }

        public void SetRecaptchaInnerHtml(String value)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            string Script = String.Format("document.getElementById('g-recaptcha-response').innerHTML=\"{0}\";",
                value);
            js.ExecuteScript(Script);
            Console.WriteLine($"Set reCaptcha InnerHtml to {value} \n ");
        }

        public void SetRecaptcha1000InnerHtml(String value)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            string Script = String.Format("document.getElementById('g-recaptcha-response-100000').innerHTML=\"{0}\";",
                value);
            js.ExecuteScript(Script);
            Console.WriteLine($"Set reCaptcha1000 InnerHtml to {value} \n ");
        }

        public string GetFirstCaptchaDataSiteKey()
        {
            Console.WriteLine($"Getting first captcha datasite-key \n ");
            return Driver.FindElement(By.ClassName("g-recaptcha")).GetProperty("data-sitekey");
        }

        public void SubmitFormById(String ID)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            string Script = String.Format("document.getElementById('{0}').submit();",
                ID);
            js.ExecuteScript(Script);
        }

        public void SubmitFirstForm()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            string Script = "document.getElementsByTagName('form')[0].submit();";
            js.ExecuteScript(Script);
            Console.WriteLine($"Submited first form\n ");
        }

        public void SubmitFormByXPath(string XPath)
        {
            Driver.FindElement(By.XPath(XPath)).Submit();
            Console.WriteLine($"Submited form with XPath {XPath}\n ");
        }

        public void SetInnerHtmlById(string id, String value)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            string Script = String.Format("document.getElementById('" + id + "').innerHTML=\"{0}\";",
                value);
            js.ExecuteScript(Script);
        }

        public void SubmitJavaScriptFormById(String ID)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            string Script = String.Format("document.getElementById('{0}').submit();",
                ID);
            js.ExecuteScript(Script);
        }

        public void ClearValueById(String id)
        {
            Driver.FindElement(By.Id(id)).Clear();
            Console.WriteLine($"Cleared the element with id = {id} \n");
        }

        public void ClickInElementById(String id)
        {
            Driver.FindElement(By.Id(id)).Click();
            Console.WriteLine($"Clicked on element with id = {id} \n");
        }

        public void ClickInElementByClassname(String className)
        {
            Driver.FindElement(By.ClassName(className)).Click();
            Console.WriteLine($"Clicked on element with class = {className} \n");
        }

        public void ClickInElementByName(String name)
        {
            Driver.FindElement(By.Name(name)).Click();
            Console.WriteLine($"Clicked on element with name = {name} \n");
        }

        public void ClickInElementByXPath(String xpath)
        {
            Driver.FindElement(By.XPath(xpath)).Click();
            Console.WriteLine($"Clicked on element with XPath = {xpath} \n");
        }

        public String GetIframeLinkInDiv(String idDiv, String idIframe)
        {
            IWebElement div = Driver.FindElement(By.Id(idDiv));
            Console.WriteLine($"Got a link from a div with id = {idDiv}, from Iframe with id = {idIframe} \n");
            return div.FindElement(By.Id(idIframe)).GetAttribute("src");
        }

        public Boolean CheckIfElementExistsById(String id)
        {
            return ExistsElement(By.Id(id));
        }

        public Boolean CheckIfElementExistsByClassName(String className)
        {
            try
            {
                Driver.FindElement(By.ClassName(className));
                return true;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
        }

        public String GetIdElementByTagAndIndex(String tag, Int32 index)
        {
            return Driver.FindElements(By.TagName(tag))[index].GetAttribute("id");
        }

        public IWebElement GetElementByTagAndIndex(String tag, Int32 index)
        {
            return Driver.FindElements(By.TagName(tag))[index];
        }

        public IWebElement GetElementById(string id)
        {
            return Driver.FindElement(By.Id(id));
        }

        public IWebElement GetWebElementByClassName(string className)
        {
            return Driver.FindElement(By.ClassName(className));
        }

        public IWebElement GetElementByXpath(string xPath)
        {
            return Driver.FindElement(By.XPath(xPath));
        }


        public void Close()
        {
            this.Driver.Quit();
            Console.WriteLine($"Closed the driver \n");
        }

        public string GetAttributeElementById(string id, string attribute)
        {
            IWebElement elemento = Driver.FindElement(By.Id(id));
            return elemento.GetAttribute(attribute);
        }

        public string GetAttributeElementByClassname(string className, string attribute)
        {
            IWebElement elemento = Driver.FindElement(By.ClassName(className));
            return elemento.GetAttribute(attribute);
        }

        public string GetInnerTextByClassName(string className)
        {
            IWebElement elemento = GetWebElementByClassName(className);
            return elemento.Text;
        }

        public void ClearElementeById(string id)
        {
            Driver.FindElement(By.Id(id)).Clear();
        }

        public void Refresh()
        {
            Driver.Navigate().Refresh();
            Console.WriteLine($"Refreshed the page \n");
        }

        public List<System.Net.Cookie> GetAllCookies()
        {
            List<Cookie> cookies = this.Driver.Manage().Cookies.AllCookies.ToList();
            List<System.Net.Cookie> cookiesNet = new List<System.Net.Cookie>();

            foreach (Cookie cookie in cookies)
            {
                System.Net.Cookie cookieNet = new System.Net.Cookie();
                cookieNet.Name = cookie.Name;
                cookieNet.Value = cookie.Value;
                cookieNet.Domain = cookie.Domain;
                cookieNet.Path = cookie.Path;
                cookiesNet.Add(cookieNet);
            }

            return cookiesNet;
        }

        public void SetValueByXPath(string XPath, string value)
        {
            Driver.FindElement(By.XPath(XPath)).SendKeys(value);
            Console.WriteLine($"Set the value to element with XPath = {XPath} to {value}\n");
        }

        public object ExecutaComandoJavaScript(string ComandoJS)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            Console.WriteLine($"Executou Comando JS: {ComandoJS}");
            object result = js.ExecuteScript(ComandoJS);
            return result;
        }

        public void ScrollAteTopoTela()
        {
            ExecutaComandoJavaScript("window.scrollBy(0, -1000000)");
            Thread.Sleep(1000);
            Console.WriteLine("Subiu até o topo da tela \n");
        }

        public void ScrollAteBaseTela()
        {
            ExecutaComandoJavaScript("window.scrollBy(0, 1000000)");
            Thread.Sleep(1000);
            Console.WriteLine("Desceu até a base da tela \n");
        }

        public void Dispose()
        {
            Driver.Quit();
            GC.Collect();
        }

        public void ExibirTodasCelulasnoPrompt()
        {
            IList<IWebElement> allElement = Driver.FindElements(By.TagName("td"));
            foreach (IWebElement element in allElement)
            {
                string cellText = element.Text;
                Console.WriteLine(cellText);
            }
        }

        public bool BuscaTextonaPagina(string texto)
        {
            if (Driver.PageSource.Contains(texto))
                return true;
            else
                return false;
        }

        public void GoToWindow(string window)
        {
            Driver.SwitchTo().Window(window);
            Console.WriteLine("Focou na página padrão \n");
        }

        public void GoToLastWindow()
        {
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
            Console.WriteLine("Focou na última página aberta \n");
        }

        public void GoToDefaultWindow()
        {
            Driver.SwitchTo().DefaultContent();
            Console.WriteLine("Focou na página padrão \n");
        }

        private bool WaitElement(By element, int timeout)
        {
            try
            {
                Console.WriteLine($" - Waiting for the element {element.ToString()}");
                int timesToWait = timeout * 4; // Times to wait for 1/4 of a second.
                int waitedTimes = 0; // Times waited.
                                     // This setup timesout at 7 seconds. you can change the code to pass the 

                do
                {
                    waitedTimes++;
                    if (waitedTimes >= timesToWait)
                    {
                        Console.WriteLine($" -- Element not found within (" +
                        $"{(timesToWait * 0.25)} seconds). Canceling section...");
                        return false;
                    }
                    Thread.Sleep(250);
                } while (!ExistsElement(element));

                Console.WriteLine($" -- Element found. Continuing...");
                //  Thread.Sleep(1000); // may apply here
                return true;
            }
            catch { throw; }
        }

        public bool ExistsElement(By element)
        {
            try
            {
                Driver.FindElement(element);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void FecharJanelaDeImpressao()
        {
            IWebElement webElement = Driver.FindElement(By.TagName("body"));
            webElement.SendKeys(Keys.Tab);
            webElement.SendKeys(Keys.Enter);
            Driver.SwitchTo().Window(Driver.WindowHandles[0]);
            Console.WriteLine("Inseriu TAB e ENTER no <body>");
        }
        public string GetAlertText()
        {
            return Driver.SwitchTo().Alert().Text;
        }

        public void AcceptAlert()
        {
            Driver.SwitchTo().Alert().Accept();
        }

        public void DismissAlert()
        {
            Driver.SwitchTo().Alert().Dismiss();
        }

        public void WriteToAlert(string text)
        {
            Driver.SwitchTo().Alert().SendKeys(text);
        }
    }
}

