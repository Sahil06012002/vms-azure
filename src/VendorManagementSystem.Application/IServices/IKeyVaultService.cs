using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.UtilityDtos;

namespace VendorManagementSystem.Application.IServices
{
    public interface IKeyVaultService
    {
        public ApplicationResponseDto<string> GetSecret(string key);

    }
}
