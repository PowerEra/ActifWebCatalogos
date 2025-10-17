using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class ActifCompaniaTipoDepreciacionSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public ActifCompaniaTipoDepreciacionSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion");

            Assert.Contains("Compania Tipo Depreciacion", _driver.PageSource);
            Assert.Contains("actifCompaniaTipoDepreciacionTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifCompaniaTipoDepreciacionTable")));

            var table = _driver.FindElement(By.Id("actifCompaniaTipoDepreciacionTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion");

            var addButton = _driver.FindElement(By.Id("actifcompaniatipodepreciacion_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion");

            var exportButton = _driver.FindElement(By.Id("actifcompaniatipodepreciacion_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion");

            var addButton = _driver.FindElement(By.Id("actifcompaniatipodepreciacion_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifcompaniatipodepreciacion_create_form")));

            Assert.Contains("Crear Compania Tipo Depreciacion", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewActifCompaniaTipoDepreciacion()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifcompaniatipodepreciacion_create_idcompania")));

            var companiaSelect = _driver.FindElement(By.Id("actifcompaniatipodepreciacion_create_idcompania"));
            var tipoDepSelect = _driver.FindElement(By.Id("actifcompaniatipodepreciacion_create_idtipodep"));

            // Select first available company
            var companiaSelectElement = new SelectElement(companiaSelect);
            if (companiaSelectElement.Options.Count > 1)
            {
                companiaSelectElement.SelectByIndex(1);

                // Select first available tipo depreciacion
                var tipoDepSelectElement = new SelectElement(tipoDepSelect);
                if (tipoDepSelectElement.Options.Count > 1)
                {
                    tipoDepSelectElement.SelectByIndex(1);

                    var submitButton = _driver.FindElement(By.Id("actifcompaniatipodepreciacion_create_submit"));

                    // Scroll to the submit button
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                    Thread.Sleep(500);

                    submitButton.Click();

                    wait.Until(d => d.Url.Contains("/ActifCompaniaTipoDepreciacion") && !d.Url.Contains("/Create"));
                    Assert.Contains("/ActifCompaniaTipoDepreciacion", _driver.Url);
                }
            }
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifCompaniaTipoDepreciacionTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='actifcompaniatipodepreciacion_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifCompaniaTipoDepreciacionTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='actifcompaniatipodepreciacion_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifCompaniaTipoDepreciacionTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#actifCompaniaTipoDepreciacionTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 5, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditActifCompaniaTipoDepreciacion()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifCompaniaTipoDepreciacionTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='actifcompaniatipodepreciacion_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("actifcompaniatipodepreciacion_edit_form")));

                Assert.Contains("Editar Compania Tipo Depreciacion", _driver.PageSource);

                var tipoDepSelect = _driver.FindElement(By.Id("actifcompaniatipodepreciacion_edit_idtipodep"));
                var selectElement = new SelectElement(tipoDepSelect);

                // Select different tipo depreciacion if available
                if (selectElement.Options.Count > 1)
                {
                    selectElement.SelectByIndex(1);

                    var submitButton = _driver.FindElement(By.Id("actifcompaniatipodepreciacion_edit_submit"));

                    // Scroll to the submit button
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                    Thread.Sleep(500);

                    submitButton.Click();

                    wait.Until(d => d.Url.Contains("/ActifCompaniaTipoDepreciacion") && !d.Url.Contains("/Edit"));
                    Assert.Contains("/ActifCompaniaTipoDepreciacion", _driver.Url);
                }
            }
        }

        [Fact]
        public void Test11_VerifyCreateFormFields()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifcompaniatipodepreciacion_create_form")));

            var companiaSelect = _driver.FindElement(By.Id("actifcompaniatipodepreciacion_create_idcompania"));
            var tipoDepSelect = _driver.FindElement(By.Id("actifcompaniatipodepreciacion_create_idtipodep"));

            Assert.NotNull(companiaSelect);
            Assert.NotNull(tipoDepSelect);
        }

        [Fact]
        public void Test12_VerifyValidationOnCreate()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifCompaniaTipoDepreciacion/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifcompaniatipodepreciacion_create_form")));

            var submitButton = _driver.FindElement(By.Id("actifcompaniatipodepreciacion_create_submit"));

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
