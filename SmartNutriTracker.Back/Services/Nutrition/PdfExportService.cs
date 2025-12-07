using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartNutriTracker.Shared.DTOs.Nutrition;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SmartNutriTracker.Back.Services.Nutrition
{
    /// <summary>
    /// Servicio para exportar recomendaciones nutricionales a PDF usando iTextSharp.
    /// Genera un PDF con la recomendación de la IA, datos nutricionales y comidas sugeridas.
    /// </summary>
    public class PdfExportService : IPdfExportService
    {
        public async Task<byte[]> ExportRecomendacionesToPdfAsync(ResultadoRecomendacionNutricionalDTO recomendacion, NutritionResultDTO? calculo = null)
        {
            return await Task.Run(() => GeneratePdf(recomendacion, calculo));
        }

        private byte[] GeneratePdf(ResultadoRecomendacionNutricionalDTO recomendacion, NutritionResultDTO? calculo)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Crear documento PDF
                var document = new Document(PageSize.A4, 25, 25, 25, 25);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Colores
                var colorPrimary = new BaseColor(44, 82, 130);  // #2c5282
                var colorSecondary = new BaseColor(74, 123, 167); // #4a7ba7
                var colorLight = new BaseColor(230, 242, 255); // #e6f2ff

                // Título principal
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24, colorPrimary);
                var title = new Paragraph("📋 Recomendación Nutricional Personalizada", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 10;
                document.Add(title);

                // Línea separadora
                document.Add(new Paragraph(" "));

                // Fecha y tiempo
                var infoFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var generatedDate = new Paragraph($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", infoFont);
                generatedDate.SpacingBefore = 10;
                document.Add(generatedDate);

                var tiempoAI = new Paragraph($"Tiempo de procesamiento IA: {recomendacion.TiempoRespuestaMs} ms", infoFont);
                tiempoAI.SpacingAfter = 15;
                document.Add(tiempoAI);

                // Valores nutricionales calculados
                if (calculo != null)
                {
                    var subTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, colorSecondary);
                    var nutritionTitle = new Paragraph("📊 Valores Nutricionales Calculados", subTitleFont);
                    nutritionTitle.SpacingBefore = 10;
                    nutritionTitle.SpacingAfter = 10;
                    document.Add(nutritionTitle);

                    // Tabla de valores nutricionales
                    var nutritionTable = new PdfPTable(3) { WidthPercentage = 100 };
                    nutritionTable.SetWidths(new float[] { 33, 33, 34 });

                    AddNutritionCell(nutritionTable, "TMB", $"{calculo.TMB:F2} kcal");
                    AddNutritionCell(nutritionTable, "Calorías Mantenimiento", $"{calculo.CaloriasMantenimiento:F2} kcal");
                    AddNutritionCell(nutritionTable, "Calorías Objetivo", $"{calculo.CaloriasObjetivo:F2} kcal");

                    AddNutritionCell(nutritionTable, "Proteínas", $"{calculo.ProteinasGr:F2} g");
                    AddNutritionCell(nutritionTable, "Grasas", $"{calculo.GrasasGr:F2} g");
                    AddNutritionCell(nutritionTable, "Carbohidratos", $"{calculo.CarbohidratosGr:F2} g");

                    document.Add(nutritionTable);
                    document.Add(new Paragraph("\n"));
                }

                // Recomendación de texto
                var subTitleFont2 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, colorSecondary);
                var aiRecommendationTitle = new Paragraph("💡 Recomendación de la IA", subTitleFont2);
                aiRecommendationTitle.SpacingBefore = 10;
                aiRecommendationTitle.SpacingAfter = 10;
                document.Add(aiRecommendationTitle);

                var recommendationBox = new PdfPTable(1) { WidthPercentage = 100 };
                var cell = new PdfPCell(new Phrase(recomendacion.TextoRecomendacion ?? "No hay recomendación disponible"));
                cell.BackgroundColor = colorLight;
                cell.BorderColor = colorPrimary;
                cell.Padding = 15;
                recommendationBox.AddCell(cell);
                document.Add(recommendationBox);
                document.Add(new Paragraph("\n"));

                // Comidas sugeridas
                if (recomendacion.Comidas != null && recomendacion.Comidas.Any())
                {
                    var subTitleFont3 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, colorSecondary);
                    var mealsTitle = new Paragraph("🍽️ Comidas Sugeridas", subTitleFont3);
                    mealsTitle.SpacingBefore = 10;
                    mealsTitle.SpacingAfter = 10;
                    document.Add(mealsTitle);

                    var mealsTable = new PdfPTable(6) { WidthPercentage = 100 };
                    mealsTable.SetWidths(new float[] { 15, 12, 12, 12, 12, 37 });

                    // Header
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
                    AddMealHeader(mealsTable, "Nombre", headerFont);
                    AddMealHeader(mealsTable, "Calorías", headerFont);
                    AddMealHeader(mealsTable, "Proteínas (g)", headerFont);
                    AddMealHeader(mealsTable, "Grasas (g)", headerFont);
                    AddMealHeader(mealsTable, "Carbohidratos (g)", headerFont);
                    AddMealHeader(mealsTable, "Descripción", headerFont);

                    // Filas de comidas
                    var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                    bool alternate = false;

                    foreach (var comida in recomendacion.Comidas)
                    {
                        var rowColor = alternate ? new BaseColor(245, 245, 245) : BaseColor.WHITE;

                        AddMealCell(mealsTable, comida.Nombre, cellFont, rowColor);
                        AddMealCell(mealsTable, $"{comida.Calorias:F0}", cellFont, rowColor);
                        AddMealCell(mealsTable, $"{comida.Proteinas_g:F2}", cellFont, rowColor);
                        AddMealCell(mealsTable, $"{comida.Grasas_g:F2}", cellFont, rowColor);
                        AddMealCell(mealsTable, $"{comida.Carbohidratos_g:F2}", cellFont, rowColor);
                        AddMealCell(mealsTable, comida.Descripcion ?? "", cellFont, rowColor);

                        alternate = !alternate;
                    }

                    document.Add(mealsTable);
                    document.Add(new Paragraph("\n"));
                }

                // Footer
                var footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.GRAY);
                var footer = new Paragraph("SmartNutriTracker - Recomendaciones generadas por IA\nEsta es una recomendación basada en datos calculados. Consulta a un nutricionista certificado para personalización adicional.", footerFont);
                footer.Alignment = Element.ALIGN_CENTER;
                footer.SpacingBefore = 20;
                document.Add(footer);

                document.Close();
                return memoryStream.ToArray();
            }
        }

        private void AddNutritionCell(PdfPTable table, string label, string value)
        {
            var labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            var valueFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, new BaseColor(44, 82, 130));

            var cell = new PdfPCell();
            cell.BackgroundColor = new BaseColor(230, 242, 255);
            cell.Border = Rectangle.BOX;
            cell.BorderColor = new BaseColor(44, 82, 130);
            cell.Padding = 10;

            var phrase = new Phrase();
            phrase.Add(new Chunk(label + "\n", labelFont));
            phrase.Add(new Chunk(value, valueFont));
            cell.AddElement(new Paragraph(phrase));
            table.AddCell(cell);
        }

        private void AddMealHeader(PdfPTable table, string text, Font font)
        {
            var cell = new PdfPCell(new Phrase(text, font));
            cell.BackgroundColor = new BaseColor(44, 82, 130);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 8;
            table.AddCell(cell);
        }

        private void AddMealCell(PdfPTable table, string text, Font font, BaseColor backgroundColor)
        {
            var cell = new PdfPCell(new Phrase(text, font));
            cell.BackgroundColor = backgroundColor;
            cell.Padding = 6;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
        }
    }
}

