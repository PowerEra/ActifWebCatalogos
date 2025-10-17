# Documentación para Solicitud de CRUD - ActifWebCRUD

## Objetivo
Este documento contiene las instrucciones estándar para que los agentes creen CRUDs consistentes en el proyecto ActifWebCRUD.

## Contexto del Proyecto
- **Proyecto:** ActifWebCRUD
- **Ubicación:** /Users/enrique/ActifWebCRUD/ActifWebCRUD
- **Puerto:** 5073
- **Base de datos:** actif_web_cima_dev en dbdev.powerera.com
- **Usuario DB:** earaiza
- **Password DB:** VgfN-n4ju?H1Z4#JFRE

## Diseño Requerido (OBLIGATORIO)

### Vista Index (Grid)
- **Grid con DataTables** configurado con:
  - Paginación de 50 registros
  - Filtros en la parte superior de cada columna
  - Botón "Exportar a Excel" (icono Excel verde) - **DEBE generar archivo .xlsx real usando EPPlus**
  - Título de la pantalla arriba del grid
- **Botón Agregar:** Icono de hoja con signo + (🗎+)
- **Acciones por fila:**
  - Icono de lápiz para Editar (✏️)
  - Icono de basurero para Eliminar (🗑️)
  - **NO usar textos**, solo iconos

### Vista Create/Edit (Formulario)
- **Diseño responsivo** (Bootstrap 5)
- **Campos de fecha:** Usar selector de fechas (input type="date")
- **Campos con llave foránea:** Usar dropdowns (select)
  - Detectar FKs consultando vmetadata: `SELECT ForeignTable, CampoRelacion FROM vmetadata WHERE TableName = 'nombre_tabla' AND ForeignTable IS NOT NULL`
- **Auditoría automática (OBLIGATORIO):**
  - Si la tabla tiene columnas `IDUSER_ALTA` y `IDUSER_MODIF`, el sistema debe:
    - En Create: Establecer `IDUSER_ALTA = 1` (usuario simulado)
    - En Edit: Establecer `IDUSER_MODIF = 1` (usuario simulado)
    - **NO mostrar estos campos en los formularios** - se asignan automáticamente en el controlador
- **Labels en español**
- **Botones:**
  - Guardar (botón primario)
  - Cancelar (botón secundario)

### CSS Común
- **Archivo:** /Users/enrique/ActifWebCRUD/ActifWebCRUD/wwwroot/css/actif-crud.css
- **Debe incluir:**
  - Estilos para grids con filtros
  - Iconos de acciones consistentes
  - Botones estándar
  - Espaciado y márgenes uniformes

## Estructura de Base de Datos

### Tabla autoscreens
```sql
SELECT a.AutoScreenId, a.ObjectId, o.ObjectName, a.TableScreen, a.ViewScreen
FROM autoscreens a
INNER JOIN objects o ON a.ObjectId = o.IdObject
```

- **TableScreen:** Nombre de la tabla para INSERT/UPDATE/DELETE
- **ViewScreen:** Vista para SELECT (si es NULL, usar TableScreen)

### Detectar Llaves Foráneas
```sql
SELECT Column_Name, ForeignTable, CampoRelacion, CampoBusqueda
FROM vmetadata
WHERE TableName = 'nombre_tabla'
AND ForeignTable IS NOT NULL
```

## Instrucciones para el Agente

### OBLIGATORIO ANTES DE EMPEZAR
1. Leer `/Users/enrique/readme.md` para conocer credenciales y reglas
2. Leer este documento `/Users/enrique/ActifWebCRUD/SolicitudCRUD.md`
3. **NO usar sqlcmd** - usar PESqlConnect solamente

### Pasos a Seguir

#### 1. Investigación de la Tabla
```bash
# Obtener estructura de la tabla
PESqlConnect dbdev.powerera.com earaiza "VgfN-n4ju?H1Z4#JFRE" actif_web_cima_dev "SELECT TOP 10 * FROM [nombre_tabla]"

# Obtener llaves foráneas
PESqlConnect dbdev.powerera.com earaiza "VgfN-n4ju?H1Z4#JFRE" actif_web_cima_dev "SELECT Column_Name, ForeignTable, CampoRelacion FROM vmetadata WHERE TableName = '[nombre_tabla]' AND ForeignTable IS NOT NULL"
```

#### 2. Crear Modelo C#
- Ubicación: `/Users/enrique/ActifWebCRUD/ActifWebCRUD/Models/[NombreTabla].cs`
- Usar tipos de datos correctos (short para SMALLINT, int para INT, etc.)
- Agregar `[Key]` para llaves primarias
- Agregar `[Table("nombre_tabla")]` si el nombre difiere
- **Si existen columnas de auditoría:**
  - `IDUSER_ALTA`: Usuario que creó el registro (tipo: int, nullable)
  - `IDUSER_MODIF`: Usuario que modificó el registro (tipo: int, nullable)
  - Incluir en el modelo pero NO en los formularios ni en [Bind]

