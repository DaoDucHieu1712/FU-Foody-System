import axiosConfig from "../../../shared/api/axiosConfig";
import CookieService from "../../../shared/helper/cookieConfig";

const OrderService = {
	async FindByUser(id, ctx) {
		const url =
			`/api/Order/MyOrder/${id}?` +
			`PageIndex=${ctx.queryKey[1]}&` +
			`StartDate=${ctx.queryKey[2]}&` +
			`EndDate=${ctx.queryKey[3]}&` +
			`Status=${ctx.queryKey[4]}&` +
			`SortType=${ctx.queryKey[5]}&` +
			`OrderId=${ctx.queryKey[6]}&` +
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
	async GetOrderDetail(id) {
		const url = "/api/Order/FindById/" + id;
		return axiosConfig.get(url);
	},
	async AcceptOrderWithShipper(id) {
		const url = "/api/Order/AcceptOrderWithShipper/" + id;
		return axiosConfig.put(url);
	},
	async CancelOrderWithCustomer(id, reason) {
		const url =
			"/api/Order/CancelOrderWithCustomer/" + id + `?CancelReason=${reason}`;
		return axiosConfig.put(url);
	},
	async CancelOrderWithShipper(id, reason) {
		const url =
			"/api/Order/CancelOrderWithShipper/" + id + `?CancelReason=${reason}`;
		return axiosConfig.put(url);
	},
	async GetOrderPendingWithShipper() {
		const uid = CookieService.getToken("fu_foody_id");
		const url = "/api/Order/GetOrderPendingWithShipper/" + uid;
		return axiosConfig.get(url);
	},
	async GetOrderIdel(page) {
		const url = "/api/Order/GetOrderIdel?" + `PageIndex=${page}`;
		return axiosConfig.get(url);
	},
	async CheckReceiverOrder(id) {
		const url = "/api/Order/CheckReceiverOrder/" + id;
		return axiosConfig.get(url);
	},
	async Order(data) {
		const url = `/api/Order/Order`;
		return axiosConfig.post(url, data);
	},
};

export default OrderService;
