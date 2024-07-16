using Azure;
using Azure.Security.KeyVault.Secrets;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;

namespace VendorManagementSystem.Infrastructure.Services
{


    public class KeyVaultService : IKeyVaultService
    {
        private readonly SecretClient _secretClient;
        public KeyVaultService(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }
        public ApplicationResponseDto<string> GetSecret(string key)
        {
            var resposne = new ApplicationResponseDto<string>();

            try
            {
                var secret = _secretClient.GetSecret(key);
                var value = secret.Value.Value;
                resposne.Data = value;
                
            }
            catch(Exception ex)
            {
                resposne.Error = new Error { Message = [ex.Message] };
            }
            return resposne;
        }
    }
}
