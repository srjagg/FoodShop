using FoodShop.Core.Util;
using FoodShop.Model.Models;
using FoodShop.Model.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Core.CoreInterface
{
    public interface IUserCore
    {
        Task<PetitionResponse<int>> AddUserAsync(UserDto userModel);
    }
}
