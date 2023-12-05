import axiosConfig from "../../../shared/api/axiosConfig";

const AuthServices = {
	storeRegister(data) {
		const url = "/api/Authenticate/StoreRegister";
		return axiosConfig.post(url, data);
	},
	storeUpdate(id, data) {
		const url = "/api/Store/UpdateStore/" + id;
		return axiosConfig.put(url, data);
	},
	changePassword(data) {
		const url = "/api/Authenticate/ChangePassword";
		return axiosConfig.post(url, data);
	},

	shipperRegister(data) {
		const url = "/api/Authenticate/RegisterShipper";
		return axiosConfig.post(url, data);
	},
};

export default AuthServices;
