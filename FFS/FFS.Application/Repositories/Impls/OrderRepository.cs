using AutoMapper;
using FFS.Application.Data;
using FFS.Application.DTOs.Order;
using FFS.Application.Entities;

namespace FFS.Application.Repositories.Impls
{
    public class OrderRepository : EntityRepository<Order, int>, IOrderRepository
    {
        protected readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;
        public OrderRepository(ApplicationDbContext context, IOrderDetailRepository orderDetailRepository, IMapper mapper) : base(context)
        {
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
        }

        public async Task AddOrder(List<OrderDetailDTO> orderDetailDTOs)
        {
            try
            {
                var items = _mapper.Map<List<OrderDetail>>(orderDetailDTOs);
                await _orderDetailRepository.AddMultiple(items);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderDTO> CreateOrder(OrderRequestDTO orderRequestDTO)
        {
            try
            {
                var NewOrder = await CreateAndGetEntity(_mapper.Map<Order>(orderRequestDTO));
                if (NewOrder == null) throw new Exception("Something wrong !");
                return _mapper.Map<OrderDTO>(NewOrder);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
