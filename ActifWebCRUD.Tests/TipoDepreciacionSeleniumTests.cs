using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class TipoDepreciacionSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public TipoDepreciacionSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoDepreciacion");

            Assert.Contains("Tipos de Depreciacion", _driver.PageSource);
            Assert.Contains("tipoDepreciacionTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoDepreciacion");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoDepreciacionTable")));

            var table = _driver.FindElement(By.Id("tipoDepreciacionTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoDepreciacion");

            var addButton = _driver.FindElement(By.Id("tipodepreciacion_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoDepreciacion");

            var exportButton = _driver.FindElement(By.Id("tipodepreciacion_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoDepreciacion");

            var addButton = _driver.FindElement(By.Id("tipodepreciacion_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipodepreciacion_create_form")));

            Assert.Contains("Crear Nuevo Tipo de Depreciacion", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewTipoDepreciacion()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoDepreciacion/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipodepreciacion_create_descripcion")));

            var descripcionField = _driver.FindElement(By.Id("tipodepreciacion_create_descripcion"));
            var mesIniField = _driver.FindElement(By.Id("tipodepreciacion_create_mesini"));
            var aplicaFiscalField = _driver.FindElement(By.Id("tipodepreciacion_create_aplicafiscal"));

            string testDescripcion = $"Test Dep {DateTime.Now.Ticks}";

            descripcionField.SendKeys(testDescripcion);
            mesIniField.SendKeys("1");
            aplicaFiscalField.SendKeys("0");

            var submitButton = _driver.FindElement(By.Id("tipodepreciacion_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/TipoDepreciacion"));
            Assert.Contains("/TipoDepreciacion", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoDepreciacion");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoDepreciacionTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='tipodepreciacion_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoDepreciacion");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoDepreciacionTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='tipodepreciacion_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoDepreciacion");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoDepreciacionTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#tipoDepreciacionTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 4, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditExistingTipoDepreciacion()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoDepreciacion");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipoDepreciacionTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            // Find the first edit button
            var editButtons = _driver.FindElements(By.CssSelector("a[id^='tipodepreciacion_edit_']"));
            if (editButtons.Count > 0)
            {
                // Scroll to the edit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButtons[0]);
                Thread.Sleep(500);

                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("tipodepreciacion_edit_form")));
                Assert.Contains("Editar Tipo de Depreciacion", _driver.PageSource);

                var descripcionField = _driver.FindElement(By.Id("tipodepreciacion_edit_descripcion"));
                descripcionField.Clear();
                descripcionField.SendKeys($"Updated {DateTime.Now.Ticks}");

                var submitButton = _driver.FindElement(By.Id("tipodepreciacion_edit_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/TipoDepreciacion") && !d.Url.Contains("/Edit"));
                Assert.Contains("/TipoDepreciacion", _driver.Url);
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
