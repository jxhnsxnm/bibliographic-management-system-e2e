Sistema Web de Gestión Bibliográfica con Pruebas Automatizadas E2E

Plataforma web diseñada para la administración y visualización centralizada de catálogos de autores y libros, conectada a una base de datos relacional. El proyecto incorpora una suite de pruebas automatizadas de extremo a extremo (E2E) que ejecuta y valida de forma autónoma todos los flujos CRUD, generando evidencia visual automática de cada paso ejecutado.

## Características Principales
* **Gestión de Catálogos (CRUD):** Creación, consulta, actualización y eliminación de registros de autores y libros.
* **Persistencia Relacional:** Integración directa con SQL Server para asegurar la integridad referencial de los datos.
* **Automatización de Pruebas E2E:** Suite desarrollada en C# con Selenium WebDriver que simula la interacción real del usuario en el navegador.
* **Evidencia Visual Automática:** Captura y almacenamiento automático de capturas de pantalla durante la ejecución de cada caso de prueba para auditoría y depuración.

## Tecnologías Utilizadas
* **Backend / Frontend Web:** PHP, HTML5, CSS3
* **Base de Datos:** Microsoft SQL Server
* **Automatización QA:** C#, Selenium WebDriver

## Instalación y Uso
1. Clona este repositorio en el directorio de tu servidor local (ej. `htdocs` en XAMPP o `www` en Laragon).
2. Importa el script de la base de datos en tu instancia de **SQL Server** y actualiza las credenciales en el archivo de conexión PHP.
3. Para ejecutar las pruebas automatizadas, abre la solución de pruebas en **Visual Studio**, verifica que la URL base coincida con tu entorno local y ejecuta el *Test Explorer*.

## Autor
**Jose A. Johnson M.**  
