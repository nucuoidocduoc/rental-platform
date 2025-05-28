import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    proxy: {
      "/api/properties": {
        target: "http://inventory-service:8080",
        changeOrigin: true,
        rewrite: (path) => {
          console.log("Proxying:", path); // Debug proxy
          return path.replace(/^\/properties/, "/properties");
        },
      },
      "/api/booking": {
        target: "http://booking-service:8080",
        changeOrigin: true,
        rewrite: (path) => {
          console.log("Proxying:", path); // Debug proxy
          return path.replace(/^\/booking/, "/booking");
        },
      },
      "/api/payments": {
        target: "http://payment-service:8080",
        changeOrigin: true,
        rewrite: (path) => {
          console.log("Proxying:", path); // Debug proxy
          return path.replace(/^\/payments/, "/payments");
        },
      },
    },
  },
  plugins: [react()],
});
