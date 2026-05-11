import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import svgr from "vite-plugin-svgr";

export default defineConfig({
  plugins: [react(), svgr()],
  server: {
    proxy: {
      "/api": {
        target: "http://localhost:5261",
        changeOrigin: true,
        secure: false,
      },
      "/hubs": {
        target: "http://localhost:5261",
        changeOrigin: true,
        secure: false,
        ws: true,
      },
    },
  },
});
