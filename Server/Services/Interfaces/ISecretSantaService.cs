namespace ADIRA.Server.Services.Interfaces
{
    public interface ISecretSantaService
    {
        public Task<int> AllotSecretSanta(string empId,int entityId,string location);
        public Task<int> SendSecretSantaMails();
    }
}
