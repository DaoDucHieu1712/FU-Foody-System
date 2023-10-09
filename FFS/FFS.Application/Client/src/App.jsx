import { Route, Routes } from "react-router-dom";
import Location from "./app/(public)/Location";

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<>Home Page</>} />
        <Route path="/location" element={<Location />} />
      </Routes>
    </>
  );
}

export default App;
