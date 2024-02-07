using ADIRA.Shared.BusinessDataObjects;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace ADIRA.Client.Pages
{
    public partial class ManageEmployees
    {
        private bool isLoading;
        private bool isSuccess;

        private string ErrorMessage { get; set; }
        private string SuccessMessage { get; set; }
        private List<Employee> EmployeesData { get; set; }
        private async Task HandleFileChange(InputFileChangeEventArgs e)
        {
            isLoading = true;
            isSuccess=false;
            ClearInfoLabels();
            var file = e.File;

            if (file != null && file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                using (MemoryStream memoryStream = new())
                {
                    await file.OpenReadStream().CopyToAsync(memoryStream);

                    using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(memoryStream, false))
                    {
                        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                        Worksheet worksheet = worksheetPart.Worksheet;
                        SheetData sheetData = worksheet.GetFirstChild<SheetData>();

                        EmployeesData = new();
                        foreach (Row row in sheetData.Elements<Row>())
                        {
                            var rowCount = row.RowIndex;

                            if (rowCount == 1) continue;

                            var rowData = new Employee();

                            int columnCounter = 1;
                            foreach (Cell cell in row.Elements<Cell>())
                            {
                                string cellValue = GetCellValue(cell, workbookPart);

                                switch (columnCounter)
                                {
                                    case 1:
                                        rowData.ID = cellValue;
                                        break;
                                    case 2:
                                        rowData.Entity = cellValue;
                                        break;
                                    case 3:
                                        rowData.Name = cellValue;
                                        break;
                                    case 4:
                                        rowData.Email = cellValue;
                                        break;
                                    case 5:
                                        rowData.Department = cellValue;
                                        break;
                                }

                                columnCounter++;
                            }

                            EmployeesData.Add(rowData);
                        }
                    }
                }

                await SaveDataToServer(EmployeesData);
                isLoading = false;
                await GetEmployeeDataFromServer();
            }
            else
            {
                ErrorMessage = "Error in File Upload/Unknown File Format/Empty File";
            }
        }

        private string GetCellValue(Cell cell, WorkbookPart workbookPart)
        {
            SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;
            string cellValue = cell.InnerText;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[int.Parse(cellValue)].InnerText;
            }
            else
            {
                return cellValue;
            }
        }

        private async Task SaveDataToServer(IEnumerable<Employee> employeesData)
        {

            var response = await httpClient.PostAsJsonAsync("/api/FileUploadDB/employee/save", employeesData);
            if (!response.IsSuccessStatusCode)
            {
                isSuccess= false;
                ErrorMessage = "Error Occured, Reason: " + await response.Content.ReadAsStringAsync();
            }
            isSuccess = true;
        }

        protected override async Task OnInitializedAsync()
        {
            await GetEmployeeDataFromServer();
            StateHasChanged();

        }
        private async Task HandleDelete(bool result)
        {
            if (result)
            {
               await GetEmployeeDataFromServer();
                StateHasChanged();
                
            }

        }

        private async Task GetEmployeeDataFromServer()
        {
            ClearInfoLabels();
            var response = await httpClient.GetAsync("/api/FileUploadDB/employee/read");
            // SecretSantaEmployeesData = null;

            if (response.IsSuccessStatusCode)
            {
                EmployeesData = JsonConvert.DeserializeObject<List<Employee>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                ErrorMessage = "Error Occured, Reason: No data to display";
            }
        }
        private void ClearInfoLabels()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }
        private bool isPopupVisible = false;

        void ShowPopup()
        {
            isPopupVisible = true;
            StateHasChanged();
        }

        void ClosePopup()
        {
            isPopupVisible = false;
            isSuccess = false;
            StateHasChanged();
        }


    }
}
