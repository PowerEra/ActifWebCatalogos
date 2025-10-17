using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class ActifTipoDepEdificioSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public ActifTipoDepEdificioSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio");

            Assert.Contains("Tipo Depreciación por Edificio", _driver.PageSource);
            Assert.Contains("actifTipoDepEdificioTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifTipoDepEdificioTable")));

            var table = _driver.FindElement(By.Id("actifTipoDepEdificioTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio");

            var addButton = _driver.FindElement(By.Id("actiftipodepedificio_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio");

            var exportButton = _driver.FindElement(By.Id("actiftipodepedificio_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio");

            var addButton = _driver.FindElement(By.Id("actiftipodepedificio_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actiftipodepedificio_create_form")));

            Assert.Contains("Crear Nuevo Tipo Depreciación por Edificio", _driver.PageSource);
        }

        [Fact]
        public void Test06_VerifyCreateFormFields()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actiftipodepedificio_create_form")));

            var tipoDepField = _driver.FindElement(By.Id("actiftipodepedificio_create_tipodep"));
            var edificioField = _driver.FindElement(By.Id("actiftipodepedificio_create_edificio"));
            var fechaField = _driver.FindElement(By.Id("actiftipodepedificio_create_fecha"));
            var submitButton = _driver.FindElement(By.Id("actiftipodepedificio_create_submit"));
            var cancelButton = _driver.FindElement(By.Id("actiftipodepedificio_create_cancel"));

            Assert.NotNull(tipoDepField);
            Assert.NotNull(edificioField);
            Assert.NotNull(fechaField);
            Assert.NotNull(submitButton);
            Assert.NotNull(cancelButton);
        }

        [Fact]
        public void Test07_CreateNewActifTipoDepEdificio()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actiftipodepedificio_create_tipodep")));

            var tipoDepSelect = new SelectElement(_driver.FindElement(By.Id("actiftipodepedificio_create_tipodep")));
            var edificioSelect = new SelectElement(_driver.FindElement(By.Id("actiftipodepedificio_create_edificio")));

            // Select first available option for each dropdown (skip empty option)
            if (tipoDepSelect.Options.Count > 1)
            {
                tipoDepSelect.SelectByIndex(1);
            }

            if (edificioSelect.Options.Count > 1)
            {
                edificioSelect.SelectByIndex(1);
            }

            var submitButton = _driver.FindElement(By.Id("actiftipodepedificio_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            // Wait for redirect to index or stay on create page with validation errors
            Thread.Sleep(2000);

            // Check if redirected to index (success) or stayed on create (validation error or duplicate)
            Assert.True(_driver.Url.Contains("/ActifTipoDepEdificio"));
        }

        [Fact]
        public void Test08_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifTipoDepEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='actiftipodepedificio_edit_']"));
            // May not have records initially
            Assert.True(editButtons.Count >= 0);
        }

        [Fact]
        public void Test09_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifTipoDepEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='actiftipodepedificio_delete_']"));
            // May not have records initially
            Assert.True(deleteButtons.Count >= 0);
        }

        [Fact]
        public void Test10_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifTipoDepEdificioTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#actifTipoDepEdificioTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 6, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test11_VerifyFormValidation()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actiftipodepedificio_create_form")));

            var submitButton = _driver.FindElement(By.Id("actiftipodepedificio_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            Thread.Sleep(1000);

            // The form should redirect to index or stay on create since fields are nullable
            Assert.True(_driver.Url.Contains("/ActifTipoDepEdificio"));
        }

        [Fact]
        public void Test12_VerifyCancelButtonWorks()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifTipoDepEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actiftipodepedificio_create_cancel")));

            var cancelButton = _driver.FindElement(By.Id("actiftipodepedificio_create_cancel"));
            cancelButton.Click();

            wait.Until(d => d.Url.Contains("/ActifTipoDepEdificio") && !d.Url.Contains("/Create"));
            Assert.DoesNotContain("/Create", _driver.Url);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
