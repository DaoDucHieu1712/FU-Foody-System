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
import FlashSale from "./app/(store)/FlashSale";
import ShipperDetailsPage from "./app/(auth)/ShipperDetailsPage";
import AddFlashSale from "./app/(store)/components/FlashSale/AddFlashSale";
import StoreList from "./app/(public)/StoreList";
import ShipperLayout from "./app/(shipper)/ShipperLayout";
import OrderAvailablePage from "./app/(shipper)/OrderAvailablePage";
import ShipperStatisticPage from "./app/(shipper)/ShipperStatisticPage";
import OrderFinishedPage from "./app/(shipper)/OrderFinishedPage";
import UserDetails from "./app/(public)/UserDetails";

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<Login />} />
          <Route path="/location" element={<Location />} />
          {/* <Route path="/food" element={<Food />} /> */}
          <Route path="/register-store" element={<StoreRegisterPage />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/change-passsword" element={<ChangePasswordPage />} />
          <Route path="/forgot-password" element={<ForgotPasswordPage />} />
          <Route path="/reset-password" element={<ResetPasswordPage />} />
          <Route path="/food-details/:id" element={<FoodDetails />} />
          <Route path="/cart" element={<CartPage />} />
          <Route path="/store-profile/:id" element={<StoreProfilePage />} />

          <Route path="/store/detail/:id" element={<StoreDetailPage />} />
          <Route path="/post" element={<Post />} />
          <Route path="/post-details/:postId" element={<DetailPost />} />
          <Route path="/store/comment/:id" element={<StoreCommentPage />} />
          <Route path="/my-order" element={<MyOrder />} />

          <Route path="/food-list" element={<FoodList />} />
          <Route path="/store-list" element={<StoreList />} />
          <Route path="/wishlist" element={<Wishlist />} />
          <Route path="/flash-sale/add" element={<AddFlashSale />} />
          <Route path="/flash-sale" element={<FlashSale />} />
          <Route path="/user-detail" element={<UserDetails />} />
        </Route>
        <Route element={<ShipperLayout></ShipperLayout>}>
          <Route path="/shipper/details/:id" element={<ShipperDetailsPage />} />
          <Route
            path="/shipper/order-available"
            element={<OrderAvailablePage />}
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
          <Route path="/store/food" element={<Food />} />
          <Route path="/store/inventory" element={<Inventory />} />
          <Route path="/store/category" element={<Category />} />
          <Route path="/store/discount" element={<Discount />} />
        </Route>
        <Route element={<AdminLayout></AdminLayout>}>
          <Route path="/admin/report" element={<ReportPage />} />
          <Route path="/admin/dashboard" element={<DashboardPage />} />
          <Route path="/admin/manage-acoount" element={<AccountManagePage />} />
          <Route
            path="/admin/request-account"
            element={<RequestAccountPage />}
          />
        </Route>
      </Routes>
    </>
  );
}

export default App;
