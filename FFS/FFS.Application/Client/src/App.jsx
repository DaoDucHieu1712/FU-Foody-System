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
          <Route path="/inventory" element={<Inventory />} />
          <Route path="/store/detail/:id" element={<StoreDetailPage />} />
          <Route path="/post" element={<Post />} />
          <Route path="/post-details/:postId" element={<DetailPost />} />
          <Route path="/store/comment/:id" element={<StoreCommentPage />} />
          <Route path="/my-order" element={<MyOrder />} />
          <Route path="/discount" element={<Discount />} />
          <Route path="/food-list" element={<FoodList />} />
        </Route>
        <Route element={<StoreLayout></StoreLayout>}>
          <Route path="/food" element={<Food />} />
        </Route>
      </Routes>
    </>
  );
}

export default App;
