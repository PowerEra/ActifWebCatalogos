using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class SubtipoMovimientoSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public SubtipoMovimientoSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/SubtipoMovimiento");

            Assert.Contains("Subtipos de Movimiento", _driver.PageSource);
            Assert.Contains("subtipoMovimientoTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/SubtipoMovimiento");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("subtipoMovimientoTable")));

            var table = _driver.FindElement(By.Id("subtipoMovimientoTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/SubtipoMovimiento");

            var addButton = _driver.FindElement(By.Id("subtipomovimiento_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/SubtipoMovimiento");

            var exportButton = _driver.FindElement(By.Id("subtipomovimiento_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/SubtipoMovimiento");

            var addButton = _driver.FindElement(By.Id("subtipomovimiento_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("subtipomovimiento_create_form")));

            Assert.Contains("Crear Nuevo Subtipo de Movimiento", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewSubtipoMovimiento()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/SubtipoMovimiento/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("subtipomovimiento_create_idtipomov")));

            var tipoMovSelect = _driver.FindElement(By.Id("subtipomovimiento_create_idtipomov"));
            var selectElement = new SelectElement(tipoMovSelect);

            // Select first available option (not the empty one)
            if (selectElement.Options.Count > 1)
            {
                selectElement.SelectByIndex(1);
            }

            var descripcionField = _driver.FindElement(By.Id("subtipomovimiento_create_descripcion"));
            string testDescripcion = $"Test{DateTime.Now.Ticks.ToString().Substring(8)}";
            descripcionField.SendKeys(testDescripcion);

            var afectacionField = _driver.FindElement(By.Id("subtipomovimiento_create_afectacioninterfaz"));
            afectacionField.SendKeys("S");

            var flgVisualizarField = _driver.FindElement(By.Id("subtipomovimiento_create_flgvisualizar"));
            flgVisualizarField.SendKeys("S");

            var flgGrabahistField = _driver.FindElement(By.Id("subtipomovimiento_create_flggrabahist"));
            flgGrabahistField.SendKeys("S");

            var submitButton = _driver.FindElement(By.Id("subtipomovimiento_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/SubtipoMovimiento"));
            Assert.Contains("/SubtipoMovimiento", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/SubtipoMovimiento");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("subtipoMovimientoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='subtipomovimiento_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/SubtipoMovimiento");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("subtipoMovimientoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='subtipomovimiento_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/SubtipoMovimiento");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("subtipoMovimientoTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#subtipoMovimientoTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 6, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditSubtipoMovimiento()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/SubtipoMovimiento");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("subtipoMovimientoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='subtipomovimiento_edit_']"));
            if (editButtons.Count > 0)
            {
                // Scroll to the edit button before clicking
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButtons[0]);
                Thread.Sleep(500);
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("subtipomovimiento_edit_form")));

                Assert.Contains("Editar Subtipo de Movimiento", _driver.PageSource);

                var descripcionField = _driver.FindElement(By.Id("subtipomovimiento_edit_descripcion"));
                descripcionField.Clear();
                descripcionField.SendKeys($"Upd{DateTime.Now.Ticks.ToString().Substring(10)}");

                var submitButton = _driver.FindElement(By.Id("subtipomovimiento_edit_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/SubtipoMovimiento") && !d.Url.Contains("/Edit"));
                Assert.Contains("/SubtipoMovimiento", _driver.Url);
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
