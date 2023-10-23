import axiosConfig from "../../../shared/api/axiosConfig";

const AuthServices = {
  storeRegister(data) {
    const url = "/api/Authenticate/StoreRegister";
    return axiosConfig.post(url, data);
  },
  changePassword(data) {
    const url = "/api/Authenticate/ChangePassword";
    return axiosConfig.post(url, data);
  },
};

export default AuthServices;
