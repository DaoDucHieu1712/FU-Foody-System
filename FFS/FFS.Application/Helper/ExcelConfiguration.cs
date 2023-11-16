using ClosedXML.Excel;
using FFS.Application.DTOs.Category;
using FFS.Application.DTOs.Report;
using FFS.Application.DTOs.Store;
using FFS.Application.DTOs.User;

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
            worksheet.Cell(currentRow, 1).Value = "MÃ MÓN ĂN";
            worksheet.Cell(currentRow, 2).Value = "TÊN MÓN";
            worksheet.Cell(currentRow, 3).Value = "MÔ TẢ";
            worksheet.Cell(currentRow, 4).Value = "GIÁ";
            worksheet.Cell(currentRow, 5).Value = "LOẠI MÓN ĂN";
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
            worksheet.Cell(currentRow, 1).Value = "MÃ KHO";
            worksheet.Cell(currentRow, 2).Value = "TÊN MÓN ĂN";
            worksheet.Cell(currentRow, 3).Value = "MÃ MÓN ĂN";
            worksheet.Cell(currentRow, 4).Value = "LOẠI MÓN ĂN";
            worksheet.Cell(currentRow, 5).Value = "SỐ LƯỢNG TRONG KHO";
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

        public static XLWorkbook ExportReport(List<ExportReportDTO> lstReports, XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Báo cáo");
            worksheet.Cell(1, 1).Value = "Bảng Thống Kê";
            worksheet.Cell(2, 1).Value = "Danh sách báo cáo";
            var currentRow = 3;
            worksheet.Cell(currentRow, 1).Value = "MÃ BÁO CÁO";
            worksheet.Cell(currentRow, 2).Value = "LOẠI BÁO CÁO";
            worksheet.Cell(currentRow, 3).Value = "NGƯỜI BÁO CÁO";
            worksheet.Cell(currentRow, 4).Value = "NGƯỜI NHẬN BÁO CÁO";
            worksheet.Cell(currentRow, 5).Value = "NỘI DUNG";
            worksheet.Cell(currentRow, 6).Value = "THỜI GIAN TẠO";
            foreach (var report in lstReports)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = report.Id;
                worksheet.Cell(currentRow, 2).Value = report.ReportType;
                worksheet.Cell(currentRow, 3).Value = report.FromEmail;
                worksheet.Cell(currentRow, 4).Value = report.TargetEmail;
                worksheet.Cell(currentRow, 5).Value = report.Desciption;
                worksheet.Cell(currentRow, 6).Value = report.ReportTime;

            }
            // Auto-fit columns after adding data
            worksheet.Columns().AdjustToContents();
            return workbook;
        }
        public static XLWorkbook ExportUser(List<UserExportDTO> lstUsers, XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Người dùng");
            worksheet.Cell(1, 1).Value = "Bảng Thống Kê";
            worksheet.Cell(2, 1).Value = "Danh sách người dùng";
            var currentRow = 3;
            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "USERNAME";
            worksheet.Cell(currentRow, 3).Value = "EMAIL";
            worksheet.Cell(currentRow, 4).Value = "VAI TRÒ";
            worksheet.Cell(currentRow, 5).Value = "TRẠNG THÁI";
            foreach (var user in lstUsers)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = user.Number;
                worksheet.Cell(currentRow, 2).Value = user.Username;
                worksheet.Cell(currentRow, 3).Value = user.Email;
                worksheet.Cell(currentRow, 4).Value = user.Role;
                worksheet.Cell(currentRow, 5).Value = user.Status;
            }
            // Auto-fit columns after adding data
            worksheet.Columns().AdjustToContents();
            return workbook;
        }

        public static XLWorkbook ExportCategory(List<CategoryDTO> lstCategory, XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Danh mục");
            worksheet.Cell(1, 1).Value = "Bảng Thống Kê";
            worksheet.Cell(2, 1).Value = "Danh sách danh mục";
            var currentRow = 3;
            worksheet.Cell(currentRow, 1).Value = "Mã danh mục";
            worksheet.Cell(currentRow, 2).Value = "Tên danh mục";
            worksheet.Cell(currentRow, 3).Value = "Ngày tạo";
            foreach (var cate in lstCategory)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = cate.Id;
                worksheet.Cell(currentRow, 2).Value = cate.CategoryName;
                worksheet.Cell(currentRow, 3).Value = cate.CreatedAt.ToString("MM/dd/yyyy");
            }
            // Auto-fit columns after adding data
            worksheet.Columns().AdjustToContents();
            return workbook;
        }
    }
}
