using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class GrupoEdificioSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public GrupoEdificioSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio");

            Assert.Contains("Grupo Edificio", _driver.PageSource);
            Assert.Contains("grupoEdificioTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioTable")));

            var table = _driver.FindElement(By.Id("grupoEdificioTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio");

            var addButton = _driver.FindElement(By.Id("grupoedificio_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio");

            var exportButton = _driver.FindElement(By.Id("grupoedificio_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio");

            var addButton = _driver.FindElement(By.Id("grupoedificio_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoedificio_create_form")));

            Assert.Contains("Crear Nuevo Grupo Edificio", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewGrupoEdificio()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoedificio_create_grupo")));

            var grupoField = _driver.FindElement(By.Id("grupoedificio_create_grupo"));

            string testGrupo = $"Test{DateTime.Now.Ticks.ToString().Substring(8)}";

            grupoField.SendKeys(testGrupo);

            var submitButton = _driver.FindElement(By.Id("grupoedificio_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/GrupoEdificio") && !d.Url.Contains("/Create"));
            Assert.Contains("/GrupoEdificio", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='grupoedificio_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='grupoedificio_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#grupoEdificioTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 2, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditGrupoEdificio()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='grupoedificio_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("grupoedificio_edit_form")));

                Assert.Contains("Editar Grupo Edificio", _driver.PageSource);

                var grupoField = _driver.FindElement(By.Id("grupoedificio_edit_grupo"));
                grupoField.Clear();
                grupoField.SendKeys($"Upd{DateTime.Now.Ticks.ToString().Substring(8)}");

                var submitButton = _driver.FindElement(By.Id("grupoedificio_edit_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/GrupoEdificio") && !d.Url.Contains("/Edit"));
                Assert.Contains("/GrupoEdificio", _driver.Url);
            }
        }

        [Fact]
        public void Test11_NavigateToDetailsPage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='grupoedificio_edit_']"));
            if (editButtons.Count > 0)
            {
                // Extract the ID from the edit button and navigate to details
                var editUrl = editButtons[0].GetAttribute("href");
                var idMatch = System.Text.RegularExpressions.Regex.Match(editUrl, @"Edit/(\d+)");
                if (idMatch.Success)
                {
                    var id = idMatch.Groups[1].Value;
                    _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio/Details/{id}");

                    wait.Until(d => d.FindElement(By.Id("grupoedificio_details_back")));
                    Assert.Contains("Detalles del Grupo Edificio", _driver.PageSource);
                }
            }
        }

        [Fact]
        public void Test12_DeleteGrupoEdificio()
        {
            // First create a new record to delete
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoedificio_create_grupo")));

            var grupoField = _driver.FindElement(By.Id("grupoedificio_create_grupo"));
            string testGrupo = $"DelTest{DateTime.Now.Ticks.ToString().Substring(8)}";
            grupoField.SendKeys(testGrupo);

            var submitButton = _driver.FindElement(By.Id("grupoedificio_create_submit"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);
            submitButton.Click();

            wait.Until(d => d.Url.Contains("/GrupoEdificio") && !d.Url.Contains("/Create"));

            // Now find and delete the record
            Thread.Sleep(2000);

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='grupoedificio_delete_']"));
            if (deleteButtons.Count > 0)
            {
                var deleteBtn = deleteButtons[deleteButtons.Count - 1];
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", deleteBtn);
                Thread.Sleep(500);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", deleteBtn);

                wait.Until(d => d.FindElement(By.Id("grupoedificio_delete_form")));
                Assert.Contains("Eliminar Grupo Edificio", _driver.PageSource);

                var deleteSubmit = _driver.FindElement(By.Id("grupoedificio_delete_submit"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", deleteSubmit);
                Thread.Sleep(500);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", deleteSubmit);

                wait.Until(d => d.Url.Contains("/GrupoEdificio") && !d.Url.Contains("/Delete"));
                Assert.Contains("/GrupoEdificio", _driver.Url);
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
