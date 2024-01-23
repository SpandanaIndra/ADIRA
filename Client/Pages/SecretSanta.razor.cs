using ADIRA.Shared.BusinessDataObjects;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ADIRA.Client.Pages
{
    public partial class SecretSanta
    {
        private List<Employee> EmployeesData { get; set; }
        private List<SecretSantaData> SecretSantaEmployeesData { get; set; }
        private string ErrorMessage { get; set; }
        private string SuccessMessage { get; set; }
        private bool SendEmailButtonDisabled { get; set; } = false;
        private string SantaDataPassword { get; set; }
        private bool ShowSantaPasswordInput { get; set; } = false;

       
        private async Task RunSecretSantaAllocation()
        {
            ClearInfoLabels();
            var response = await httpClient.PostAsJsonAsync("/api/SecretSanta/allocate", string.Empty);
            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Secret Santa Allocations Have Been Successfully Done";
            }
            else
            {
                ErrorMessage = "Error Occured, Reason: " + await response.Content.ReadAsStringAsync();
            }
        }

        private async Task GetPasswordFieldForSecretSantaEmployeeData()
        {
            ClearInfoLabels();
            ShowSantaPasswordInput = true;
            EmployeesData = null;
            SecretSantaEmployeesData = null;
        }

        private async Task GetSecretSantaEmployeeDataFromServer()
        {
            ClearInfoLabels();

            if (string.IsNullOrWhiteSpace(SantaDataPassword))
            {
                ErrorMessage = "Password Empty";
            }
            else
            {
                var response = await httpClient.GetAsync($"/api/file/SecretSanta/read/{SantaDataPassword}");
                EmployeesData = null;
                ShowSantaPasswordInput = false;
                SantaDataPassword = null;

                if (response.IsSuccessStatusCode)
                {
                    SecretSantaEmployeesData = JsonConvert.DeserializeObject<List<SecretSantaData>>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    ErrorMessage = $"Error Occured, Reason: {await response.Content.ReadAsStringAsync()}";
                }
            }
        }

        private async Task SendEmailsToEmployees()
        {
            SendEmailButtonDisabled = true;
            ClearInfoLabels();
            SuccessMessage = "Processing, please wait...";

            var response = await httpClient.PostAsJsonAsync("/api/SecretSanta/sendmails", string.Empty);
            if (response.IsSuccessStatusCode)
            {
                SendEmailButtonDisabled = false;
                SuccessMessage = "25 Secret Santa mails have been successfully sent, please click Send Emails button again for the next batch";
            }
            else
            {
                SendEmailButtonDisabled = false;
                SuccessMessage = null;
                ErrorMessage = "Error Occured, Reason: " + await response.Content.ReadAsStringAsync();
            }
        }

        private void ClearInfoLabels()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }
    }
}
