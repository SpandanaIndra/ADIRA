using ADIRA.Client.Authentication;
using ADIRA.Shared.BusinessDataObjects;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace ADIRA.Client.Pages
{
    public partial class SecretSanta
    {
        private string selectedEntity;
        private string selectedLocation;
        private bool buttonClicked;

        private List<Entity> entities;
        private List<EmployeeLocation> locations = new List<EmployeeLocation>
        {
           
            new EmployeeLocation { Name="Hyderabad"},
            new EmployeeLocation { Name="Banglore"}
        };

        


        private bool isLoading;
        private bool isSuccess;
        private List<Employee> EmployeesData { get; set; }
        private List<SecretSantaData> SecretSantaEmployeesData { get; set; }
        private string ErrorMessage { get; set; }
        private string SuccessMessage { get; set; }
        private bool SendEmailButtonDisabled { get; set; } = false;
        private string SantaDataPassword { get; set; }
        private bool ShowSantaPasswordInput { get; set; } = false;

        int entId;


        private async Task RunSecretSantaAllocation()
        {
           
            buttonClicked = true;
            if (!string.IsNullOrEmpty(selectedLocation)&&!string.IsNullOrEmpty(selectedEntity))
            {
                ClearInfoLabels();
                if (selectedEntity != "")
                {
                     entId = Convert.ToInt32(selectedEntity);

                }
               
                string loc = selectedLocation;
                ClearInfoLabels();
                // Obtain the JWT token from your authentication state
                var customStateProvider = (CustomAuthenticationStateProvider)_authStateProvider;

                var jwtToken = await customStateProvider.GetToken();

                if (jwtToken != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(jwtToken);
                   
                    var apiUrl = $"/api/SecretSanta/allocate/{entId}/{loc}";

                    // Make the API call
                    var response = await httpClient.PostAsJsonAsync(apiUrl, string.Empty);


                    if (response.IsSuccessStatusCode)
                    {
                        SuccessMessage = "Secret Santa Allocations Have Been Successfully Done";
                    }
                    else
                    {
                        ErrorMessage = "Error Occured, Reason: " + await response.Content.ReadAsStringAsync();
                    }
                }
                buttonClicked = false;
            }
           
        }

       /* private async Task GetPasswordFieldForSecretSantaEmployeeData()
        {
            ClearInfoLabels();
            ShowSantaPasswordInput = true;
            EmployeesData = null;
            SecretSantaEmployeesData = null;
        }*/

        private async Task GetSecretSantaEmployeeDataFromServer()
        {
            ClearInfoLabels();

            if (string.IsNullOrWhiteSpace(SantaDataPassword))
            {
                ErrorMessage = "Password Empty";
            }
            else
            {
                var secretSantaEmployeesData = await httpClient.GetAsync($"/api/FileUploadDB/SecretSanta/read/{SantaDataPassword}");
                EmployeesData = null;
                ShowSantaPasswordInput = false;
                SantaDataPassword = null;

                if(secretSantaEmployeesData.IsSuccessStatusCode)
                {
                    SecretSantaEmployeesData = JsonConvert.DeserializeObject<List<SecretSantaData>>(await secretSantaEmployeesData.Content.ReadAsStringAsync());
                }
                else
                {
                    ErrorMessage = $"Error Occured, Reason: {await secretSantaEmployeesData.Content.ReadAsStringAsync()}";
                }
            }
        }

        

        private void ClearInfoLabels()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }


        private bool isPopupVisible = false;

      

        protected override async Task OnInitializedAsync()
        {
            var entity = await httpClient.GetAsync($"/api/FileUploadDB/entity/read");
            if (entity.IsSuccessStatusCode)
            {
                entities = JsonConvert.DeserializeObject<List<Entity>>(await entity.Content.ReadAsStringAsync());
            }
            else
            {
                ErrorMessage = $"Error Occured, Reason: {await entity.Content.ReadAsStringAsync()}";
            }
        }

        

        void ClosePopup()
        {
            isPopupVisible = false;
            isSuccess = false;
            StateHasChanged();
        }
      
    }
}
