import { Route, Routes } from "react-router-dom";
import Login from "./app/(public)/Login";
import Layout from "./app/(auth)/Layout";
import Location from "./app/(public)/Location";
import Sidebar from "./app/(public)/Sidebar";

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Layout />} >
          <Route path="/" element={<>Home Page</>} />
          <Route path="/Login" element={<Login />} />
          <Route path="/location" element={<Location />} />
          <Route path="/profile" element={<Sidebar />} />
        </Route>
      </Routes>
    </>
  )
}

export default App;
