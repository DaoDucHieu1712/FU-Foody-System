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

	async CalcFeeShip(fromDistrictId, toDistrictID, toWardCode, item) {
		const headers = {
			Token: "6c942378-8c0f-11ee-a6e6-e60958111f48",
			ShopId: 190398,
		};
		const url =
			"https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee";
		const data = {
			service_id: 53320,
			from_district_id: fromDistrictId,
			to_district_id: toDistrictID,
			to_ward_code: toWardCode,
			weight: 10,
			items: item,
		};
		return axios
			.post(url, data, { headers: headers })
			.then((response) => {
				return response;
			})
			.catch((error) => {
				console.error(error);
			});
	},
	async CalcTimeShip(fromDistrictId, fromWardCode, toDistrictID, toWardCode) {
		const headers = {
			Token: "6c942378-8c0f-11ee-a6e6-e60958111f48",
			ShopId: 190398,
		};
		const url =
			"https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/leadtime";

		const data = {
			from_district_id: fromDistrictId,
			from_ward_code: fromWardCode,
			to_district_id: toDistrictID,
			to_ward_code: toWardCode,
			service_id: 53320,
		};
		return axios
			.post(url, data, { headers: headers })
			.then((response) => {
				return response;
			})
			.catch((error) => {
				console.error(error);
			});
	},
};

export default CartService;
