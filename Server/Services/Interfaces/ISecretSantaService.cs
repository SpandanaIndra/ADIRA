namespace ADIRA.Server.Services.Interfaces
{
    public interface ISecretSantaService
    {
        public Task<int> AllotSecretSanta();
        public Task<int> SendSecretSantaMails();
    }
}
