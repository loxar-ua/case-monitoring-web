using ShkandalData.Common;
using shkandalData.DTOs.ClusterDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShkandalData.Models;

namespace ShkandalServices
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetAllAsync();
        public Task<Category?> GetByIdAsync(int id);
    }
}
