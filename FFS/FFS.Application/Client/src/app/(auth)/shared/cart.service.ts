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
};

export default CartService;
