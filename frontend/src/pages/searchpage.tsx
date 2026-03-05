import { useEffect, useState } from "react";
import { useSearchParams, Link } from "react-router-dom";
import Filters from "../components/base/filters"; // Імпорт винесених фільтрів
import NewsPageLayout from "../layouts/newsPageLayout.tsx";
import type { ClusterReadDto, CategoryReadDto } from "../types.ts";

const ITEMS_PER_PAGE = 12;

export default function SearchPage() {
  const [results, setResults] = useState<ClusterReadDto[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [categories, setCategories] = useState<CategoryReadDto[]>([]);
  const [searchParams, setSearchParams] = useSearchParams();

  const query = searchParams.get("query") || "";
  const page = parseInt(searchParams.get("page") || "1", 10);
  const sortBy = searchParams.get("sortBy") || "";
  const categoryId = searchParams.get("categoryId") || "";

  useEffect(() => {
    fetch(`/api/Category`)
      .then((res) => res.json())
      .then((data: CategoryReadDto[]) => setCategories(data))
      .catch((err) => console.error("Помилка категорій:", err));
  }, []);

  useEffect(() => {
    if (!query) return;

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
    setSearchParams({ query, page: String(newPage), sortBy, categoryId });
  };

  const changeSort = (value: string) => {
    setSearchParams({ query, page: "1", sortBy: value, categoryId });
  };

  const changeCategory = (value: string) => {
    setSearchParams({ query, page: "1", sortBy, categoryId: value });
  };

  return (
    <div className="search-page">
      <h4>
        <Link to="/" className="back-home">← На головну</Link>
      </h4>
      
      <h3>Результати пошуку для: {query}</h3>

      <Filters 
        sortBy={sortBy}
        categoryId={categoryId}
        categories={categories}
        onSortChange={changeSort}
        onCategoryChange={changeCategory}
      />

      <NewsPageLayout
        news={results}
        page={page}
        totalPages={totalPages}
        changePage={changePage}
      />
    </div>
  );
}