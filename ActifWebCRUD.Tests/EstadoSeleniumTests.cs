using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class EstadoSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public EstadoSeleniumTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Fact]
        public void Test01_NavigateToIndex_ShouldLoadSuccessfully()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Estado");

            Assert.Contains("Estados", _driver.PageSource);
            Assert.Contains("estadoTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Estado");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoTable")));

            var table = _driver.FindElement(By.Id("estadoTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Estado");

            var addButton = _driver.FindElement(By.Id("estado_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Estado");

            var exportButton = _driver.FindElement(By.Id("estado_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Estado");

            var addButton = _driver.FindElement(By.Id("estado_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estado_create_form")));

            Assert.Contains("Crear Nuevo Estado", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewEstado()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Estado/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estado_create_nombre")));

            var nombreField = _driver.FindElement(By.Id("estado_create_nombre"));
            var paisDropdown = _driver.FindElement(By.Id("estado_create_idpais"));

            string testNombre = $"Test{DateTime.Now.Ticks.ToString().Substring(8)}";

            nombreField.SendKeys(testNombre);

            // Select first non-empty option from dropdown
            var selectElement = new SelectElement(paisDropdown);
            if (selectElement.Options.Count > 1)
            {
                selectElement.SelectByIndex(1);
            }

            var submitButton = _driver.FindElement(By.Id("estado_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/Estado"));
            Assert.Contains("/Estado", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Estado");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='estado_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Estado");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='estado_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Estado");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#estadoTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 3, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditEstado()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Estado");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='estado_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("estado_edit_form")));

                Assert.Contains("Editar Estado", _driver.PageSource);

                var nombreField = _driver.FindElement(By.Id("estado_edit_nombre"));
                nombreField.Clear();
                nombreField.SendKeys($"Updated{DateTime.Now.Ticks.ToString().Substring(8)}");

                var submitButton = _driver.FindElement(By.Id("estado_edit_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/Estado") && !d.Url.Contains("/Edit"));
                Assert.Contains("/Estado", _driver.Url);
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
