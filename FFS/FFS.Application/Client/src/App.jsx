import { Route, Routes } from "react-router-dom";
import Login from "./app/(public)/Login";
import Layout from "./app/(auth)/Layout";
import StoreRegisterPage from "./app/(public)/StoreRegisterPage";
import ChangePasswordPage from "./app/(auth)/ChangePasswordPage";
import ForgotPasswordPage from "./app/(public)/ForgotPasswordPage";
import Location from "./app/(auth)/Location";
import ProfilePage from "./app/(auth)/ProfilePage";
import ResetPasswordPage from "./app/(public)/ResetPassWord";

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path="/" element={<>Home Page</>} />
          <Route path="/login" element={<Login />} />
          <Route path="/location" element={<Location />} />
          <Route path="/register-store" element={<StoreRegisterPage />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/change-passsword" element={<ChangePasswordPage />} />
          <Route path="/forgot-password" element={<ForgotPasswordPage />} />
          <Route path="/reset-password" element={<ResetPasswordPage />} />
        </Route>
      </Routes>
    </>
  );
}

export default App;
