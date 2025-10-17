using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class PlantillaEdificioSeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public PlantillaEdificioSeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio");

            Assert.Contains("Plantilla Edificio", _driver.PageSource);
            Assert.Contains("plantillaEdificioTable", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaEdificioTable")));

            var table = _driver.FindElement(By.Id("plantillaEdificioTable"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio");

            var addButton = _driver.FindElement(By.Id("plantillaedificio_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio");

            var exportButton = _driver.FindElement(By.Id("plantillaedificio_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio");

            var addButton = _driver.FindElement(By.Id("plantillaedificio_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaedificio_create_form")));

            Assert.Contains("Crear Nueva Plantilla Edificio", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewPlantillaEdificio()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaedificio_create_plantilla")));

            var plantillaField = _driver.FindElement(By.Id("plantillaedificio_create_plantilla"));
            var tipoDepSelect = _driver.FindElement(By.Id("plantillaedificio_create_idtipodep"));
            var tomaEdificioSelect = _driver.FindElement(By.Id("plantillaedificio_create_tomaedificio"));
            var tomaCCSelect = _driver.FindElement(By.Id("plantillaedificio_create_tomacc"));
            var companiaSelect = _driver.FindElement(By.Id("plantillaedificio_create_idcompania"));

            plantillaField.SendKeys("Plantilla Test " + DateTime.Now.Ticks);

            // Select tipo depreciacion
            var tipoDepSelectElement = new SelectElement(tipoDepSelect);
            if (tipoDepSelectElement.Options.Count > 1)
            {
                tipoDepSelectElement.SelectByIndex(1);
            }

            // Select Toma Edificio
            var tomaEdificioSelectElement = new SelectElement(tomaEdificioSelect);
            tomaEdificioSelectElement.SelectByValue("true");

            // Select Toma CC
            var tomaCCSelectElement = new SelectElement(tomaCCSelect);
            tomaCCSelectElement.SelectByValue("false");

            // Select compania
            var companiaSelectElement = new SelectElement(companiaSelect);
            if (companiaSelectElement.Options.Count > 1)
            {
                companiaSelectElement.SelectByIndex(1);
            }

            var submitButton = _driver.FindElement(By.Id("plantillaedificio_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/PlantillaEdificio") && !d.Url.Contains("/Create"));
            Assert.Contains("/PlantillaEdificio", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='plantillaedificio_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='plantillaedificio_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaEdificioTable")));

            var filterInputs = _driver.FindElements(By.CssSelector("#plantillaEdificioTable thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 5, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditPlantillaEdificio()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaEdificioTable")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='plantillaedificio_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("plantillaedificio_edit_form")));

                Assert.Contains("Editar Plantilla Edificio", _driver.PageSource);

                var plantillaField = _driver.FindElement(By.Id("plantillaedificio_edit_plantilla"));
                plantillaField.Clear();
                plantillaField.SendKeys("Plantilla Updated " + DateTime.Now.Ticks);

                var submitButton = _driver.FindElement(By.Id("plantillaedificio_edit_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/PlantillaEdificio") && !d.Url.Contains("/Edit"));
                Assert.Contains("/PlantillaEdificio", _driver.Url);
            }
        }

        [Fact]
        public void Test11_VerifyCreateFormFields()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaedificio_create_form")));

            var plantillaField = _driver.FindElement(By.Id("plantillaedificio_create_plantilla"));
            var tipoDepSelect = _driver.FindElement(By.Id("plantillaedificio_create_idtipodep"));
            var tomaEdificioSelect = _driver.FindElement(By.Id("plantillaedificio_create_tomaedificio"));
            var tomaCCSelect = _driver.FindElement(By.Id("plantillaedificio_create_tomacc"));
            var companiaSelect = _driver.FindElement(By.Id("plantillaedificio_create_idcompania"));

            Assert.NotNull(plantillaField);
            Assert.NotNull(tipoDepSelect);
            Assert.NotNull(tomaEdificioSelect);
            Assert.NotNull(tomaCCSelect);
            Assert.NotNull(companiaSelect);
        }

        [Fact]
        public void Test12_VerifyExcelExport()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/PlantillaEdificio");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("plantillaedificio_export_button")));

            var exportButton = _driver.FindElement(By.Id("plantillaedificio_export_button"));

            // Just verify the button is clickable
            Assert.True(exportButton.Enabled);
            Assert.True(exportButton.Displayed);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
