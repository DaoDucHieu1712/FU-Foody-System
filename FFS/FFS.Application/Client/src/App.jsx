import { Route, Routes } from "react-router-dom";
import Login from "./app/(public)/Login";
import Layout from "./app/(auth)/Layout";

import Location from "./app/(public)/Location";
import StoreRegisterPage from "./app/(public)/StoreRegisterPage";
import ChangePasswordPage from "./app/(public)/ChangePasswordPage";

import Location from "./app/(auth)/Location";


function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path="/" element={<>Home Page</>} />
          <Route path="/Login" element={<Login />} />
          <Route path="/location" element={<Location />} />
          <Route path="/register-store" element={<StoreRegisterPage />} />
          <Route path="/change-password" element={<ChangePasswordPage />} />
        </Route>
      </Routes>
    </>
  );
}

export default App;
