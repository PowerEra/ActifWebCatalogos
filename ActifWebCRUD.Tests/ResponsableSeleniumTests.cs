using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class ResponsableSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public ResponsableSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable");

            Assert.Contains("Responsables", _driver.PageSource);
            Assert.Contains("responsable_datatable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("responsable_datatable")));

            var table = _driver.FindElement(By.Id("responsable_datatable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable");

            var addButton = _driver.FindElement(By.Id("responsable_agregar"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable");

            var exportButton = _driver.FindElement(By.Id("responsable_exportar_excel"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable");

            var addButton = _driver.FindElement(By.Id("responsable_agregar"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("responsable_create_form")));

            Assert.Contains("Crear Responsable", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewResponsable()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("responsable_create_nombre")));

            // Select compania from dropdown
            var companiaDropdown = _driver.FindElement(By.Id("responsable_create_idcompania"));
            var selectCompania = new SelectElement(companiaDropdown);
            if (selectCompania.Options.Count > 1)
            {
                selectCompania.SelectByIndex(1); // Select first option after default
            }

            // Select centro costo from dropdown
            var centroCostoDropdown = _driver.FindElement(By.Id("responsable_create_idcentrocosto"));
            var selectCentroCosto = new SelectElement(centroCostoDropdown);
            if (selectCentroCosto.Options.Count > 1)
            {
                selectCentroCosto.SelectByIndex(1); // Select first option after default
            }

            var nombreField = _driver.FindElement(By.Id("responsable_create_nombre"));
            string testNombre = $"TestResp{DateTime.Now.Ticks.ToString().Substring(8)}";
            nombreField.SendKeys(testNombre);

            var puestoField = _driver.FindElement(By.Id("responsable_create_puesto"));
            puestoField.SendKeys("Test Puesto");

            var numeroEmpleadoField = _driver.FindElement(By.Id("responsable_create_numeroempleado"));
            numeroEmpleadoField.SendKeys("99999");

            var submitButton = _driver.FindElement(By.Id("responsable_create_guardar"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/Responsable") && !d.Url.Contains("/Create"));
            Assert.Contains("/Responsable", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("responsable_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='responsable_editar_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("responsable_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='responsable_eliminar_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("responsable_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var filterInputs = _driver.FindElements(By.CssSelector("#responsable_datatable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 5, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditResponsable()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("responsable_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='responsable_editar_']"));
            if (editButtons.Count > 0)
            {
                // Scroll to the edit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButtons[0]);
                Thread.Sleep(500);

                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("responsable_edit_form")));

                Assert.Contains("Editar Responsable", _driver.PageSource);

                var nombreField = _driver.FindElement(By.Id("responsable_edit_nombre"));
                nombreField.Clear();
                nombreField.SendKeys($"Updated{DateTime.Now.Ticks.ToString().Substring(8)}");

                var submitButton = _driver.FindElement(By.Id("responsable_edit_guardar"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/Responsable") && !d.Url.Contains("/Edit"));
                Assert.Contains("/Responsable", _driver.Url);
            }
        }

        [Fact]
        public void Test11_ViewDetails()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("responsable_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var detailsButtons = _driver.FindElements(By.CssSelector("a[id^='responsable_detalles_']"));
            if (detailsButtons.Count > 0)
            {
                // Scroll to the details button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", detailsButtons[0]);
                Thread.Sleep(500);

                detailsButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("responsable_details_idresponsable")));

                Assert.Contains("Detalles del Responsable", _driver.PageSource);
            }
        }

        [Fact]
        public void Test12_DeleteResponsable()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Responsable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("responsable_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='responsable_eliminar_']"));
            if (deleteButtons.Count > 0)
            {
                // Scroll to the delete button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", deleteButtons[deleteButtons.Count - 1]);
                Thread.Sleep(500);

                deleteButtons[deleteButtons.Count - 1].Click();

                wait.Until(d => d.FindElement(By.Id("responsable_delete_form")));

                Assert.Contains("Eliminar Responsable", _driver.PageSource);

                var confirmButton = _driver.FindElement(By.Id("responsable_delete_confirmar"));

                // Scroll to the confirm button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", confirmButton);
                Thread.Sleep(500);

                confirmButton.Click();

                wait.Until(d => d.Url.Contains("/Responsable") && !d.Url.Contains("/Delete"));
                Assert.Contains("/Responsable", _driver.Url);
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
