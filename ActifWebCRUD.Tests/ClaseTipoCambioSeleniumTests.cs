using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class ClaseTipoCambioSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public ClaseTipoCambioSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio");

            Assert.Contains("Clases Tipo de Cambio", _driver.PageSource);
            Assert.Contains("clasetipocambio_datatable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("clasetipocambio_datatable")));

            var table = _driver.FindElement(By.Id("clasetipocambio_datatable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio");

            var addButton = _driver.FindElement(By.Id("clasetipocambio_agregar"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio");

            var exportButton = _driver.FindElement(By.Id("clasetipocambio_exportar_excel"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio");

            var addButton = _driver.FindElement(By.Id("clasetipocambio_agregar"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("clasetipocambio_create_form")));

            Assert.Contains("Crear Clase Tipo de Cambio", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewClaseTipoCambio()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("clasetipocambio_create_idclasetipocam")));

            // Generate a unique ID using timestamp
            short testId = (short)(DateTime.Now.Ticks % 30000 + 1000);
            var idField = _driver.FindElement(By.Id("clasetipocambio_create_idclasetipocam"));
            idField.SendKeys(testId.ToString());

            var descripcionField = _driver.FindElement(By.Id("clasetipocambio_create_descripcion"));
            string testDescripcion = $"Test{DateTime.Now.Ticks.ToString().Substring(8)}";
            descripcionField.SendKeys(testDescripcion);

            var submitButton = _driver.FindElement(By.Id("clasetipocambio_create_guardar"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/ClaseTipoCambio") && !d.Url.Contains("/Create"));
            Assert.Contains("/ClaseTipoCambio", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("clasetipocambio_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='clasetipocambio_editar_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("clasetipocambio_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='clasetipocambio_eliminar_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("clasetipocambio_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var filterInputs = _driver.FindElements(By.CssSelector("#clasetipocambio_datatable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 2, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditClaseTipoCambio()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("clasetipocambio_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='clasetipocambio_editar_']"));
            if (editButtons.Count > 0)
            {
                // Scroll to the edit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButtons[0]);
                Thread.Sleep(500);

                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("clasetipocambio_edit_form")));

                Assert.Contains("Editar Clase Tipo de Cambio", _driver.PageSource);

                var descripcionField = _driver.FindElement(By.Id("clasetipocambio_edit_descripcion"));
                descripcionField.Clear();
                descripcionField.SendKeys($"Updated{DateTime.Now.Ticks.ToString().Substring(8)}");

                var submitButton = _driver.FindElement(By.Id("clasetipocambio_edit_guardar"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/ClaseTipoCambio") && !d.Url.Contains("/Edit"));
                Assert.Contains("/ClaseTipoCambio", _driver.Url);
            }
        }

        [Fact]
        public void Test11_ViewDetails()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("clasetipocambio_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var detailsButtons = _driver.FindElements(By.CssSelector("a[id^='clasetipocambio_detalles_']"));
            if (detailsButtons.Count > 0)
            {
                // Scroll to the details button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", detailsButtons[0]);
                Thread.Sleep(500);

                detailsButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("clasetipocambio_details_idclasetipocam")));

                Assert.Contains("Detalles de Clase Tipo de Cambio", _driver.PageSource);
            }
        }

        [Fact]
        public void Test12_DeleteClaseTipoCambio()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ClaseTipoCambio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("clasetipocambio_datatable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='clasetipocambio_eliminar_']"));
            if (deleteButtons.Count > 0)
            {
                // Scroll to the delete button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", deleteButtons[deleteButtons.Count - 1]);
                Thread.Sleep(500);

                deleteButtons[deleteButtons.Count - 1].Click();

                wait.Until(d => d.FindElement(By.Id("clasetipocambio_delete_form")));

                Assert.Contains("Eliminar Clase Tipo de Cambio", _driver.PageSource);

                var confirmButton = _driver.FindElement(By.Id("clasetipocambio_delete_confirmar"));

                // Scroll to the confirm button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", confirmButton);
                Thread.Sleep(500);

                confirmButton.Click();

                wait.Until(d => d.Url.Contains("/ClaseTipoCambio") && !d.Url.Contains("/Delete"));
                Assert.Contains("/ClaseTipoCambio", _driver.Url);
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
