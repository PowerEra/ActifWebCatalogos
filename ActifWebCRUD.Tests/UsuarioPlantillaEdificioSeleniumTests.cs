using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class UsuarioPlantillaEdificioSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public UsuarioPlantillaEdificioSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio");

            Assert.Contains("Usuario Plantilla Edificio", _driver.PageSource);
            Assert.Contains("usuarioPlantillaEdificioTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioPlantillaEdificioTable")));

            var table = _driver.FindElement(By.Id("usuarioPlantillaEdificioTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio");

            var addButton = _driver.FindElement(By.Id("usuarioplantillaedificio_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio");

            var exportButton = _driver.FindElement(By.Id("usuarioplantillaedificio_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio");

            var addButton = _driver.FindElement(By.Id("usuarioplantillaedificio_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioplantillaedificio_create_form")));

            Assert.Contains("Crear Usuario Plantilla Edificio", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewUsuarioPlantillaEdificio()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioplantillaedificio_create_iduser")));

            var userSelect = _driver.FindElement(By.Id("usuarioplantillaedificio_create_iduser"));
            var plantillaSelect = _driver.FindElement(By.Id("usuarioplantillaedificio_create_idplantilla"));

            // Select first available user
            var userSelectElement = new SelectElement(userSelect);
            if (userSelectElement.Options.Count > 1)
            {
                userSelectElement.SelectByIndex(1);

                // Select first available plantilla
                var plantillaSelectElement = new SelectElement(plantillaSelect);
                if (plantillaSelectElement.Options.Count > 1)
                {
                    plantillaSelectElement.SelectByIndex(1);

                    var submitButton = _driver.FindElement(By.Id("usuarioplantillaedificio_create_submit"));

                    // Scroll to the submit button
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                    Thread.Sleep(500);

                    submitButton.Click();

                    wait.Until(d => d.Url.Contains("/UsuarioPlantillaEdificio") && !d.Url.Contains("/Create"));
                    Assert.Contains("/UsuarioPlantillaEdificio", _driver.Url);
                }
            }
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioPlantillaEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='usuarioplantillaedificio_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioPlantillaEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='usuarioplantillaedificio_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioPlantillaEdificioTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#usuarioPlantillaEdificioTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 4, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditUsuarioPlantillaEdificio()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioPlantillaEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='usuarioplantillaedificio_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("usuarioplantillaedificio_edit_form")));

                Assert.Contains("Editar Usuario Plantilla Edificio", _driver.PageSource);

                var plantillaSelect = _driver.FindElement(By.Id("usuarioplantillaedificio_edit_idplantilla"));
                var selectElement = new SelectElement(plantillaSelect);

                // Select different plantilla if available
                if (selectElement.Options.Count > 1)
                {
                    selectElement.SelectByIndex(1);

                    var submitButton = _driver.FindElement(By.Id("usuarioplantillaedificio_edit_submit"));

                    // Scroll to the submit button
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                    Thread.Sleep(500);

                    submitButton.Click();

                    wait.Until(d => d.Url.Contains("/UsuarioPlantillaEdificio") && !d.Url.Contains("/Edit"));
                    Assert.Contains("/UsuarioPlantillaEdificio", _driver.Url);
                }
            }
        }

        [Fact]
        public void Test11_VerifyCreateFormFields()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioplantillaedificio_create_form")));

            var userSelect = _driver.FindElement(By.Id("usuarioplantillaedificio_create_iduser"));
            var plantillaSelect = _driver.FindElement(By.Id("usuarioplantillaedificio_create_idplantilla"));

            Assert.NotNull(userSelect);
            Assert.NotNull(plantillaSelect);
        }

        [Fact]
        public void Test12_VerifyValidationOnCreate()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioPlantillaEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioplantillaedificio_create_form")));

            var submitButton = _driver.FindElement(By.Id("usuarioplantillaedificio_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            // Should stay on create page due to validation
            Thread.Sleep(1000);
            Assert.Contains("/Create", _driver.Url);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
