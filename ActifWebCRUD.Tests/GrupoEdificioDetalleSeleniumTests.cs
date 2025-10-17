using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class GrupoEdificioDetalleSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public GrupoEdificioDetalleSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle");

            Assert.Contains("Grupo Edificio Detalle", _driver.PageSource);
            Assert.Contains("grupoEdificioDetalleTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioDetalleTable")));

            var table = _driver.FindElement(By.Id("grupoEdificioDetalleTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle");

            var addButton = _driver.FindElement(By.Id("grupoedificiodetalle_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle");

            var exportButton = _driver.FindElement(By.Id("grupoedificiodetalle_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle");

            var addButton = _driver.FindElement(By.Id("grupoedificiodetalle_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoedificiodetalle_create_form")));

            Assert.Contains("Crear Grupo Edificio Detalle", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewGrupoEdificioDetalle()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoedificiodetalle_create_form")));

            // Select Grupo
            var grupoSelect = new SelectElement(_driver.FindElement(By.Id("grupoedificiodetalle_create_idgrupo")));
            if (grupoSelect.Options.Count > 1)
            {
                grupoSelect.SelectByIndex(1);
            }

            // Select Edificio
            var edificioSelect = new SelectElement(_driver.FindElement(By.Id("grupoedificiodetalle_create_idedificio")));
            if (edificioSelect.Options.Count > 1)
            {
                edificioSelect.SelectByIndex(1);
            }

            var submitButton = _driver.FindElement(By.Id("grupoedificiodetalle_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/GrupoEdificioDetalle") && !d.Url.Contains("/Create"));
            Assert.Contains("/GrupoEdificioDetalle", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioDetalleTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='grupoedificiodetalle_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioDetalleTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='grupoedificiodetalle_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioDetalleTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#grupoEdificioDetalleTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 3, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditGrupoEdificioDetalle()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioDetalleTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='grupoedificiodetalle_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("grupoedificiodetalle_edit_form")));

                Assert.Contains("Editar Grupo Edificio Detalle", _driver.PageSource);

                // Select different Grupo if available
                var grupoSelect = new SelectElement(_driver.FindElement(By.Id("grupoedificiodetalle_edit_idgrupo")));
                if (grupoSelect.Options.Count > 2)
                {
                    grupoSelect.SelectByIndex(2);
                }

                var submitButton = _driver.FindElement(By.Id("grupoedificiodetalle_edit_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/GrupoEdificioDetalle") && !d.Url.Contains("/Edit"));
                Assert.Contains("/GrupoEdificioDetalle", _driver.Url);
            }
        }

        [Fact]
        public void Test11_NavigateToDetailsPage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoEdificioDetalleTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='grupoedificiodetalle_edit_']"));
            if (editButtons.Count > 0)
            {
                // Extract the ID from the edit button and navigate to details
                var editUrl = editButtons[0].GetAttribute("href");
                var idMatch = System.Text.RegularExpressions.Regex.Match(editUrl, @"Edit/(\d+)");
                if (idMatch.Success)
                {
                    var id = idMatch.Groups[1].Value;
                    _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle/Details/{id}");

                    wait.Until(d => d.FindElement(By.Id("grupoedificiodetalle_details_back")));
                    Assert.Contains("Detalles Grupo Edificio Detalle", _driver.PageSource);
                }
            }
        }

        [Fact]
        public void Test12_DeleteGrupoEdificioDetalle()
        {
            // First create a new record to delete
            _driver.Navigate().GoToUrl($"{_baseUrl}/GrupoEdificioDetalle/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("grupoedificiodetalle_create_form")));

            // Select Grupo
            var grupoSelect = new SelectElement(_driver.FindElement(By.Id("grupoedificiodetalle_create_idgrupo")));
            if (grupoSelect.Options.Count > 1)
            {
                grupoSelect.SelectByIndex(1);
            }

            // Select Edificio
            var edificioSelect = new SelectElement(_driver.FindElement(By.Id("grupoedificiodetalle_create_idedificio")));
            if (edificioSelect.Options.Count > 1)
            {
                edificioSelect.SelectByIndex(1);
            }

            var submitButton = _driver.FindElement(By.Id("grupoedificiodetalle_create_submit"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);
            submitButton.Click();

            wait.Until(d => d.Url.Contains("/GrupoEdificioDetalle") && !d.Url.Contains("/Create"));

            // Now find and delete the record
            Thread.Sleep(2000);

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='grupoedificiodetalle_delete_']"));
            if (deleteButtons.Count > 0)
            {
                var deleteBtn = deleteButtons[deleteButtons.Count - 1];
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", deleteBtn);
                Thread.Sleep(500);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", deleteBtn);

                wait.Until(d => d.FindElement(By.Id("grupoedificiodetalle_delete_form")));
                Assert.Contains("Eliminar Grupo Edificio Detalle", _driver.PageSource);

                var deleteSubmit = _driver.FindElement(By.Id("grupoedificiodetalle_delete_submit"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", deleteSubmit);
                Thread.Sleep(500);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", deleteSubmit);

                wait.Until(d => d.Url.Contains("/GrupoEdificioDetalle") && !d.Url.Contains("/Delete"));
                Assert.Contains("/GrupoEdificioDetalle", _driver.Url);
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
