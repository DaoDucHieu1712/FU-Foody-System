import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.jsx";
import "./styles/index.scss";
import { Provider } from "react-redux";
import store from "../src/redux/store.js";
import "react-toastify/dist/ReactToastify.css";
import { BrowserRouter } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@material-tailwind/react";
import ProfilePlaceHolder from "./placeholder/profilePlaceHolder.js";
import { LazyLoadComponent } from "./app/(auth)/Layout.jsx";

ReactDOM.createRoot(document.getElementById("root")).render(
  <Provider store={store}>
    <ThemeProvider>
      <BrowserRouter>
        <App />
        <LazyLoadComponent>
          <ProfilePlaceHolder />
        </LazyLoadComponent>
        <ToastContainer></ToastContainer>
      </BrowserRouter>
    </ThemeProvider>
  </Provider>
);
