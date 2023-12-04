import { createSlice } from "@reduxjs/toolkit";

const cartcombo = "fu_foody_combo";

const ComboLocalStorage = JSON.parse(localStorage.getItem(cartcombo) || "[]");

const initialState = {
	list: ComboLocalStorage,
	totalPrice: 0,
	totalQuantity: 0,
};

const comboSlice = createSlice({
	name: "combo",
	initialState,
	reducers: {
		addToCart: (state, action) => {
			console.log(action.payload.id);
			let find = state.list.findIndex((item) => item.id === action.payload.id);
			if (find >= 0) {
				state.list[find].quantity += 1;
			} else {
				state.list.push(action.payload);
			}
			localStorage.setItem(cartcombo, JSON.stringify(state.list));
		},

		getCartTotal: (state) => {
			let { totalQuantity, totalPrice } = state.list.reduce(
				(cartTotal, cartItem) => {
					const { price, quantity } = cartItem;
					const itemTotal = price * quantity;
					cartTotal.totalPrice += itemTotal;
					cartTotal.totalQuantity += quantity;
					return cartTotal;
				},
				{ totalPrice: 0, totalQuantity: 0 }
			);
			state.totalPrice = parseInt(totalPrice.toFixed(2));
			state.totalQuantity = totalQuantity;
		},

		removeItem: (state, action) => {
			state.list = state.list.filter(
				(item) => JSON.stringify(item) !== JSON.stringify(action.payload)
			);
			localStorage.setItem(cartcombo, JSON.stringify(state.list));
		},

		increaseItemQuantity: (state, action) => {
			state.list = state.list.map((item) => {
				if (JSON.stringify(item) === JSON.stringify(action.payload)) {
					return { ...item, quantity: item.quantity + 1 };
				}
				return item;
			});
			localStorage.setItem(cartcombo, JSON.stringify(state.list));
		},

		decreaseItemQuantity: (state, action) => {
			let isRemove = false;
			state.list = state.list.map((item) => {
				if (JSON.stringify(item) === JSON.stringify(action.payload)) {
					if (item.quantity <= 1) {
						isRemove = true;
					} else {
						return { ...item, quantity: item.quantity - 1 };
					}
				}
				return item;
			});

			if (isRemove) {
				state.list = state.list.filter(
					(item) => JSON.stringify(item) !== JSON.stringify(action.payload)
				);
			}

			localStorage.setItem(cartcombo, JSON.stringify(state.list));
		},
		clearCart: (state) => {
			state.list = [];
			localStorage.setItem(cartcombo, JSON.stringify(state.list));
		},
		useDiscount: (state, action) => {
			// state.list = state.list.map((item) => {
			// 	if (item.storeId == action.payload.storeId) {
			// 		return {
			// 			...item,
			// 			price: item.price - item.price * action.payload.discount,
			// 		};
			// 	} else {
			// 		return item;
			// 	}
			// });
			// localStorage.setItem(cartfood, JSON.stringify(state.list));
			state.totalPrice = state.totalPrice * action.payload.discount;
		},
		updateTotalPrice: (state, action) => {
			state.totalPrice = action.payload;
		},
	},
});

export const comboActions = comboSlice.actions;
export const comboSelector = (state) => state.list;
export const comboReducer = comboSlice.reducer;
export default comboReducer;
