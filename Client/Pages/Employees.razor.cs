using ADIRA.Shared.BusinessDataObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Net.Http;

namespace ADIRA.Client.Pages
{
    public partial class Employees
    {
        [Inject]
        public IJSRuntime jsRuntime { get; set; }
        [Inject]
        public HttpClient httpClient { get; set; }
        [Parameter]
        public List<Employee> EmployeesData { get; set; }
        [Parameter]
        public EventCallback<bool> Result { get; set; }
        protected async void DeleteEmployee(string id)
        {
          bool res = await jsRuntime.InvokeAsync<bool>("confirm", "Click OK to delete Employee permanently..!!");
            if(res)
            {
                // Perform actions based on the selected year
                var response = await httpClient.DeleteAsync($"/api/FileUploadDB/delete/{id}");


                if (response.IsSuccessStatusCode)
                {
                    await Result.InvokeAsync(true);

                    StateHasChanged();
                }
                else
                {
                    await Result.InvokeAsync(false);

                    //ErrorMessage = $"Error Occured, Reason: {await secretSantaEmployeesData.Content.ReadAsStringAsync()}";
                    StateHasChanged();
                }
            }
           

        }
        protected void EditEmployee(Employee employee)
        {

        }
    }
}
