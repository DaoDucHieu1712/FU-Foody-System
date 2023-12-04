import { configureStore } from "@reduxjs/toolkit";
import authSlice from "./auth";
import cartReducer from "../app/(auth)/shared/cartSlice";
import chatReducer from "../app/(auth)/shared/chatSlice";
import notificationReducer from "../app/(auth)/shared/notificationSlice";
import comboReducer from "../app/(auth)/shared/comboSlice";

const store = configureStore({
	reducer: {
		auth: authSlice?.reducer,
		cart: cartReducer,
		chat: chatReducer,
		notification: notificationReducer,
		combo: comboReducer,
	},
});

export default store;
