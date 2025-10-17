using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class AreaSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public AreaSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area");

            Assert.Contains("Areas", _driver.PageSource);
            Assert.Contains("areaTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("areaTable")));

            var table = _driver.FindElement(By.Id("areaTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area");

            var addButton = _driver.FindElement(By.Id("area_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area");

            var exportButton = _driver.FindElement(By.Id("area_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area");

            var addButton = _driver.FindElement(By.Id("area_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("area_create_form")));

            Assert.Contains("Crear Nueva Area", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewArea()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("area_create_descripcion")));

            var descripcionField = _driver.FindElement(By.Id("area_create_descripcion"));
            var cta1Field = _driver.FindElement(By.Id("area_create_cta1"));

            string testDescripcion = $"Test Area {DateTime.Now.Ticks}";
            string testCta1 = "1234";

            descripcionField.SendKeys(testDescripcion);
            cta1Field.SendKeys(testCta1);

            var submitButton = _driver.FindElement(By.Id("area_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/Area"));
            Assert.Contains("/Area", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("areaTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='area_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("areaTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='area_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("areaTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#areaTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 8, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_VerifyEditPageLoad()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("areaTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='area_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("area_edit_form")));
                Assert.Contains("Editar Area", _driver.PageSource);
            }
        }

        [Fact]
        public void Test11_VerifyDeletePageLoad()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("areaTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='area_delete_']"));
            if (deleteButtons.Count > 0)
            {
                deleteButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("area_delete_form")));
                Assert.Contains("Eliminar Area", _driver.PageSource);
                Assert.Contains("Advertencia", _driver.PageSource);
            }
        }

        [Fact]
        public void Test12_VerifyCancelButtonWorks()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Area/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("area_create_form")));

            var cancelButton = _driver.FindElement(By.Id("area_create_cancel"));
            cancelButton.Click();

            wait.Until(d => d.Url.Contains("/Area") && !d.Url.Contains("/Create"));
            Assert.DoesNotContain("/Create", _driver.Url);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
