import axios from "axios";
import axiosConfig from "../../../shared/api/axiosConfig";

const CartService = {
	async CreateOrder(data) {
		const url = "/api/Order/CreaterOrder";
		return axiosConfig.post(url, data);
	},
	async AddOrderItem(data) {
		const url = "/api/Order/AddOrderItem";
		return axiosConfig.post(url, data);
	},
	async CheckDiscount(code, userId, totalPrice, storeIds) {
		const url =
			"/api/Discount/CheckDiscount?" +
			`Code=${code}&` +
			`UserId=${userId}&` +
			`TotalPrice=${totalPrice}`;
		return axiosConfig.post(url, storeIds);
	},
	async UseDiscount(code, userId) {
		const url =
			"/api/Discount/UseDiscount?" + `Code=${code}&` + `UserId=${userId}`;
		return axiosConfig.get(url);
	},

	async CalcFeeShip(addressStore, addressUser) {
		const data = {
			addressStore: addressStore,
			addressUser: addressUser,
		};
		const url = "/api/Store/CalcFeeship";
		return axiosConfig
			.post(url, data)
			.then((response) => {
				return response;
			})
			.catch((error) => {
				console.error(error);
			});
	},
};

export default CartService;
