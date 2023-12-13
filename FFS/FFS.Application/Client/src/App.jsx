import { Route, Routes } from "react-router-dom";
import Login from "./app/(public)/Login";
import Layout from "./app/(auth)/Layout";
import StoreRegisterPage from "./app/(public)/StoreRegisterPage";
import ChangePasswordPage from "./app/(auth)/ChangePasswordPage";
import ForgotPasswordPage from "./app/(public)/ForgotPasswordPage";
import FoodDetails from "./app/(public)/FoodDetails";
import Location from "./app/(auth)/Location";
import Inventory from "./app/(auth)/shared/components/inventory/Inventory";
import ProfilePage from "./app/(auth)/ProfilePage";
import ResetPasswordPage from "./app/(public)/ResetPassWord";
import CartPage from "./app/(auth)/CartPage";
import Food from "./app/(store)/Food";
import HomePage from "./app/(public)/HomePage";
import StoreProfilePage from "./app/(store)/StoreProfilePage";
import StoreDetailPage from "./app/(store)/StoreDetailPage";
import Post from "./app/(public)/Post";
import DetailPost from "./app/(auth)/shared/components/post/DetailPost";
import StoreCommentPage from "./app/(store)/StoreCommentPage";
import StoreLayout from "./app/(store)/StoreLayout";
import MyOrder from "./app/(auth)/MyOrder";
import Discount from "./app/(store)/Discount";
import FoodList from "./app/(public)/FoodList";
import Wishlist from "./app/(public)/Wishlist";
import AdminLayout from "./app/(admin)/AdminLayoutPage";
import ReportPage from "./app/(admin)/ReportPage";
import DashboardPage from "./app/(admin)/DashboardPage";
import AccountManagePage from "./app/(admin)/AccountManagePage";
import RequestAccountPage from "./app/(admin)/RequestAccountPage";
import Category from "./app/(store)/Category";
import OrderList from "./app/(store)/OrderList";
import FlashSale from "./app/(store)/FlashSale";
import ShipperDetailsPage from "./app/(auth)/ShipperDetailsPage";
import AddFlashSale from "./app/(store)/components/FlashSale/AddFlashSale";
import StoreList from "./app/(public)/StoreList";
import ShipperLayout from "./app/(shipper)/ShipperLayout";
import OrderAvailablePage from "./app/(shipper)/OrderAvailablePage";
import OrderShippingPage from "./app/(shipper)/OrderShippingPage";
import ShipperStatisticPage from "./app/(shipper)/ShipperStatisticPage";
import OrderFinishedPage from "./app/(shipper)/OrderFinishedPage";
import UserDetails from "./app/(public)/UserDetails";
import ShipperRegisterPage from "./app/(public)/ShipperRegister";
import AccessDenied from "./app/(public)/AccessDenied";
import NotFoundPage from "./app/NotFoundPage";
import PaymentPage from "./app/(auth)/PaymentPage";
import ConfirmPaymentPage from "./app/(public)/ConfirmPaymentPage";
import MyOrderDetail from "./app/(auth)/MyOrderDetail";
import OrderDetail from "./app/(store)/OrderDetail";
import ViewApplicationPage from "./app/(admin)/ViewApplicationPage";

import Checkout from "./app/(auth)/Checkout";

import PostManagePage from "./app/(admin)/PostManagePage";
import OrderIdelDetail from "./app/(shipper)/OrderIdelDetail";
import StoreDashboardPage from "./app/(store)/StoreDashboard";
import ShipperProfile from "./app/(shipper)/ShipperProfile";

import StoreOwnderApplication from "./app/(admin)/shared/components/StoreOwnderApplication";
import ShipperApplication from "./app/(admin)/shared/components/ShipperApplication";

import StoreEditPage from "./app/(store)/StoreEditPage";

