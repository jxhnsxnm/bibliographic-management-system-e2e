using System;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest
{
    class Programa
    {
        static IWebDriver driver;
        static StringBuilder reporte = new StringBuilder();

        static void Main()
        {
            driver = new ChromeDriver();
            reporte.Append("<html><head><title>Reporte de Pruebas</title></head><body>");
            reporte.Append("<h1>Resultado de Pruebas</h1>");

            EjecutarPrueba("1. Agregar autor válido", ProbarAgregarAutorValido);
            EjecutarPrueba("2. Ver listado de autores", ProbarListadoAutores);
            EjecutarPrueba("3. Editar autor existente", ProbarEditarAutor);
            EjecutarPrueba("4. Eliminar autor", ProbarEliminarAutor);
            EjecutarPrueba("5. Acceder formulario de contacto", ProbarFormularioContacto);
                
            reporte.Append("</body></html>");
            File.WriteAllText("ReportePruebas.html", reporte.ToString());

            driver.Quit();
            Console.WriteLine("✔ Pruebas finalizadas. Revisa 'ReportePruebas.html' y la carpeta 'Capturas'.");
        }

        static void EjecutarPrueba(string nombrePrueba, Action prueba)
        {
            try
            {
                prueba();
                reporte.Append($"<p style='color:green;'>{nombrePrueba}: ✅ Éxito</p>");
                TomarCaptura(nombrePrueba.Replace(" ", "_") + "_OK");
            }
            catch (Exception ex)
            {
                reporte.Append($"<p style='color:red;'>{nombrePrueba}: ❌ Error - {ex.Message}</p>");
                TomarCaptura(nombrePrueba.Replace(" ", "_") + "_Error");
            }
        }

        static void TomarCaptura(string nombreArchivo)
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string rutaCapturas = Path.Combine(Directory.GetCurrentDirectory(), "Capturas");
                Directory.CreateDirectory(rutaCapturas);
                string rutaArchivo = Path.Combine(rutaCapturas, nombreArchivo + ".png");
                screenshot.SaveAsFile(rutaArchivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error tomando captura: " + ex.Message);
            }
        }

        // 🟢 Historia 1: Agregar autor válido
        static void ProbarAgregarAutorValido()
        {
            driver.Navigate().GoToUrl("http://web-library.free.nf/functions/autores/agregar.php");
            driver.Manage().Window.Maximize();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("id_autor")));

            driver.FindElement(By.Name("id_autor")).SendKeys("4241819");
            driver.FindElement(By.Name("apellido")).SendKeys("TestApellido");
            driver.FindElement(By.Name("nombre")).SendKeys("TestNombre");
            driver.FindElement(By.Name("telefono")).SendKeys("8091234567");
            driver.FindElement(By.Name("direccion")).SendKeys("Calle Test");
            driver.FindElement(By.Name("ciudad")).SendKeys("TestCity");
            driver.FindElement(By.Name("estado")).SendKeys("TestState");
            driver.FindElement(By.Name("pais")).SendKeys("TestLand");
            driver.FindElement(By.Name("cod_postal")).SendKeys("99999");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Espera la redirección a la lista de autores
            wait.Until(ExpectedConditions.UrlContains("autores.php"));
            if (!driver.Url.Contains("autores.php"))
                throw new Exception("No se redirigió a la lista de autores después del registro.");
        }

        // 🟢 Historia 2: Ver listado de autores
        static void ProbarListadoAutores()
        {
            driver.Navigate().GoToUrl("http://web-library.free.nf/functions/autores/autores.php");
            driver.Manage().Window.Maximize();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.PageSource.Contains("Autores") || driver.FindElements(By.TagName("table")).Count > 0);

            bool contieneTabla = driver.PageSource.Contains("Autores") || driver.FindElements(By.TagName("table")).Count > 0;
            if (!contieneTabla)
                throw new Exception("No se muestra la tabla de autores.");
        }

        // 🟢 Historia 3: Editar un autor existente
        static void ProbarEditarAutor()
        {
            driver.Navigate().GoToUrl("http://web-library.free.nf/functions/autores/editar.php?id_autor=4241819");
            driver.Manage().Window.Maximize();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var nombreInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("nombre")));
            nombreInput.Clear();
            nombreInput.SendKeys("NombreEditado");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Manejo del Alert que muestra la confirmación de actualización
            wait.Until(ExpectedConditions.AlertIsPresent());
            var alert = driver.SwitchTo().Alert();
            alert.Accept();

            // Espera redirección a la lista de autores
            wait.Until(ExpectedConditions.UrlContains("autores.php"));
            if (!driver.Url.Contains("autores.php"))
                throw new Exception("No se redirigió a la lista de autores después de editar.");
        }

        // 🟢 Historia 4: Eliminar autor
        static void ProbarEliminarAutor()
        {
            driver.Navigate().GoToUrl("http://web-library.free.nf/functions/autores/eliminar.php?id_autor=4241819");
            driver.Manage().Window.Maximize();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var botonEliminar = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit']")));
            botonEliminar.Click();

            // Espera a que el autor ya no aparezca en el contenido de la página
            wait.Until(driver => !driver.PageSource.Contains("4241819"));
            if (driver.PageSource.Contains("4241819"))
                throw new Exception("El autor aún aparece en la lista después de intentar eliminar.");
        }

        // 🟢 Historia 5: Acceder formulario de contacto
        static void ProbarFormularioContacto()
        {
            driver.Navigate().GoToUrl("http://web-library.free.nf/functions/contacto/contacto.php");
            driver.Manage().Window.Maximize();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("nombre")));

            driver.FindElement(By.Name("nombre")).SendKeys("TestUser");
            driver.FindElement(By.Name("correo")).SendKeys("test@email.com");
            driver.FindElement(By.Name("asunto")).SendKeys("Prueba de contacto");
            driver.FindElement(By.Name("comentario")).SendKeys("Este es un mensaje de prueba.");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Manejo de confirmación vía alert o redirección, según el comportamiento de procesar_contacto.php.
            // Suponiendo que se lanza un alert de confirmación:
            wait.Until(ExpectedConditions.AlertIsPresent());
            var alert = driver.SwitchTo().Alert();
            string mensajeAlert = alert.Text;
            alert.Accept();

            if (!mensajeAlert.Contains("Mensaje enviado correctamente"))
            {
                throw new Exception("El mensaje del alert no coincide con la confirmación esperada.");
            }
        }
    }
}