#### 3. Actualizar DbContext
- Archivo: `/Users/enrique/ActifWebCRUD/ActifWebCRUD/Data/ApplicationDbContext.cs`
- Agregar `public DbSet<NombreTabla> NombreTabla { get; set; }`

#### 4. Crear Controller
- Ubicación: `/Users/enrique/ActifWebCRUD/ActifWebCRUD/Controllers/[NombreTabla]Controller.cs`
- Implementar: Index, Create, Edit, Delete, Details
- Usar la vista configurada en `autoscreens.ViewScreen` para Index
- **Auditoría automática:**
  - En `Create [HttpPost]`: Si existe `IDUSER_ALTA`, establecer `objeto.IdUserAlta = 1`
  - En `Edit [HttpPost]`: Si existe `IDUSER_MODIF`, establecer `objeto.IdUserModif = 1`
- **Exportar a Excel:**
  - Método `ExportToExcel()` debe generar archivo .xlsx usando **EPPlus**
  - Instalar paquete: `dotnet add package EPPlus`
  - Retornar: `File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "[Tabla]_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx")`

#### 5. Crear Vistas
Todas en `/Users/enrique/ActifWebCRUD/ActifWebCRUD/Views/[NombreTabla]/`

**Index.cshtml:**
```html
- DataTables con paginación de 50
- Filtros en columnas
- Botón exportar Excel
- Iconos para acciones (no texto)
```

**Create.cshtml y Edit.cshtml:**
```html
- Formulario responsivo
- Selectores de fecha para DateTime
- Dropdowns para FKs
- Validación del lado cliente
```

**Delete.cshtml:**
```html
- Confirmación con datos del registro
- Botones Eliminar/Cancelar
```

#### 6. Pruebas Selenium
- Crear proyecto de pruebas si no existe
- Ubicación: `/Users/enrique/ActifWebCRUD/ActifWebCRUD.Tests/`
- Archivo: `[NombreTabla]SeleniumTests.cs`
- Tests mínimos:
  - Navegación a Index
  - Crear registro
  - Editar registro
  - Eliminar registro
  - Verificar DataTables funciona

#### 7. Verificación Obligatoria
```bash
cd /Users/enrique/ActifWebCRUD/ActifWebCRUD
dotnet build
dotnet run --urls "http://localhost:5073"
```

Verificar:
- Compilación sin errores
- Aplicación inicia correctamente
- URL funciona: http://localhost:5073/[NombreTabla]
- Grid carga datos
- CRUD completo funciona

#### 8. Ejecutar Pruebas
```bash
cd /Users/enrique/ActifWebCRUD/ActifWebCRUD.Tests
dotnet test
```

## Formato de Reporte

Al finalizar, el agente debe reportar:

```markdown
## REPORTE - CRUD [NombreTabla]

### Estado de Compilación
✅/❌ Exitoso/Fallido

### Estado de Ejecución
✅/❌ Funcional/Error

### URL de Prueba
http://localhost:5073/[NombreTabla] - HTTP [código]

### Estructura de la Tabla
- Campo1: tipo (PK/FK)
- Campo2: tipo
...

### Llaves Foráneas Detectadas
- Campo -> TablaReferenciada.CampoRelacionado

### Resultado de Pruebas Selenium
X/Y pruebas pasaron

### Problemas Encontrados y Soluciones
- Problema 1: Solución aplicada
...

### Confirmación
✅/❌ CRUD 100% funcional
```

## Notas Importantes

- **NUNCA modificar código de otros CRUDs**
- **SIEMPRE usar el CSS común** definido en actif-crud.css
- **SIEMPRE usar PESqlConnect**, nunca sqlcmd
- Los IDs de elementos HTML deben seguir: `[tabla]_[accion]_[campo]`
- Paginación siempre 50 registros
- **Exportar Excel debe generar archivo .xlsx real (NO CSV)** - usar EPPlus
- Iconos en lugar de texto para acciones
- **Auditoría automática:** Si existen IDUSER_ALTA/IDUSER_MODIF, asignar automáticamente valor 1

## Actualizaciones de este Documento

Este documento se actualiza después de cada CRUD para mejorar las instrucciones basándose en la experiencia.

**Actualizaciones:**
- **2025-10-13:** Versión inicial
- **2025-10-14:**
  - Agregado: Exportar a Excel debe ser .xlsx real usando EPPlus (NO CSV)
  - Agregado: Auditoría automática para IDUSER_ALTA y IDUSER_MODIF (simular usuario ID=1)
  - Total de CRUDs completados hasta ahora: 8/37 (80 tests pasados)
