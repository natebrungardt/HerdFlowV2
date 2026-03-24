import { BrowserRouter, Routes, Route } from "react-router-dom";
import AllCowPage from "./pages/AllCowPage";
import CowDetailPage from "./pages/CowDetailPage";
import Navbar from "./components/Navbar";
import Dashboard from "./pages/Dashboard";

function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/cows" element={<AllCowPage />} />
        <Route path="/cows/:id" element={<CowDetailPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
