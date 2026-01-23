import "./header.css";
import LightTheme from "./../images/light_theme.svg?react";
import DarkTheme from "./../images/dark_theme.svg?react";
import { useTheme } from "../../context/themeContext";
import { useNavigate } from "react-router-dom"; 
import type { ChangeEvent } from "react";

export default function Header() {
  const { theme, toggleTheme } = useTheme();
  const ThemeIcon = theme === "light" ? LightTheme : DarkTheme;
  const navigate = useNavigate();

  const handleSearch = (e: ChangeEvent<HTMLInputElement>) => { const value = e.target.value; navigate(`/search?query=${encodeURIComponent(value)}`); };
  return (
    <header className="app-header" role="banner">
      <div className="header-inner">
        <img
          src="/src/components/images/logo.svg"
          alt="Шкандаль"
          className="header-logo"
        />

        <form
          className="search-form"
          role="search"
          aria-label="Site search"
          onSubmit={(e) => e.preventDefault()}
        >
          <input
            className="search-input"
            type="search"
            placeholder="Search"
            aria-label="Search"
            onChange={handleSearch}
          />
        </form>

        <button
          className="theme-toggle"
          onClick={toggleTheme}
          aria-pressed={theme === "dark"}
          title="Toggle theme"
        >
          <span className="visually-hidden">Toggle theme</span>
          <ThemeIcon className="theme-icon" width={20} height={20} />
        </button>
      </div>
    </header>
  );
}
