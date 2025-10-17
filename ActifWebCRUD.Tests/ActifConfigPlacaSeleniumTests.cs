using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class ActifConfigPlacaSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public ActifConfigPlacaSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca");

            Assert.Contains("Configuracion de Placas", _driver.PageSource);
            Assert.Contains("actifConfigPlacaTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifConfigPlacaTable")));

            var table = _driver.FindElement(By.Id("actifConfigPlacaTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca");

            var addButton = _driver.FindElement(By.Id("actifconfigplaca_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca");

            var exportButton = _driver.FindElement(By.Id("actifconfigplaca_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca");

            var addButton = _driver.FindElement(By.Id("actifconfigplaca_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifconfigplaca_create_form")));

            Assert.Contains("Crear Nueva Configuracion de Placa", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewActifConfigPlaca()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifconfigplaca_create_prefijo")));

            // Select a company
            var companiaSelect = _driver.FindElement(By.Id("actifconfigplaca_create_idcompania"));
            var selectElement = new SelectElement(companiaSelect);
            if (selectElement.Options.Count > 1)
            {
                selectElement.SelectByIndex(1);
            }

            var prefijoField = _driver.FindElement(By.Id("actifconfigplaca_create_prefijo"));
            var minDigitosField = _driver.FindElement(By.Id("actifconfigplaca_create_mindigitos"));
            var maxDigitosField = _driver.FindElement(By.Id("actifconfigplaca_create_maxdigitos"));

            string testPrefijo = $"TST{DateTime.Now.Ticks % 1000}";

            prefijoField.SendKeys(testPrefijo);
            minDigitosField.SendKeys("10");
            maxDigitosField.SendKeys("20");

            var submitButton = _driver.FindElement(By.Id("actifconfigplaca_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/ActifConfigPlaca") && !d.Url.Contains("/Create"));
            Assert.Contains("/ActifConfigPlaca", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifConfigPlacaTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='actifconfigplaca_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifConfigPlacaTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='actifconfigplaca_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifConfigPlacaTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#actifConfigPlacaTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 5, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_VerifyEditPageLoad()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifConfigPlacaTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='actifconfigplaca_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("actifconfigplaca_edit_form")));
                Assert.Contains("Editar Configuracion de Placa", _driver.PageSource);
            }
        }

        [Fact]
        public void Test11_VerifyDeletePageLoad()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifConfigPlacaTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='actifconfigplaca_delete_']"));
            if (deleteButtons.Count > 0)
            {
                deleteButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("actifconfigplaca_delete_form")));
                Assert.Contains("Eliminar Configuracion de Placa", _driver.PageSource);
            }
        }

        [Fact]
        public void Test12_VerifyCancelButtonWorks()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ActifConfigPlaca/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("actifconfigplaca_create_form")));

            var cancelButton = _driver.FindElement(By.Id("actifconfigplaca_create_cancel"));
            cancelButton.Click();

            wait.Until(d => d.Url.Contains("/ActifConfigPlaca") && !d.Url.Contains("/Create"));
            Assert.DoesNotContain("/Create", _driver.Url);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
