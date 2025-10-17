using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class TipoCalculoSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public TipoCalculoSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo");

            Assert.Contains("Tipos de Cálculo", _driver.PageSource);
            Assert.Contains("tipocalculo_datatable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipocalculo_datatable")));

            var table = _driver.FindElement(By.Id("tipocalculo_datatable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo");

            var addButton = _driver.FindElement(By.Id("tipocalculo_agregar"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo");

            var exportButton = _driver.FindElement(By.Id("tipocalculo_exportar_excel"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo");

            var addButton = _driver.FindElement(By.Id("tipocalculo_agregar"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipocalculo_crear_form")));

            Assert.Contains("Crear Tipo de Cálculo", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewTipoCalculo()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipocalculo_crear_descripcion")));

            var descripcionField = _driver.FindElement(By.Id("tipocalculo_crear_descripcion"));
            string testDescripcion = $"TestCalc{DateTime.Now.Ticks.ToString().Substring(8)}";
            descripcionField.SendKeys(testDescripcion);

            var submitButton = _driver.FindElement(By.Id("tipocalculo_crear_guardar"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/TipoCalculo") && !d.Url.Contains("/Create"));
            Assert.Contains("/TipoCalculo", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipocalculo_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='tipocalculo_editar_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipocalculo_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='tipocalculo_eliminar_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipocalculo_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var filterInputs = _driver.FindElements(By.CssSelector("#tipocalculo_datatable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 2, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditTipoCalculo()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipocalculo_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='tipocalculo_editar_']"));
            if (editButtons.Count > 0)
            {
                // Scroll to the edit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButtons[0]);
                Thread.Sleep(500);

                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("tipocalculo_editar_form")));

                Assert.Contains("Editar Tipo de Cálculo", _driver.PageSource);

                var descripcionField = _driver.FindElement(By.Id("tipocalculo_editar_descripcion"));
                descripcionField.Clear();
                descripcionField.SendKeys($"Updated{DateTime.Now.Ticks.ToString().Substring(8)}");

                var submitButton = _driver.FindElement(By.Id("tipocalculo_editar_guardar"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/TipoCalculo") && !d.Url.Contains("/Edit"));
                Assert.Contains("/TipoCalculo", _driver.Url);
            }
        }

        [Fact]
        public void Test11_ViewDetails()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipocalculo_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var detailsButtons = _driver.FindElements(By.CssSelector("a[id^='tipocalculo_detalles_']"));
            if (detailsButtons.Count > 0)
            {
                // Scroll to the details button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", detailsButtons[0]);
                Thread.Sleep(500);

                detailsButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("tipocalculo_detalles_idtipocalculo")));

                Assert.Contains("Detalles del Tipo de Cálculo", _driver.PageSource);
            }
        }

        [Fact]
        public void Test12_DeleteTipoCalculo()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/TipoCalculo");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("tipocalculo_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='tipocalculo_eliminar_']"));
            if (deleteButtons.Count > 0)
            {
                // Scroll to the delete button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", deleteButtons[deleteButtons.Count - 1]);
                Thread.Sleep(500);

                deleteButtons[deleteButtons.Count - 1].Click();

                wait.Until(d => d.FindElement(By.Id("tipocalculo_eliminar_form")));

                Assert.Contains("Eliminar Tipo de Cálculo", _driver.PageSource);

                var confirmButton = _driver.FindElement(By.Id("tipocalculo_eliminar_confirmar"));

                // Scroll to the confirm button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", confirmButton);
                Thread.Sleep(500);

                confirmButton.Click();

                wait.Until(d => d.Url.Contains("/TipoCalculo") && !d.Url.Contains("/Delete"));
                Assert.Contains("/TipoCalculo", _driver.Url);
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
