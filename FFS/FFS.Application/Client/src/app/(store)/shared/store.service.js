import axiosConfig from "../../../shared/api/axiosConfig";

const StoreService = {
  async GetStore(id) {
    const url = `/api/Store/GetStoreByUid?uId=${id}`;
    return axiosConfig.get(url);
  },
};

export default StoreService;
