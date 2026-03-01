import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import Start from "../components/homepage/start.tsx"; 
import NewsPageLayout from "../layouts/newsPageLayout.tsx";
import type { ClusterReadDto } from "../types.ts";
import type { CategoryReadDto } from "../types.ts";
import "./homepage.css";

const ITEMS_PER_PAGE = 12;

export default function HomePage() {
  const [allNews, setAllNews] = useState<ClusterReadDto[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [searchParams, setSearchParams] = useSearchParams();
  const initialPage = parseInt(searchParams.get("page") || "1", 10);
  const [page, setPage] = useState(initialPage);
  const initialSort = searchParams.get("sortBy") || "";
  const [sortBy, setSortBy] = useState(initialSort);
  const initialCategory = searchParams.get("categoryId") || "";
  const [categoryId, setCategoryId] = useState(initialCategory);
  const [categories, setCategories] = useState<CategoryReadDto[]>([]);

  useEffect(() => {
    // load categories once
    fetch(`/api/Category`)
      .then((res) => res.json())
      .then((data: CategoryReadDto[]) => setCategories(data))
      .catch((err) => console.error("Помилка завантаження категорій:", err));

    const params = new URLSearchParams();
    params.set("pageNumber", String(page));
    params.set("pageSize", String(ITEMS_PER_PAGE));
    if (sortBy) params.set("sortBy", sortBy);
    if (categoryId) params.set("categoryId", categoryId);

    fetch(`/api/Cluster?${params.toString()}`)
      .then(res => res.json())
      .then((data: { items: ClusterReadDto[]; totalPages: number }) => {
        setAllNews(data.items);
        setTotalPages(data.totalPages);
      })
      .catch(err => console.error("Помилка завантаження новин:", err));
  }, [page, sortBy, categoryId]);

  const changePage = (newPage: number) => {
    setPage(newPage);
    setSearchParams({ page: String(newPage), ...(sortBy ? { sortBy } : {}) });
  };

  const changeSort = (value: string) => {
    setSortBy(value);
    // reset to page 1 when sort changes
    setPage(1);
    setSearchParams({ page: "1", ...(value ? { sortBy: value } : {}), ...(categoryId ? { categoryId } : {}) });
  };

  const changeCategory = (value: string) => {
    setCategoryId(value);
    setPage(1);
    setSearchParams({ page: "1", ...(sortBy ? { sortBy } : {}), ...(value ? { categoryId: value } : {}) });
  };

  return (
    <>
      <Start /> {/* Hero секція */}
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
        news={allNews}
        page={page}
        totalPages={totalPages}
        changePage={changePage}
      />
    </>
  );
}
