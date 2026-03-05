import type { CategoryReadDto } from "../../types.ts";
import "./filters.css";

interface NewsFiltersProps {
  sortBy: string;
  categoryId: string;
  categories: CategoryReadDto[];
  onSortChange: (value: string) => void;
  onCategoryChange: (value: string) => void;
}

export default function Filters({
  sortBy,
  categoryId,
  categories,
  onSortChange,
  onCategoryChange,
}: NewsFiltersProps) {
  
  const sortOptions = [
    { value: "", label: "За замовчуванням" },
    { value: "popular", label: "Популярні" },
    { value: "unpopular", label: "Непопулярні" },
    { value: "newest", label: "Нові" },
    { value: "oldest", label: "Старі" },
  ];

  return (
    <div className="filters-root">
      <div className="filter-section">
        <h3 className="category-title">
          Сортування <span className="arrow-icon">→</span>
        </h3>
        <div className="category-tags-container">
          {sortOptions.map((option) => (
            <button
              key={option.value}
              className={`category-tag-btn ${sortBy === option.value ? "active" : ""}`}
              onClick={() => onSortChange(option.value)}
            >
              {option.label} <span className="tag-arrow-icon">→</span>
            </button>
          ))}
        </div>
      </div>

      <div className="filter-section" style={{ marginTop: "12px" }}>
        <h3 className="category-title">
          Категорії <span className="arrow-icon">→</span>
        </h3>
        <div className="category-tags-container">
          <button
            className={`category-tag-btn ${categoryId === "" ? "active" : ""}`}
            onClick={() => onCategoryChange("")}
          >
            Усі <span className="tag-arrow-icon">→</span>
          </button>

          {categories.map((c) => (
            <button
              key={c.id}
              className={`category-tag-btn ${categoryId === String(c.id) ? "active" : ""}`}
              onClick={() => onCategoryChange(String(c.id))}
            >
              {c.name} <span className="tag-arrow-icon">→</span>
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}