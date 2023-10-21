import { createSlice } from "@reduxjs/toolkit";



const initialState = { 
  userProfile: null,
  accessToken: null, 
};

const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
      dismiss: () => initialState,
  
  
      setUserProfile: (state, action) => {
        state.userProfile = action.payload;
      },
  
  
      setAccessToken(state, action) {
        state.accessToken = action.payload;
      },
  
    },
  });
  export const {
    setAccessToken,
    setUserProfile,
  } = authSlice.actions;
  export default authSlice;
  