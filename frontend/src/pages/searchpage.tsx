import { useEffect, useState } from "react";
import { useSearchParams, Link } from "react-router-dom";
import NewsPageLayout from "../layouts/newsPageLayout.tsx";
import type { ClusterReadDto, CategoryReadDto } from "../types.ts";

const ITEMS_PER_PAGE = 12;

export default function SearchPage() {
  const [results, setResults] = useState<ClusterReadDto[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [searchParams, setSearchParams] = useSearchParams();

  const query = searchParams.get("query") || "";
  const page = parseInt(searchParams.get("page") || "1", 10);
  const initialSort = searchParams.get("sortBy") || "";
  const [sortBy, setSortBy] = useState(initialSort);
  const initialCategory = searchParams.get("categoryId") || "";
  const [categoryId, setCategoryId] = useState(initialCategory);
  const [categories, setCategories] = useState<CategoryReadDto[]>([]);

  useEffect(() => {
    if (!query) return;

    // load categories once
    fetch(`/api/Category`)
      .then((res) => res.json())
      .then((data: CategoryReadDto[]) => setCategories(data))
      .catch((err) => console.error("Помилка завантаження категорій:", err));

    const params = new URLSearchParams();
    params.set("name", query);
    params.set("pageNumber", String(page));
    params.set("pageSize", String(ITEMS_PER_PAGE));
    if (sortBy) params.set("sortBy", sortBy);
    if (categoryId) params.set("categoryId", categoryId);

    fetch(`/api/Cluster?${params.toString()}`)
      .then((res) => res.json())
      .then((data: { items: ClusterReadDto[]; totalPages: number }) => {
        setResults(data.items);
        setTotalPages(data.totalPages);
      })
      .catch((err) => console.error("Помилка пошуку:", err));
  }, [query, page, sortBy, categoryId]);

  const changePage = (newPage: number) => {
    setSearchParams({ query, page: String(newPage), ...(sortBy ? { sortBy } : {}), ...(categoryId ? { categoryId } : {}) });
  };

  const changeSort = (value: string) => {
    setSortBy(value);
    setSearchParams({ query, page: "1", ...(value ? { sortBy: value } : {}), ...(categoryId ? { categoryId } : {}) });
  };

  const changeCategory = (value: string) => {
    setCategoryId(value);
    setSearchParams({ query, page: "1", ...(sortBy ? { sortBy } : {}), ...(value ? { categoryId: value } : {}) });
  };

  return (
    <div className="search-page">
      <h4><Link to="/" className="back-home">
        ← На головну
      </Link></h4>
      <h3>Результати пошуку для: {query}</h3>

      <div className="controls">
        <div className="sort-control">
          <label htmlFor="sortBy">Сортування:&nbsp;</label>
          <select id="sortBy" value={sortBy} onChange={(e) => changeSort(e.target.value)}>
            <option value="">За замовчуванням</option>
            <option value="popular">Популярні</option>
            <option value="unpopular">Непопулярні</option>
            <option value="newest">Нові</option>
            <option value="oldest">Старі</option>
          </select>
        </div>

        <div className="category-control">
          <label htmlFor="categoryId">Категорія:&nbsp;</label>
          <select id="categoryId" value={categoryId} onChange={(e) => changeCategory(e.target.value)}>
            <option value="">Усі</option>
            {categories.map((c) => (
              <option key={c.id} value={String(c.id)}>{c.name}</option>
            ))}
          </select>
        </div>
      </div>

      <NewsPageLayout
        news={results}
        page={page}
        totalPages={totalPages}
        changePage={changePage}
      />
    </div>
  );
}
