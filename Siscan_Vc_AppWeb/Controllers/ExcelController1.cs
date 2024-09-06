using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Siscan_Vc_DAL.DataContext;
using Siscan_Vc_AppWeb.Models.ViewModels;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class ExcelController1 : Controller
    {
        private readonly DbSiscanContext _context;

        public ExcelController1(DbSiscanContext context)
        {
            _context = context;
        }
        //metodo para crear el excel con los datos de la tabla
        [HttpPost]
        public IActionResult ExportarExcel(TyTConsultationViewModel model)
        {
            // Asegúra de que el modelo tenga datos
            if (model?.Aprendices == null || !model.Aprendices.Any())
            {
                return BadRequest("No hay datos para exportar.");
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Aprendices");

                // Encabezados
                worksheet.Cells[1, 1].Value = "Número de Documento";
                worksheet.Cells[1, 2].Value = "Ficha";
                worksheet.Cells[1, 3].Value = "Nombre";
                worksheet.Cells[1, 4].Value = "Apellido";
                worksheet.Cells[1, 5].Value = "Código Inscripción";
                worksheet.Cells[1, 6].Value = "Celular";
                worksheet.Cells[1, 7].Value = "Correo";
                worksheet.Cells[1, 8].Value = "Dirección";
                worksheet.Cells[1, 9].Value = "Estado TyT";
                worksheet.Cells[1, 10].Value = "Tipo de Documento";
                worksheet.Cells[1, 11].Value = "Ciudad";

                // Agrega los datos
                for (int i = 0; i < model.Aprendices.Count; i++)
                {
                    var aprendiz = model.Aprendices[i];
                    worksheet.Cells[i + 2, 1].Value = aprendiz.NumeroDocumentoAprendiz;
                    worksheet.Cells[i + 2, 2].Value = aprendiz.Ficha;
                    worksheet.Cells[i + 2, 3].Value = aprendiz.NombreAprendiz;
                    worksheet.Cells[i + 2, 4].Value = aprendiz.ApellidoAprendiz;
                    worksheet.Cells[i + 2, 5].Value = aprendiz.CodigoInscripcion;
                    worksheet.Cells[i + 2, 6].Value = aprendiz.CelAprendiz;
                    worksheet.Cells[i + 2, 7].Value = aprendiz.CorreoAprendiz;
                    worksheet.Cells[i + 2, 8].Value = aprendiz.DireccionAprendiz;
                    worksheet.Cells[i + 2, 9].Value = aprendiz.EstadoTytNombre;
                    worksheet.Cells[i + 2, 10].Value = aprendiz.TipoDocumentoNombre;
                    worksheet.Cells[i + 2, 11].Value = aprendiz.CiudadNombre;
                }

                // Configuracion el tipo de contenido y el nombre del archivo
                var stream = new MemoryStream();
                package.SaveAs(stream);
                var fileName = "Aprendices.xlsx";
                stream.Position = 0; // Asegúra de que la posición del stream esté al principio
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
