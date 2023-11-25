import { createSlice } from "@reduxjs/toolkit";

const chatSlice = createSlice({
	name: "chat",
	initialState: { IsShow: false, CurrentBox: "" },
	reducers: {
		Update: (state, action) => {
			state.IsShow = action.payload;
			console.log(state.IsShow);
		},
	},
});

export const chatActions = chatSlice.actions;
export const chatSelector = (state) => state.chat;
export const chatReducer = chatSlice.reducer;
export default chatReducer;
