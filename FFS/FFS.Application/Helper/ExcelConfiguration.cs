using ClosedXML.Excel;
using FFS.Application.DTOs.Store;

namespace FFS.Application.Helper
{
    public class ExcelConfiguration
    {
        public static XLWorkbook ExportFood(List<ExportFoodDTO> listFoods, XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Món ăn");
            worksheet.Cell(1, 1).Value = "Bảng Thống Kê";
            worksheet.Cell(2, 1).Value = "Danh sách món ăn";
            var currentRow = 3;
            worksheet.Cell(currentRow, 1).Value = "Mã món ăn";
            worksheet.Cell(currentRow, 2).Value = "Tên món";
            worksheet.Cell(currentRow, 3).Value = "Mô tả";
            worksheet.Cell(currentRow, 4).Value = "Giá";
            worksheet.Cell(currentRow, 5).Value = "Loại món ăn";
            foreach (var Food in listFoods)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = Food.Id;
                worksheet.Cell(currentRow, 2).Value = Food.FoodName;
                worksheet.Cell(currentRow, 3).Value = Food.Description;
                worksheet.Cell(currentRow, 4).Value = Food.Price;
                worksheet.Cell(currentRow, 5).Value = Food.CategoryName;

            }
            // Auto-fit columns after adding data
            worksheet.Columns().AdjustToContents();
            return workbook;
        }
        public static XLWorkbook ExportInventory(List<ExportInventoryDTO> listInventories, XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Kho");
            worksheet.Cell(1, 1).Value = "Bảng Thống Kê";
            worksheet.Cell(2, 1).Value = "Danh sách món ăn trong kho";
            var currentRow = 3;
            worksheet.Cell(currentRow, 1).Value = "Mã kho";
            worksheet.Cell(currentRow, 2).Value = "Tên món ăn";
            worksheet.Cell(currentRow, 3).Value = "Mã món ăn";
            worksheet.Cell(currentRow, 4).Value = "Loại món ăn";
            worksheet.Cell(currentRow, 5).Value = "Số lượng trong kho";
            foreach (var inventory in listInventories)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = inventory.Id;
                worksheet.Cell(currentRow, 2).Value = inventory.FoodName;
                worksheet.Cell(currentRow, 3).Value = inventory.FoodId;
                worksheet.Cell(currentRow, 4).Value = inventory.CategoryName;
                worksheet.Cell(currentRow, 5).Value = inventory.quantity;

            }
            // Auto-fit columns after adding data
            worksheet.Columns().AdjustToContents();
            return workbook;
        }
    }
}
