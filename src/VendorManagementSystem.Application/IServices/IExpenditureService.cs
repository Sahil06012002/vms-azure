using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.Expenditure;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public interface IExpenditureService
    {
        public ApplicationResponseDto<bool> AddExpenditure(List<ExpenditureDTO> expenditureDto, string token, int id);

        public ApplicationResponseDto<List<Expenditure>> GetAllExpenditure(int id);

        public ApplicationResponseDto<IEnumerable<object>> GetTopExpenditure(int count);
    }
}
