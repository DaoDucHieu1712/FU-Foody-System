using AutoMapper;

using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FFS.Application.Repositories.Impls {
    public class StoreRepository : IStoreRepository {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StoreRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<StoreInforDTO> GetInformationStore(int id)
        {
            try
            {
                Store? stores = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id);
                if(stores == null)
                {
                    throw new Exception("Cửa hàng không tồn tại!");
                }
                else
                {
                    StoreInforDTO storeInforDTO = _mapper.Map<StoreInforDTO>(stores);
                    return storeInforDTO;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
