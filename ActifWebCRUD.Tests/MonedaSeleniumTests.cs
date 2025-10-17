using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class MonedaSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public MonedaSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda");

            Assert.Contains("Monedas", _driver.PageSource);
            Assert.Contains("moneda_datatable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("moneda_datatable")));

            var table = _driver.FindElement(By.Id("moneda_datatable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda");

            var addButton = _driver.FindElement(By.Id("moneda_agregar"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda");

            var exportButton = _driver.FindElement(By.Id("moneda_exportar_excel"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda");

            var addButton = _driver.FindElement(By.Id("moneda_agregar"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("moneda_create_form")));

            Assert.Contains("Crear Moneda", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewMoneda()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("moneda_create_nombre")));

            // Select a pais from dropdown
            var paisDropdown = _driver.FindElement(By.Id("moneda_create_idpais"));
            var selectElement = new SelectElement(paisDropdown);
            if (selectElement.Options.Count > 1)
            {
                selectElement.SelectByIndex(1); // Select first option after default
            }

            var nombreField = _driver.FindElement(By.Id("moneda_create_nombre"));
            string testNombre = $"Test{DateTime.Now.Ticks.ToString().Substring(8)}";
            nombreField.SendKeys(testNombre);

            var simboloField = _driver.FindElement(By.Id("moneda_create_simbolo"));
            simboloField.SendKeys("T$");

            var submitButton = _driver.FindElement(By.Id("moneda_create_guardar"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/Moneda") && !d.Url.Contains("/Create"));
            Assert.Contains("/Moneda", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("moneda_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='moneda_editar_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("moneda_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='moneda_eliminar_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("moneda_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var filterInputs = _driver.FindElements(By.CssSelector("#moneda_datatable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 3, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditMoneda()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("moneda_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='moneda_editar_']"));
            if (editButtons.Count > 0)
            {
                // Scroll to the edit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButtons[0]);
                Thread.Sleep(500);

                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("moneda_edit_form")));

                Assert.Contains("Editar Moneda", _driver.PageSource);

                var nombreField = _driver.FindElement(By.Id("moneda_edit_nombre"));
                nombreField.Clear();
                nombreField.SendKeys($"Updated{DateTime.Now.Ticks.ToString().Substring(8)}");

                var submitButton = _driver.FindElement(By.Id("moneda_edit_guardar"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/Moneda") && !d.Url.Contains("/Edit"));
                Assert.Contains("/Moneda", _driver.Url);
            }
        }

        [Fact]
        public void Test11_ViewDetails()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("moneda_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var detailsButtons = _driver.FindElements(By.CssSelector("a[id^='moneda_detalles_']"));
            if (detailsButtons.Count > 0)
            {
                // Scroll to the details button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", detailsButtons[0]);
                Thread.Sleep(500);

                detailsButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("moneda_details_idmoneda")));

                Assert.Contains("Detalles de Moneda", _driver.PageSource);
            }
        }

        [Fact]
        public void Test12_DeleteMoneda()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Moneda");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("moneda_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='moneda_eliminar_']"));
            if (deleteButtons.Count > 0)
            {
                // Scroll to the delete button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", deleteButtons[deleteButtons.Count - 1]);
                Thread.Sleep(500);

                deleteButtons[deleteButtons.Count - 1].Click();

                wait.Until(d => d.FindElement(By.Id("moneda_delete_form")));

                Assert.Contains("Eliminar Moneda", _driver.PageSource);

                var confirmButton = _driver.FindElement(By.Id("moneda_delete_confirmar"));

                // Scroll to the confirm button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", confirmButton);
                Thread.Sleep(500);

                confirmButton.Click();

                wait.Until(d => d.Url.Contains("/Moneda") && !d.Url.Contains("/Delete"));
                Assert.Contains("/Moneda", _driver.Url);
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
