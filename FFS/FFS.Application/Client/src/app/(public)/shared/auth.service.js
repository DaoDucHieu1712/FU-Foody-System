import axiosConfig from "../../../shared/api/axiosConfig";

const AuthServices = {
  storeRegister(data) {
    const url = "/Authenticate/StoreRegister";
    return axiosConfig.post(url, data);
  },
};

export default AuthServices;
