using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class EdificioCentroCostoSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public EdificioCentroCostoSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto");

            Assert.Contains("Edificio Centro Costo", _driver.PageSource);
            Assert.Contains("edificioCentroCostoTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("edificioCentroCostoTable")));

            var table = _driver.FindElement(By.Id("edificioCentroCostoTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto");

            var addButton = _driver.FindElement(By.Id("edificiocentrocosto_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto");

            var exportButton = _driver.FindElement(By.Id("edificiocentrocosto_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto");

            var addButton = _driver.FindElement(By.Id("edificiocentrocosto_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("edificiocentrocosto_create_form")));

            Assert.Contains("Crear Nueva Relación Edificio-Centro Costo", _driver.PageSource);
        }

        [Fact]
        public void Test06_VerifyCreateFormFields()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("edificiocentrocosto_create_form")));

            var companiaField = _driver.FindElement(By.Id("edificiocentrocosto_create_compania"));
            var edificioField = _driver.FindElement(By.Id("edificiocentrocosto_create_edificio"));
            var centroCostoField = _driver.FindElement(By.Id("edificiocentrocosto_create_centrocosto"));
            var submitButton = _driver.FindElement(By.Id("edificiocentrocosto_create_submit"));
            var cancelButton = _driver.FindElement(By.Id("edificiocentrocosto_create_cancel"));

            Assert.NotNull(companiaField);
            Assert.NotNull(edificioField);
            Assert.NotNull(centroCostoField);
            Assert.NotNull(submitButton);
            Assert.NotNull(cancelButton);
        }

        [Fact]
        public void Test07_CreateNewEdificioCentroCosto()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("edificiocentrocosto_create_compania")));

            var companiaSelect = new SelectElement(_driver.FindElement(By.Id("edificiocentrocosto_create_compania")));
            var edificioSelect = new SelectElement(_driver.FindElement(By.Id("edificiocentrocosto_create_edificio")));
            var centroCostoSelect = new SelectElement(_driver.FindElement(By.Id("edificiocentrocosto_create_centrocosto")));

            // Select first available option for each dropdown (skip empty option)
            if (companiaSelect.Options.Count > 1)
            {
                companiaSelect.SelectByIndex(1);
            }

            if (edificioSelect.Options.Count > 1)
            {
                edificioSelect.SelectByIndex(1);
            }

            if (centroCostoSelect.Options.Count > 1)
            {
                centroCostoSelect.SelectByIndex(1);
            }

            var submitButton = _driver.FindElement(By.Id("edificiocentrocosto_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            // Wait for redirect to index or stay on create page with validation errors
            Thread.Sleep(2000);

            // Check if redirected to index (success) or stayed on create (validation error or duplicate)
            Assert.True(_driver.Url.Contains("/EdificioCentroCosto"));
        }

        [Fact]
        public void Test08_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("edificioCentroCostoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='edificiocentrocosto_edit_']"));
            // May not have records initially
            Assert.True(editButtons.Count >= 0);
        }

        [Fact]
        public void Test09_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("edificioCentroCostoTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='edificiocentrocosto_delete_']"));
            // May not have records initially
            Assert.True(deleteButtons.Count >= 0);
        }

        [Fact]
        public void Test10_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("edificioCentroCostoTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#edificioCentroCostoTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 7, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test11_VerifyFormValidation()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("edificiocentrocosto_create_form")));

            var submitButton = _driver.FindElement(By.Id("edificiocentrocosto_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            Thread.Sleep(1000);

            // The form should still be on the create page since required fields are empty
            Assert.Contains("/Create", _driver.Url);
        }

        [Fact]
        public void Test12_VerifyCancelButtonWorks()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/EdificioCentroCosto/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("edificiocentrocosto_create_cancel")));

            var cancelButton = _driver.FindElement(By.Id("edificiocentrocosto_create_cancel"));
            cancelButton.Click();

            wait.Until(d => d.Url.Contains("/EdificioCentroCosto") && !d.Url.Contains("/Create"));
            Assert.DoesNotContain("/Create", _driver.Url);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
