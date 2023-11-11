import { configureStore } from "@reduxjs/toolkit";
import authSlice from "./auth";
import cartReducer from "../app/(auth)/shared/cartSlice";

const store = configureStore({
  reducer: {
    auth: authSlice?.reducer,
    cart: cartReducer,
  },
});

export default store;
