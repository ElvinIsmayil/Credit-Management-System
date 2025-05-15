using AutoMapper;
using Credit_Management_System.Models.Common;
using Credit_Management_System.Repositories.Interfaces;

namespace Credit_Management_System.Services.Implementations
{
    public class GenericService<TViewModel, TModel> : IGenericService<TViewModel, TModel> where TViewModel : class where TModel : BaseEntity, new()
    {
        protected readonly IGenericRepository<TModel> _repository;
        protected readonly IMapper _mapper;

        public GenericService(IGenericRepository<TModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TViewModel> AddAsync(TViewModel viewModel)
        {
            var data = _mapper.Map<TModel>(viewModel);
            var result = await _repository.AddAsync(data);
            if (result is null)
            {
                return null;
            }
            var VM = _mapper.Map<TViewModel>(result);

            return VM;
        }

        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<IEnumerable<TViewModel>> GetAllAsync()
        {
            var datas = await _repository.GetAllAsync();
            var VMs = _mapper.Map<IEnumerable<TViewModel>>(datas);
            return VMs;
        }

        public async Task<TViewModel> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            if(data is null)
            {
                return null;
            }   
            var VM = _mapper.Map<TViewModel>(data);
            return VM;
        }

        public async Task<TViewModel> UpdateAsync(TViewModel viewModel)
        {
            var data = _mapper.Map<TModel>(viewModel);

            var result = await _repository.UpdateAsync(data);
            if (result is null)
            {
                return null;
            }
            var VM = _mapper.Map<TViewModel>(result);

            return VM;

        }
    }
}
