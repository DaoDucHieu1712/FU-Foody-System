import { configureStore } from "@reduxjs/toolkit";
import authSlice from "./auth";
import cartReducer from "../app/(auth)/shared/cartSlice";
import chatReducer from "../app/(auth)/shared/chatSlice";

const store = configureStore({
	reducer: {
		auth: authSlice?.reducer,
		cart: cartReducer,
		chat: chatReducer,
	},
});

export default store;
