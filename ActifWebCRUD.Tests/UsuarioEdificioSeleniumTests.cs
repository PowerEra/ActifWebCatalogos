using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class UsuarioEdificioSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public UsuarioEdificioSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio");

            Assert.Contains("Usuarios Edificios", _driver.PageSource);
            Assert.Contains("usuarioEdificioTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioEdificioTable")));

            var table = _driver.FindElement(By.Id("usuarioEdificioTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio");

            var addButton = _driver.FindElement(By.Id("usuarioedificio_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio");

            var exportButton = _driver.FindElement(By.Id("usuarioedificio_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio");

            var addButton = _driver.FindElement(By.Id("usuarioedificio_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioedificio_create_form")));

            Assert.Contains("Crear Nuevo Usuario Edificio", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewUsuarioEdificio()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioedificio_create_form")));

            // Wait for dropdowns to load
            Thread.Sleep(1000);

            var usuarioDropdown = _driver.FindElement(By.Id("usuarioedificio_create_idusuario"));
            var companiaDropdown = _driver.FindElement(By.Id("usuarioedificio_create_idcompania"));
            var edificioDropdown = _driver.FindElement(By.Id("usuarioedificio_create_idedificio"));

            // Select values from dropdowns
            var usuarioSelect = new SelectElement(usuarioDropdown);
            var companiaSelect = new SelectElement(companiaDropdown);
            var edificioSelect = new SelectElement(edificioDropdown);

            // Try to select the second option (first is the placeholder)
            if (usuarioSelect.Options.Count > 1)
            {
                usuarioSelect.SelectByIndex(1);
            }

            if (companiaSelect.Options.Count > 1)
            {
                companiaSelect.SelectByIndex(1);
            }

            if (edificioSelect.Options.Count > 1)
            {
                edificioSelect.SelectByIndex(1);
            }

            var submitButton = _driver.FindElement(By.Id("usuarioedificio_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/UsuarioEdificio"));
            Assert.Contains("/UsuarioEdificio", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='usuarioedificio_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='usuarioedificio_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioEdificioTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#usuarioEdificioTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 7, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_VerifyEditPageLoad()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='usuarioedificio_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("usuarioedificio_edit_form")));
                Assert.Contains("Editar Usuario Edificio", _driver.PageSource);
            }
        }

        [Fact]
        public void Test11_VerifyDeletePageLoad()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='usuarioedificio_delete_']"));
            if (deleteButtons.Count > 0)
            {
                deleteButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("usuarioedificio_delete_form")));
                Assert.Contains("Eliminar Usuario Edificio", _driver.PageSource);
            }
        }

        [Fact]
        public void Test12_VerifyCancelButtonWorks()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsuarioEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usuarioedificio_create_form")));

            var cancelButton = _driver.FindElement(By.Id("usuarioedificio_create_cancel"));
            cancelButton.Click();

            wait.Until(d => d.Url.Contains("/UsuarioEdificio") && !d.Url.Contains("/Create"));
            Assert.DoesNotContain("/Create", _driver.Url);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
