using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class UsersCampaniasSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public UsersCampaniasSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias");

            Assert.Contains("Usuarios Companias", _driver.PageSource);
            Assert.Contains("usersCampaniasTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usersCampaniasTable")));

            var table = _driver.FindElement(By.Id("usersCampaniasTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias");

            var addButton = _driver.FindElement(By.Id("userscampanias_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias");

            var exportButton = _driver.FindElement(By.Id("userscampanias_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias");

            var addButton = _driver.FindElement(By.Id("userscampanias_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("userscampanias_create_form")));

            Assert.Contains("Crear Nueva Relacion Usuario-Compania", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewUsersCampanias()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("userscampanias_create_iduser")));

            // Select a user
            var userSelect = _driver.FindElement(By.Id("userscampanias_create_iduser"));
            var userSelectElement = new SelectElement(userSelect);
            if (userSelectElement.Options.Count > 1)
            {
                userSelectElement.SelectByIndex(1);
            }

            // Select a company
            var companiaSelect = _driver.FindElement(By.Id("userscampanias_create_idcompania"));
            var companiaSelectElement = new SelectElement(companiaSelect);
            if (companiaSelectElement.Options.Count > 1)
            {
                companiaSelectElement.SelectByIndex(1);
            }

            var submitButton = _driver.FindElement(By.Id("userscampanias_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/UsersCampanias") && !d.Url.Contains("/Create"));
            Assert.Contains("/UsersCampanias", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usersCampaniasTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='userscampanias_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usersCampaniasTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='userscampanias_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usersCampaniasTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#usersCampaniasTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 3, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_VerifyEditPageLoad()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usersCampaniasTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='userscampanias_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("userscampanias_edit_form")));
                Assert.Contains("Editar Relacion Usuario-Compania", _driver.PageSource);
            }
        }

        [Fact]
        public void Test11_VerifyDeletePageLoad()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("usersCampaniasTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='userscampanias_delete_']"));
            if (deleteButtons.Count > 0)
            {
                deleteButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("userscampanias_delete_form")));
                Assert.Contains("Eliminar Relacion Usuario-Compania", _driver.PageSource);
            }
        }

        [Fact]
        public void Test12_VerifyCancelButtonWorks()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/UsersCampanias/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("userscampanias_create_form")));

            var cancelButton = _driver.FindElement(By.Id("userscampanias_create_cancel"));
            cancelButton.Click();

            wait.Until(d => d.Url.Contains("/UsersCampanias") && !d.Url.Contains("/Create"));
            Assert.DoesNotContain("/Create", _driver.Url);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
