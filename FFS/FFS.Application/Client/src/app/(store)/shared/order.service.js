import axiosConfig from "../../../shared/api/axiosConfig";

const OrderService = {
	async FindByUser(id, ctx) {
		const url =
			`/api/Order/MyOrder/${id}?` +
			`PageIndex=${ctx.queryKey[1]}&` +
			`StartDate=${ctx.queryKey[2]}&` +
			`EndDate=${ctx.queryKey[3]}&` +
			`ToPrice=${ctx.queryKey[4]}&` +
			`FromPrice=${ctx.queryKey[5]}&` +
			`Status=${ctx.queryKey[6]}&` +
			`SortType=${ctx.queryKey[7]}&` +
			`OrderId=${ctx.queryKey[8]}&` +
			``;
		return axiosConfig.get(url);
	},
	async FindByStore(id, ctx) {
		const url =
			`/api/Order/GetOrderWithStore/${id}?` +
			`PageIndex=${ctx.queryKey[1]}&` +
			`StartDate=${ctx.queryKey[2]}&` +
			`EndDate=${ctx.queryKey[3]}&` +
			`ToPrice=${ctx.queryKey[4]}&` +
			`FromPrice=${ctx.queryKey[5]}&` +
			`Status=${ctx.queryKey[6]}&` +
			`SortType=${ctx.queryKey[7]}&` +
			``;
		return axiosConfig.get(url);
	},
};

export default OrderService;
