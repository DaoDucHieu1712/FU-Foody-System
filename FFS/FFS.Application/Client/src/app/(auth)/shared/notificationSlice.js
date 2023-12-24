import { createSlice } from '@reduxjs/toolkit';
import axios from "../../../shared/api/axiosConfig";
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

      markAllAsRead: (state) => {
        return {
          ...state,
          notifications: state.notifications.map((notification) => ({
            ...notification,
            isRead: true,
          })),
        };
      },
      
    },
  });
  
export const { addNotification, markAllAsRead  } = notificationSlice.actions;
export const selectNotifications = (state) => state.notification.notifications;
export const notificationReducer = notificationSlice.reducer;
export default notificationReducer;