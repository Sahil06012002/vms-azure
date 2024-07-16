using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.Item;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Item;

namespace VendorManagementSystem.Application.IServices
{
    public interface IItemService
    {
        public ApplicationResponseDto<ItemFormDto> GetItemFormDetails();
        public ApplicationResponseDto<int> AddItem(string jwtToken, ItemDto itemDto);
        public ApplicationResponseDto<IEnumerable<ItemResponseDto>> GetItemService();
    }
}
