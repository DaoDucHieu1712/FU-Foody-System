import { createSlice } from "@reduxjs/toolkit";

const CartLocalStorage = JSON.parse(localStorage.getItem("cart") || "[]");

const initialState = {
  cart: CartLocalStorage,
  totalPrice: 0,
  totalQuantity: 0,
};

const cartSlice = createSlice({
  name: "cart",
  initialState,
  reducers: {
    addToCart: (state, action) => {
      let find = state.cart.findIndex(
        (item) =>
          item.productId === action.payload.productId &&
          item.size == action.payload.size
      );
      if (find >= 0) {
        state.cart[find].quantity += 1;
      } else {
        state.cart.push(action.payload);
      }
      localStorage.setItem("cart", JSON.stringify(state.cart));
    },

    getCartTotal: (state) => {
      let { totalQuantity, totalPrice } = state.cart.reduce(
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
      state.cart = state.cart.filter(
        (item) => JSON.stringify(item) !== JSON.stringify(action.payload)
      );
      localStorage.setItem("cart", JSON.stringify(state.cart));
    },

    increaseItemQuantity: (state, action) => {
      state.cart = state.cart.map((item) => {
        if (JSON.stringify(item) === JSON.stringify(action.payload)) {
          return { ...item, quantity: item.quantity + 1 };
        }
        return item;
      });
      localStorage.setItem("cart", JSON.stringify(state.cart));
    },

    decreaseItemQuantity: (state, action) => {
      let isRemove = false;
      state.cart = state.cart.map((item) => {
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
        state.cart = state.cart.filter(
          (item) => JSON.stringify(item) !== JSON.stringify(action.payload)
        );
      }

      localStorage.setItem("cart", JSON.stringify(state.cart));
    },
    clearCart: (state) => {
      state.cart = [];
      localStorage.setItem("cart", JSON.stringify(state.cart));
    },
  },
});

export const cartActions = cartSlice.actions;
export const cartSelector = (state) => state.cart;
export const cartReducer = cartSlice.reducer;
export default cartReducer;
