using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supermarket.API.Domain.Communication;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Repositories;
using Supermarket.API.Domain.Services;

namespace Supermarket.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            return await _categoryRepository.ListAsync();
        }

        public async Task<CategoryResponse> SaveAsync(Category category)
        {
            try
            {
                await _categoryRepository.AddAsync(category);
                await _unitOfWork.CompleteAsync();

                return new CategoryResponse(category);
            }
            catch(Exception ex)
            {
                return new CategoryResponse($"An error ocurred while saving the category: {ex.Message}");
            }
        }

        public async Task<CategoryResponse> UpdateAsync(int id, Category category)
        {
            var exitingCategory = await _categoryRepository.FindByIdAsync(id);

            if (exitingCategory== null)
            {
                return new CategoryResponse("Category not found");

            }

            exitingCategory.Name = category.Name;

            try
            {
                _categoryRepository.Update(exitingCategory);
                await _unitOfWork.CompleteAsync();
                return new CategoryResponse(exitingCategory);
            }
            catch (Exception ex)
            {
                return new CategoryResponse($"An error ocurred while updtading the Category: {ex.Message}");

            }
            
            
            throw new NotImplementedException();
        }

        public async Task<CategoryResponse> DeleteAsync(int id)
        {
            var existingCategory = await _categoryRepository.FindByIdAsync(id);

            if (existingCategory == null)
                return new CategoryResponse("Category not found.");


            try
            {
                _categoryRepository.Remove(existingCategory);
                await _unitOfWork.CompleteAsync();
                return new CategoryResponse(existingCategory);
            }
            catch (Exception)
            {

                return new CategoryResponse($"An error ocurred while deleiting category");
            }
        }

        Task<CategoryResponse> ICategoryService.SaveAsync(Category category)
        {
            throw new NotImplementedException();
        }

        Task<CategoryResponse> ICategoryService.UpdateAsync(int id, Category category)
        {
            throw new NotImplementedException();
        }

        Task<CategoryResponse> ICategoryService.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
