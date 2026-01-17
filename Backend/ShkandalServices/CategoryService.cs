using AutoMapper;
using ShkandalData.Models;
using ShkandalInfrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShkandalServices
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
        }

        public async Task<List<Category>> GetAllAsync() {
            var categories = await _repository.GetAllCategoriesAsync();
            return categories;
        }
        public async Task<Category?> GetByIdAsync(int id) {

            var category = await _repository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return null;
            }

            return category;
        }
    }
}
