using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class ConceptoContableSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public ConceptoContableSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable");

            Assert.Contains("Conceptos Contables", _driver.PageSource);
            Assert.Contains("conceptoContableTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("conceptoContableTable")));

            var table = _driver.FindElement(By.Id("conceptoContableTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable");

            var addButton = _driver.FindElement(By.Id("conceptocontable_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable");

            var exportButton = _driver.FindElement(By.Id("conceptocontable_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable");

            var addButton = _driver.FindElement(By.Id("conceptocontable_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("conceptocontable_create_form")));

            Assert.Contains("Crear Nuevo Concepto Contable", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewConceptoContable()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("conceptocontable_create_idconcepto")));

            // Fill in the ID Concepto
            var idConceptoField = _driver.FindElement(By.Id("conceptocontable_create_idconcepto"));
            var descripcionField = _driver.FindElement(By.Id("conceptocontable_create_descripcion"));

            short testIdConcepto = (short)(30000 + DateTime.Now.Ticks % 10000);
            string testDescripcion = $"Test Concepto {DateTime.Now.Ticks}";

            idConceptoField.SendKeys(testIdConcepto.ToString());
            descripcionField.SendKeys(testDescripcion);

            // Select a company if available
            var companiaSelect = _driver.FindElement(By.Id("conceptocontable_create_idcompania"));
            var selectCompania = new SelectElement(companiaSelect);
            if (selectCompania.Options.Count > 1)
            {
                selectCompania.SelectByIndex(1);
            }

            var submitButton = _driver.FindElement(By.Id("conceptocontable_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/ConceptoContable") && !d.Url.Contains("/Create"));
            Assert.Contains("/ConceptoContable", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("conceptoContableTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='conceptocontable_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("conceptoContableTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='conceptocontable_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("conceptoContableTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#conceptoContableTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 9, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_VerifyEditPageLoad()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("conceptoContableTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='conceptocontable_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("conceptocontable_edit_form")));
                Assert.Contains("Editar Concepto Contable", _driver.PageSource);
            }
        }

        [Fact]
        public void Test11_VerifyDeletePageLoad()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("conceptoContableTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='conceptocontable_delete_']"));
            if (deleteButtons.Count > 0)
            {
                deleteButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("conceptocontable_delete_form")));
                Assert.Contains("Eliminar Concepto Contable", _driver.PageSource);
            }
        }

        [Fact]
        public void Test12_VerifyCancelButtonWorks()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ConceptoContable/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("conceptocontable_create_form")));

            var cancelButton = _driver.FindElement(By.Id("conceptocontable_create_cancel"));
            cancelButton.Click();

            wait.Until(d => d.Url.Contains("/ConceptoContable") && !d.Url.Contains("/Create"));
            Assert.DoesNotContain("/Create", _driver.Url);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
