using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class TipoActivoSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public TipoActivoSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoActivo");

            Assert.Contains("Tipos de Activo", _driver.PageSource);
            Assert.Contains("tipoActivoTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoActivo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoActivoTable")));

            var table = _driver.FindElement(By.Id("tipoActivoTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoActivo");

            var addButton = _driver.FindElement(By.Id("tipoactivo_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoActivo");

            var exportButton = _driver.FindElement(By.Id("tipoactivo_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoActivo");

            var addButton = _driver.FindElement(By.Id("tipoactivo_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoactivo_create_form")));

            Assert.Contains("Crear Nuevo Tipo de Activo", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewTipoActivo()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoActivo/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoactivo_create_descripcion")));

            var idField = _driver.FindElement(By.Id("tipoactivo_create_idtipoactivo"));
            var descripcionField = _driver.FindElement(By.Id("tipoactivo_create_descripcion"));
            var companiaDropdown = _driver.FindElement(By.Id("tipoactivo_create_idcompania"));

            string testId = DateTime.Now.Ticks.ToString().Substring(10);
            string testDescripcion = $"Test Activo {DateTime.Now.Ticks}";

            idField.SendKeys(testId);
            descripcionField.SendKeys(testDescripcion);

            // Select a compania if available (skip the first option which is the placeholder)
            var selectElement = new SelectElement(companiaDropdown);
            if (selectElement.Options.Count > 1)
            {
                selectElement.SelectByIndex(1);
            }

            var submitButton = _driver.FindElement(By.Id("tipoactivo_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/TipoActivo"));
            Assert.Contains("/TipoActivo", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoActivo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoActivoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='tipoactivo_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoActivo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoActivoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='tipoactivo_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoActivo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoActivoTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#tipoActivoTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 3, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditExistingTipoActivo()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoActivo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoActivoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            // Find the first edit button
            var editButtons = _driver.FindElements(By.CssSelector("a[id^='tipoactivo_edit_']"));
            if (editButtons.Count > 0)
            {
                // Scroll to the edit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButtons[0]);
                Thread.Sleep(500);

                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("tipoactivo_edit_form")));
                Assert.Contains("Editar Tipo de Activo", _driver.PageSource);

                var descripcionField = _driver.FindElement(By.Id("tipoactivo_edit_descripcion"));
                descripcionField.Clear();
                descripcionField.SendKeys($"Updated {DateTime.Now.Ticks}");

                var submitButton = _driver.FindElement(By.Id("tipoactivo_edit_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                // Wait a bit longer for redirect
                Thread.Sleep(2000);
                wait.Until(d => d.Url.EndsWith("/TipoActivo") || d.Url.Contains("/TipoActivo?"));
                Assert.Contains("/TipoActivo", _driver.Url);
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