function App() {
	return (
		<>
			<Routes>
				<Route path="/" element={<Layout />}>
					<Route path="/confirm-payment" element={<ConfirmPaymentPage />} />
					<Route path="/" element={<HomePage />} />
					<Route path="/login" element={<Login />} />
					<Route path="/location" element={<Location />} />
					<Route path="/register-store" element={<StoreRegisterPage />} />
					<Route path="/register-shipper" element={<ShipperRegisterPage />} />
					<Route path="/profile" element={<ProfilePage />} />
					<Route path="/change-password" element={<ChangePasswordPage />} />
					<Route path="/forgot-password" element={<ForgotPasswordPage />} />
					<Route path="/reset-password" element={<ResetPasswordPage />} />
					<Route path="/food-details/:id" element={<FoodDetails />} />
					<Route path="/cart" element={<CartPage />} />
					<Route path="/checkout" element={<Checkout />} />
					<Route path="/payment" element={<PaymentPage />} />
					<Route path="/store-profile/:id" element={<StoreProfilePage />} />
					<Route path="/store/detail/:id" element={<StoreDetailPage />} />
					<Route path="/post" element={<Post />} />
					<Route path="/post-details/:postId" element={<DetailPost />} />
					<Route path="/store/comment/:id" element={<StoreCommentPage />} />
					<Route path="/my-order" element={<MyOrder />} />
					<Route path="/food-list/:foodNameSearch" element={<FoodList />} />
					<Route path="/food-list" element={<FoodList />} />
					<Route path="/store-list" element={<StoreList />} />s
					<Route path="/wishlist" element={<Wishlist />} />
					<Route path="/user-detail/:id" element={<UserDetails />} />
					<Route path="/my-order/:id" element={<MyOrderDetail />} />
					<Route path="/shipper/details/:id" element={<ShipperDetailsPage />} />
				</Route>
				<Route element={<ShipperLayout></ShipperLayout>}>
					<Route
						path="/shipper/order-pending"
						element={<OrderShippingPage />}
					/>
					<Route path="/shipper/profile" element={<ShipperProfile />} />

					<Route
						path="/shipper/order-available"
						element={<OrderAvailablePage />}
					/>
					<Route
						path="/shipper/order-available/:id"
						element={<OrderIdelDetail />}
					/>
					<Route
						path="/shipper/view-statistic"
						element={<ShipperStatisticPage />}
					/>
					<Route
						path="/shipper/order-shipped"
						element={<OrderFinishedPage />}
					/>
				</Route>
				<Route element={<StoreLayout></StoreLayout>}>
					<Route path="/store/edit" element={<StoreEditPage />} />

					<Route path="/store/order" element={<OrderList />} />
					<Route path="/store/food" element={<Food />} />
					<Route path="/store/inventory" element={<Inventory />} />
					<Route path="/store/category" element={<Category />} />
					<Route path="/store/discount" element={<Discount />} />user
					
					<Route path="/store/order/:id" element={<OrderDetail />} />
					<Route path="/store/flash-sale/add" element={<AddFlashSale />} />
					<Route path="/store/flash-sale" element={<FlashSale />} />
					<Route path="/store/manager" element={<StoreDashboardPage />} />
				</Route>
				<Route element={<AdminLayout></AdminLayout>}>
					<Route path="/admin/report" element={<ReportPage />} />
					<Route path="/admin/manage-post" element={<PostManagePage />} />

					<Route path="/admin/dashboard" element={<DashboardPage />} />
					<Route path="/admin/manage-acoount" element={<AccountManagePage />} />
					<Route
						path="/admin/request-account"
						element={<RequestAccountPage />}
					/>
					<Route
						path="/admin/application/:id"
						element={<ViewApplicationPage />}
					/>
					<Route
						path="/admin/application/store-owner/:id"
						element={<StoreOwnderApplication />}
					/>
					<Route
						path="/admin/application/shipper/:id"
						element={<ShipperApplication />}
					/>
				</Route>
				<Route path="*" element={<NotFoundPage />}></Route>
				<Route path="/access-denied" element={<AccessDenied />}></Route>
			</Routes>
		</>
	);
}

export default App;
