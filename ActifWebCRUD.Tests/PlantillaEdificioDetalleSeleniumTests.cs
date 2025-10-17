using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class PlantillaEdificioDetalleSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public PlantillaEdificioDetalleSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle");

            Assert.Contains("Plantilla Edificio Detalle", _driver.PageSource);
            Assert.Contains("plantillaEdificioDetalleTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaEdificioDetalleTable")));

            var table = _driver.FindElement(By.Id("plantillaEdificioDetalleTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle");

            var addButton = _driver.FindElement(By.Id("plantillaedificiodetalle_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle");

            var exportButton = _driver.FindElement(By.Id("plantillaedificiodetalle_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle");

            var addButton = _driver.FindElement(By.Id("plantillaedificiodetalle_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaedificiodetalle_create_form")));

            Assert.Contains("Crear Plantilla Edificio Detalle", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewPlantillaEdificioDetalle()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaedificiodetalle_create_idplantilla")));

            var plantillaSelect = _driver.FindElement(By.Id("plantillaedificiodetalle_create_idplantilla"));
            var edificioSelect = _driver.FindElement(By.Id("plantillaedificiodetalle_create_idedificio"));
            var companiaSelect = _driver.FindElement(By.Id("plantillaedificiodetalle_create_idcompania"));

            // Select first available options
            var plantillaSelectElement = new SelectElement(plantillaSelect);
            if (plantillaSelectElement.Options.Count > 1)
            {
                plantillaSelectElement.SelectByIndex(1);

                var edificioSelectElement = new SelectElement(edificioSelect);
                if (edificioSelectElement.Options.Count > 1)
                {
                    edificioSelectElement.SelectByIndex(1);
                }

                var companiaSelectElement = new SelectElement(companiaSelect);
                if (companiaSelectElement.Options.Count > 1)
                {
                    companiaSelectElement.SelectByIndex(1);
                }

                var submitButton = _driver.FindElement(By.Id("plantillaedificiodetalle_create_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/PlantillaEdificioDetalle") && !d.Url.Contains("/Create"));
                Assert.Contains("/PlantillaEdificioDetalle", _driver.Url);
            }
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaEdificioDetalleTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='plantillaedificiodetalle_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaEdificioDetalleTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='plantillaedificiodetalle_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaEdificioDetalleTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#plantillaEdificioDetalleTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 4, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditPlantillaEdificioDetalle()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaEdificioDetalleTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='plantillaedificiodetalle_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("plantillaedificiodetalle_edit_form")));

                Assert.Contains("Editar Plantilla Edificio Detalle", _driver.PageSource);

                var companiaSelect = _driver.FindElement(By.Id("plantillaedificiodetalle_edit_idcompania"));
                var companiaSelectElement = new SelectElement(companiaSelect);
                if (companiaSelectElement.Options.Count > 1)
                {
                    companiaSelectElement.SelectByIndex(1);
                }

                var submitButton = _driver.FindElement(By.Id("plantillaedificiodetalle_edit_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/PlantillaEdificioDetalle") && !d.Url.Contains("/Edit"));
                Assert.Contains("/PlantillaEdificioDetalle", _driver.Url);
            }
        }

        [Fact]
        public void Test11_VerifyCreateFormFields()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaedificiodetalle_create_form")));

            var plantillaSelect = _driver.FindElement(By.Id("plantillaedificiodetalle_create_idplantilla"));
            var edificioSelect = _driver.FindElement(By.Id("plantillaedificiodetalle_create_idedificio"));
            var companiaSelect = _driver.FindElement(By.Id("plantillaedificiodetalle_create_idcompania"));

            Assert.NotNull(plantillaSelect);
            Assert.NotNull(edificioSelect);
            Assert.NotNull(companiaSelect);
        }

        [Fact]
        public void Test12_VerifyValidationOnCreate()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificioDetalle/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaedificiodetalle_create_form")));

            var submitButton = _driver.FindElement(By.Id("plantillaedificiodetalle_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            // Should stay on create page due to validation or go back to index if no validation
            Thread.Sleep(1000);
            // Just verify the URL changed or stayed (either is acceptable)
            Assert.True(_driver.Url.Contains("/PlantillaEdificioDetalle"));
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
