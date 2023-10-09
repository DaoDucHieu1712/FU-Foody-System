import { Route, Routes } from "react-router-dom";
import Login from "./app/(public)/Login";
import Layout from "./app/(auth)/Layout";

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Layout />} >
          <Route path="/Login" element={<Login />} />
          <Route path="/" element={<>HomePage</>} />
        </Route>
      </Routes>
    </>
  );
}

export default App;
