using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ActifWebCRUD.Tests
{
    public class INPC2SeleniumTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5073";

        public INPC2SeleniumTests()
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
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2");

            Assert.Contains("INPC2", _driver.PageSource);
            Assert.Contains("inpc2Table", _driver.PageSource);
        }

        [Fact]
        public void Test02_VerifyDataTablesIsLoaded()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("inpc2Table")));

            var table = _driver.FindElement(By.Id("inpc2Table"));
            Assert.NotNull(table);
        }

        [Fact]
        public void Test03_VerifyAddButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2");

            var addButton = _driver.FindElement(By.Id("inpc2_add_button"));
            Assert.NotNull(addButton);
            Assert.True(addButton.Displayed);
        }

        [Fact]
        public void Test04_VerifyExportButtonExists()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2");

            var exportButton = _driver.FindElement(By.Id("inpc2_export_button"));
            Assert.NotNull(exportButton);
            Assert.True(exportButton.Displayed);
        }

        [Fact]
        public void Test05_NavigateToCreatePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2");

            var addButton = _driver.FindElement(By.Id("inpc2_add_button"));
            addButton.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("inpc2_create_form")));

            Assert.Contains("Crear Nuevo Registro INPC2", _driver.PageSource);
        }

        [Fact]
        public void Test06_CreateNewINPC2()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("inpc2_create_anio")));

            var anioField = _driver.FindElement(By.Id("inpc2_create_anio"));
            var mesField = _driver.FindElement(By.Id("inpc2_create_mes"));
            var grupoSimulacionField = _driver.FindElement(By.Id("inpc2_create_idgruposimulacion"));
            var paisField = _driver.FindElement(By.Id("inpc2_create_idpais"));
            var indiceField = _driver.FindElement(By.Id("inpc2_create_indice"));

            int testYear = DateTime.Now.Year;
            int testMonth = DateTime.Now.Month;

            anioField.SendKeys(testYear.ToString());
            mesField.SendKeys(testMonth.ToString());

            // Select first available grupo simulacion
            var selectGrupo = new SelectElement(grupoSimulacionField);
            if (selectGrupo.Options.Count > 1)
            {
                selectGrupo.SelectByIndex(1);
            }

            // Select first available pais
            var selectPais = new SelectElement(paisField);
            if (selectPais.Options.Count > 1)
            {
                selectPais.SelectByIndex(1);
            }

            indiceField.SendKeys("1.234567");

            var submitButton = _driver.FindElement(By.Id("inpc2_create_submit"));

            // Scroll to the submit button
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
            Thread.Sleep(500);

            submitButton.Click();

            wait.Until(d => d.Url.Contains("/INPC2"));
            Assert.Contains("/INPC2", _driver.Url);
        }

        [Fact]
        public void Test07_VerifyEditButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("inpc2Table")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='inpc2_edit_']"));
            Assert.True(editButtons.Count > 0, "Should have at least one edit button");
        }

        [Fact]
        public void Test08_VerifyDeleteButtonsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("inpc2Table")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var deleteButtons = _driver.FindElements(By.CssSelector("a[id^='inpc2_delete_']"));
            Assert.True(deleteButtons.Count > 0, "Should have at least one delete button");
        }

        [Fact]
        public void Test09_VerifyFilterInputsExist()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("inpc2Table")));

            var filterInputs = _driver.FindElements(By.CssSelector("#inpc2Table thead input.form-control-sm"));
            Assert.True(filterInputs.Count >= 6, "Should have filter inputs for main columns");
        }

        [Fact]
        public void Test10_EditINPC2()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("inpc2Table")));

            Thread.Sleep(2000); // Wait for DataTables to initialize

            var editButtons = _driver.FindElements(By.CssSelector("a[id^='inpc2_edit_']"));
            if (editButtons.Count > 0)
            {
                editButtons[0].Click();

                wait.Until(d => d.FindElement(By.Id("inpc2_edit_form")));

                Assert.Contains("Editar Registro INPC2", _driver.PageSource);

                var indiceField = _driver.FindElement(By.Id("inpc2_edit_indice"));
                indiceField.Clear();
                indiceField.SendKeys("2.345678");

                var submitButton = _driver.FindElement(By.Id("inpc2_edit_submit"));

                // Scroll to the submit button
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);
                Thread.Sleep(500);

                submitButton.Click();

                wait.Until(d => d.Url.Contains("/INPC2") && !d.Url.Contains("/Edit"));
                Assert.Contains("/INPC2", _driver.Url);
            }
        }

        [Fact]
        public void Test11_VerifyForeignKeyDropdownsWork()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2/Create");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("inpc2_create_idgruposimulacion")));

            var grupoSimulacionField = _driver.FindElement(By.Id("inpc2_create_idgruposimulacion"));
            var paisField = _driver.FindElement(By.Id("inpc2_create_idpais"));

            var selectGrupo = new SelectElement(grupoSimulacionField);
            var selectPais = new SelectElement(paisField);

            Assert.True(selectGrupo.Options.Count > 0, "Grupo Simulacion dropdown should have options");
            Assert.True(selectPais.Options.Count > 0, "Pais dropdown should have options");
        }

        [Fact]
        public void Test12_VerifyExportToExcelWorks()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/INPC2");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("inpc2_export_button")));

            var exportButton = _driver.FindElement(By.Id("inpc2_export_button"));

            // Just verify the button is clickable and doesn't throw error
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
