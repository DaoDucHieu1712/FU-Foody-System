using System.Data;
using Dapper;
using FFS.Application.Data;
using FFS.Application.Entities;

namespace FFS.Application.Repositories.Impls
{
	public class ComboRepository : EntityRepository<Combo, int>, IComboRepository
	{
		private readonly DapperContext _dapperContext;
		public ComboRepository(ApplicationDbContext context, DapperContext dapperContext) : base(context)
		{
			_dapperContext = dapperContext;
		}

		public async Task AddComboFood(int comboId, int storeId, List<int> idFoods)
		{
			foreach (int id in idFoods)
			{
				FoodCombo foodCombo = new FoodCombo()
				{
					ComboId = comboId,
					StoreId = storeId,
					FoodId = id,
				};
				_ = await _context.AddAsync(foodCombo);
			}
			_ = await _context.SaveChangesAsync();
		}

		public void DeleteCombo(int comboId)
		{
			Combo combo = _context.Combos.FirstOrDefault(x => x.Id == comboId);
			List<FoodCombo> foodCombos = _context.FoodCombos.Where(x => x.Id == comboId).ToList();
			foreach (FoodCombo cb in foodCombos)
			{
				cb.IsDelete = true;
			}
			combo.IsDelete = true;
			_ = _context.SaveChanges();
		}

		public void DeleteDetail(int id)
		{
			FoodCombo foodCombos = _context.FoodCombos.FirstOrDefault(x => x.Id == id);
			foodCombos.IsDelete = true;
			_ = _context.SaveChanges();
		}

		public async Task<List<dynamic>> GetDetail(int id)
		{
			IEnumerable<dynamic> returnData = null;
			var parameters = new DynamicParameters();
			parameters.Add("comboId", id);
			using (var db = _dapperContext.connection)
			{

				returnData = await db.QueryAsync<dynamic>("GetDetailCombo", parameters, commandType: CommandType.StoredProcedure);
			}
			return returnData.ToList();
		}

		public void UpdateComboFood(int comboId, int storeId, List<int> idFoods)
		{
			var existingFoodCombos = _context.FoodCombos
				.Where(fc => fc.ComboId == comboId && fc.StoreId == storeId)
				.ToList();

			_context.RemoveRange(existingFoodCombos);

			foreach (int id in idFoods)
			{
				FoodCombo foodCombo = new FoodCombo()
				{
					ComboId = comboId,
					StoreId = storeId,
					FoodId = id,
				};
				_ = _context.Add(foodCombo);
			}

			_ = _context.SaveChanges();
		}
	}
}
