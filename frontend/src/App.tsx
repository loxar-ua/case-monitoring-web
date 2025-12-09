import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./layouts/layout";
import HomePage from "./pages/homepage";
import ClusterPage from "./pages/clusterpage";
import "./styles/global.css";

function App() {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/cluster/:id" element={<ClusterPage />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}

export default App;
