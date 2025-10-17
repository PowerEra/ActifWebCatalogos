using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class PeriodosCerradosSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public PeriodosCerradosSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados");

            Assert.Contains("Periodos Cerrados", _driver.PageSource);
            Assert.Contains("periodosCerradosTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("periodosCerradosTable")));

            var table = _driver.FindElement(By.Id("periodosCerradosTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados");

            var addButton = _driver.FindElement(By.Id("periodoscerrados_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados");

            var exportButton = _driver.FindElement(By.Id("periodoscerrados_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados");

            var addButton = _driver.FindElement(By.Id("periodoscerrados_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("periodoscerrados_create_form")));

            Assert.Contains("Crear Periodo Cerrado", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewPeriodoCerrado()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("periodoscerrados_create_anio")));

            var anioField = _driver.FindElement(By.Id("periodoscerrados_create_anio"));
            var mesField = _driver.FindElement(By.Id("periodoscerrados_create_mes"));
            var companiaSelect = _driver.FindElement(By.Id("periodoscerrados_create_idcompania"));

            anioField.SendKeys("2024");
            mesField.SendKeys("10");

            // Select first available company
            var selectElement = new SelectElement(companiaSelect);
            if (selectElement.Options.Count > 1)
            {
                selectElement.SelectByIndex(1);

                var submitButton = _driver.FindElement(By.Id("periodoscerrados_create_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/PeriodosCerrados") && !d.Url.Contains("/Create"));
                Assert.Contains("/PeriodosCerrados", _driver.Url);
            }
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("periodosCerradosTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='periodoscerrados_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("periodosCerradosTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='periodoscerrados_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("periodosCerradosTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#periodosCerradosTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 5, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditPeriodoCerrado()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("periodosCerradosTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='periodoscerrados_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("periodoscerrados_edit_form")));

                Assert.Contains("Editar Periodo Cerrado", _driver.PageSource);

                var anioField = _driver.FindElement(By.Id("periodoscerrados_edit_anio"));
                anioField.Clear();
                anioField.SendKeys("2025");

                var submitButton = _driver.FindElement(By.Id("periodoscerrados_edit_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/PeriodosCerrados") && !d.Url.Contains("/Edit"));
                Assert.Contains("/PeriodosCerrados", _driver.Url);
            }
        }

        [Fact]
        public void Test11_VerifyCreateFormFields()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("periodoscerrados_create_form")));

            var anioField = _driver.FindElement(By.Id("periodoscerrados_create_anio"));
            var mesField = _driver.FindElement(By.Id("periodoscerrados_create_mes"));
            var companiaSelect = _driver.FindElement(By.Id("periodoscerrados_create_idcompania"));

            Assert.NotNull(anioField);
            Assert.NotNull(mesField);
            Assert.NotNull(companiaSelect);
        }

        [Fact]
        public void Test12_VerifyValidationOnCreate()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PeriodosCerrados/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("periodoscerrados_create_form")));

            var submitButton = _driver.FindElement(By.Id("periodoscerrados_create_submit"));

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
