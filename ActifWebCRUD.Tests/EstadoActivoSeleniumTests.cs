using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class EstadoActivoSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public EstadoActivoSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/EstadoActivo");

            Assert.Contains("Estados de Activo", _driver.PageSource);
            Assert.Contains("estadoActivoTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EstadoActivo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoActivoTable")));

            var table = _driver.FindElement(By.Id("estadoActivoTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EstadoActivo");

            var addButton = _driver.FindElement(By.Id("estadoactivo_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EstadoActivo");

            var exportButton = _driver.FindElement(By.Id("estadoactivo_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EstadoActivo");

            var addButton = _driver.FindElement(By.Id("estadoactivo_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoactivo_create_form")));

            Assert.Contains("Crear Nuevo Estado de Activo", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewEstadoActivo()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EstadoActivo/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoactivo_create_descripcion")));

            var descripcionField = _driver.FindElement(By.Id("estadoactivo_create_descripcion"));

            string testDescripcion = $"Test{DateTime.Now.Ticks.ToString().Substring(8)}";

            descripcionField.SendKeys(testDescripcion);

            var submitButton = _driver.FindElement(By.Id("estadoactivo_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/EstadoActivo"));
            Assert.Contains("/EstadoActivo", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EstadoActivo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoActivoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='estadoactivo_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EstadoActivo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoActivoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='estadoactivo_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EstadoActivo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoActivoTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#estadoActivoTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 2, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditEstadoActivo()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EstadoActivo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("estadoActivoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='estadoactivo_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("estadoactivo_edit_form")));

                Assert.Contains("Editar Estado de Activo", _driver.PageSource);

                var descripcionField = _driver.FindElement(By.Id("estadoactivo_edit_descripcion"));
                descripcionField.Clear();
                descripcionField.SendKeys($"Updated{DateTime.Now.Ticks.ToString().Substring(8)}");

                var submitButton = _driver.FindElement(By.Id("estadoactivo_edit_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/EstadoActivo") && !d.Url.Contains("/Edit"));
                Assert.Contains("/EstadoActivo", _driver.Url);
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
