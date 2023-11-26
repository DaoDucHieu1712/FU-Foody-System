import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  notifications: [],
};

const notificationSlice = createSlice({
    name: 'notification',
    initialState,
    reducers: {
      addNotification: (state, action) => {
        return {
          ...state,
          notifications: [...state.notifications, action.payload],
        };
      },
    },
  });

export const { addNotification } = notificationSlice.actions;
export const selectNotifications = (state) => state.notification.notifications;
export const notificationReducer = notificationSlice.reducer;
export default notificationReducer;