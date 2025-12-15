// // src/context/ThemeContext.tsx
// import { createContext, useContext, useEffect, useMemo, useState } from "react";

// type Theme = "light" | "dark";
// type ThemeContextType = { theme: Theme; toggleTheme: () => void; setTheme: (t: Theme) => void };

// const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

// export function ThemeProvider({ children }: { children: React.ReactNode }) {
//   const [theme, setTheme] = useState<Theme>(() => {
//     const saved = localStorage.getItem("theme") as Theme | null;
//     if (saved) return saved;
//     const prefersDark = window.matchMedia?.("(prefers-color-scheme: dark)").matches;
//     return prefersDark ? "dark" : "light";
//   });

//   // Синхронізуємо клас на <html> і localStorage
//   useEffect(() => {
//     document.documentElement.classList.toggle("dark", theme === "dark");
//     localStorage.setItem("theme", theme);
//   }, [theme]);

//   // Реакція на зміну системної теми (не обов’язково)
//   useEffect(() => {
//     const mq = window.matchMedia("(prefers-color-scheme: dark)");
//     const handler = (e: MediaQueryListEvent) => {
//       const saved = localStorage.getItem("theme");
//       if (!saved) setTheme(e.matches ? "dark" : "light");
//     };
//     mq.addEventListener("change", handler);
//     return () => mq.removeEventListener("change", handler);
//   }, []);

//   const toggleTheme = () => setTheme(theme === "light" ? "dark" : "light");

//   const value = useMemo(() => ({ theme, toggleTheme, setTheme }), [theme]);

//   return <ThemeContext.Provider value={value}>{children}</ThemeContext.Provider>;
// }

// export function useTheme() {
//   const ctx = useContext(ThemeContext);
//   if (!ctx) throw new Error("useTheme must be used within ThemeProvider");
//   return ctx;
// }

// src/context/ThemeContext.tsx
import { createContext, useContext, useEffect, useState } from "react";

type Theme = "light" | "dark";
type ThemeContextType = { theme: Theme; toggleTheme: () => void };

const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

export function ThemeProvider({ children }: { children: React.ReactNode }) {
  const [theme, setTheme] = useState<Theme>(
    (localStorage.getItem("theme") as Theme) || "light"
  );

  useEffect(() => {
    document.documentElement.classList.toggle("dark", theme === "dark");
    localStorage.setItem("theme", theme);
  }, [theme]);

  const toggleTheme = () => setTheme(theme === "light" ? "dark" : "light");

  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

export function useTheme() {
  const ctx = useContext(ThemeContext);
  if (!ctx) throw new Error("useTheme must be used within ThemeProvider");
  return ctx;
}
