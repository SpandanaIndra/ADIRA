using ADIRA.Shared.BusinessDataObjects;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;

namespace ADIRA.Client.Pages
{
    public partial class RevealSecretSanta
    {
        private string selectedEntity;
        private string selectedLocation;
        private bool buttonClicked;
        private string selectedYear;

        private List<Entity> entities;
        private List<EmployeeLocation> locations = new List<EmployeeLocation>
        {

            new EmployeeLocation { Name="Hyderabad"},
            new EmployeeLocation { Name="Banglore"}
        };


        [Inject]
        public HttpClient httpClient { get; set; }
        private bool isLoading;
        private bool isSuccess;
        private List<Employee> EmployeesData { get; set; }
        private List<SecretSantaData> SecretSantaEmployeesData { get; set; }
        private string ErrorMessage { get; set; }
        private string SuccessMessage { get; set; }
        private bool SendEmailButtonDisabled { get; set; } = false;
        private string SantaDataPassword { get; set; }
        private bool ShowSantaPasswordInput { get; set; } = false;
        int currentYear;
        private string noDataMessage { get;set; }
        private void ClearInfoLabels()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            noDataMessage= string.Empty;
        }
        
        private List<string> AvailableYears = new List<string>();// { "2022", "2023", "2024" };
        int entId;
        string loc;
        private async void GetSantaFilteredData()
        {
            ClearInfoLabels();
            currentYear = Convert.ToInt32(selectedYear);
            buttonClicked = true;
            if (!string.IsNullOrEmpty(selectedLocation) && !string.IsNullOrEmpty(selectedEntity) && !string.IsNullOrEmpty(selectedYear))
            {
               

                if (selectedEntity != "")
                {
                    entId = Convert.ToInt32(selectedEntity);

                }
                if(selectedLocation!="")
                {
                     loc = selectedLocation;
                }
                
                 if (selectedYear == "")
                {
                    currentYear = DateTime.Now.Year;
                }
                else
                {
                    currentYear = Convert.ToInt32(selectedYear);

                }


                // Perform actions based on the selected year
                var secretSantaEmployeesData = await httpClient.GetAsync($"/api/FileUploadDB/SecretSanta/read/{currentYear}/{entId}/{loc}");



                if (secretSantaEmployeesData.IsSuccessStatusCode)
                {
                    SecretSantaEmployeesData = JsonConvert.DeserializeObject<List<SecretSantaData>>(await secretSantaEmployeesData.Content.ReadAsStringAsync());
                    StateHasChanged();
                }
                else
                {
                    SecretSantaEmployeesData =null;
                   

                    //ErrorMessage = $"Error Occured, Reason: {await secretSantaEmployeesData.Content.ReadAsStringAsync()}";
                    noDataMessage = "No Data to display..!";
                    StateHasChanged();
                }

            }

        }
        private async void YearSelected(ChangeEventArgs args)
        {
            /*ClearInfoLabels();
            SelectedYear = args.Value.ToString();
            if(SelectedYear=="")
            {
                currentYear = DateTime.Now.Year;
            }
            else
            {
                currentYear = Convert.ToInt32(SelectedYear);

            }*/
          /*  // Perform actions based on the selected year
            var secretSantaEmployeesData = await httpClient.GetAsync($"/api/FileUploadDB/SecretSanta/read/{currentYear}");



            if (secretSantaEmployeesData.IsSuccessStatusCode)
            {
                SecretSantaEmployeesData = JsonConvert.DeserializeObject<List<SecretSantaData>>(await secretSantaEmployeesData.Content.ReadAsStringAsync());
                StateHasChanged();
            }
            else
            {
                ErrorMessage = $"Error Occured, Reason: {await secretSantaEmployeesData.Content.ReadAsStringAsync()}";
            }*/
        }



        protected override async Task OnInitializedAsync()
        {
            ClearInfoLabels();

               currentYear = DateTime.Now.Year;
            //currentYear = 2021;
            var secretSantaEmployeesData = await httpClient.GetAsync($"/api/FileUploadDB/SecretSanta/read/{currentYear}");


            if (secretSantaEmployeesData.IsSuccessStatusCode)
            {
                SecretSantaEmployeesData = JsonConvert.DeserializeObject<List<SecretSantaData>>(await secretSantaEmployeesData.Content.ReadAsStringAsync());
                StateHasChanged();
                
            }
            else
            {
                ErrorMessage = $"Error Occured, Reason: {await secretSantaEmployeesData.Content.ReadAsStringAsync()}";
            }

            //for retrieving years
            var years = await httpClient.GetAsync($"/api/FileUploadDB/createdDate/read");

            if (years.IsSuccessStatusCode)
            {
                AvailableYears=JsonConvert.DeserializeObject<List<string>>(await years.Content.ReadAsStringAsync());
            }
            else
            {
                ErrorMessage = $"Error Occured, Reason: {await secretSantaEmployeesData.Content.ReadAsStringAsync()}";
            }

            //for retrieving entities

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
       
    }
}
