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
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import ChatPage from "./app/(auth)/ChatPage.jsx";
import Cookies from "universal-cookie";

const queryClient = new QueryClient();
var cookie = new Cookies();
ReactDOM.createRoot(document.getElementById("root")).render(
	<Provider store={store}>
		<ThemeProvider>
			<BrowserRouter>
				<QueryClientProvider client={queryClient}>
					<App />
					{cookie.get("fu_foody_token") && <ChatPage />}
				</QueryClientProvider>
				<LazyLoadComponent>
					<ProfilePlaceHolder />
				</LazyLoadComponent>
				<ToastContainer></ToastContainer>
			</BrowserRouter>
		</ThemeProvider>
	</Provider>
);
