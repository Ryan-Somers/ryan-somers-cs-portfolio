import { Route, Routes } from "react-router-dom";

import IndexPage from "@/pages/index";
import BlogPage from "@/pages/blog";
import AboutPage from "@/pages/about";
import ProductsPage from "@/pages/products.tsx";
import ReportsPage from "@/pages/reports.tsx";

function App() {
  return (
    <Routes>
      <Route element={<IndexPage />} path="/" />
      <Route element={<ProductsPage />} path="/products" />
      <Route element={<ReportsPage />} path="/reports" />
      <Route element={<BlogPage />} path="/blog" />
      <Route element={<AboutPage />} path="/about" />
    </Routes>
  );
}

export default App;
