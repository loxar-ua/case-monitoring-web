import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import Start from "../components/homepage/start.tsx";
import Filters from "../components/base/filters";
import NewsPageLayout from "../layouts/newsPageLayout.tsx";
import type { ClusterReadDto, CategoryReadDto } from "../types.ts";
import "./homepage.css";

const ITEMS_PER_PAGE = 12;

export default function HomePage() {
  const [allNews, setAllNews] = useState<ClusterReadDto[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [categories, setCategories] = useState<CategoryReadDto[]>([]);
  const [searchParams, setSearchParams] = useSearchParams();

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
    const params = new URLSearchParams();
    params.set("pageNumber", String(page));
    params.set("pageSize", String(ITEMS_PER_PAGE));
    if (sortBy) params.set("sortBy", sortBy);
    if (categoryId) params.set("categoryId", categoryId);

    fetch(`/api/Cluster?${params.toString()}`)
      .then((res) => res.json())
      .then((data: { items: ClusterReadDto[]; totalPages: number }) => {
        setAllNews(data.items);
        setTotalPages(data.totalPages);
      })
      .catch((err) => console.error("Помилка новин:", err));
  }, [page, sortBy, categoryId]);

  const updateParams = (newParams: Record<string, string>) => {
    const current = Object.fromEntries(searchParams.entries());
    setSearchParams({ ...current, ...newParams });
  };

  const changePage = (newPage: number) => {
    updateParams({ page: String(newPage) });
  };

  const changeSort = (value: string) => {
    setSearchParams({ page: "1", sortBy: value, categoryId });
  };

  const changeCategory = (value: string) => {
    setSearchParams({ page: "1", sortBy, categoryId: value });
  };

  return (
    <>
      <Start />
      
      <Filters 
        sortBy={sortBy}
        categoryId={categoryId}
        categories={categories}
        onSortChange={changeSort}
        onCategoryChange={changeCategory}
      />

      <NewsPageLayout
        news={allNews}
        page={page}
        totalPages={totalPages}
        changePage={changePage}
      />
    </>
  );
}