import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./layouts/layout";
import HomePage from "./pages/homepage";
import ClusterPage from "./pages/clusterpage";
import SearchPage from "./pages/searchpage";
import "./styles/global.css";
import "./styles/variables.css";
import { ThemeProvider } from "./context/themeProvider";

function App() {
  return (
    <ThemeProvider>
      <BrowserRouter>
        <Layout>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/cluster/:id" element={<ClusterPage />} />
            <Route path="/search" element={<SearchPage />} />
          </Routes>
        </Layout>
      </BrowserRouter>
    </ThemeProvider>
  );
}

export default App;
