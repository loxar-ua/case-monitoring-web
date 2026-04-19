import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import Start from "../components/homepage/start.tsx";
import Filters from "../components/base/filters";
import NewsPageLayout from "../layouts/newsPageLayout.tsx";
import { fetchCategories, fetchClusters } from "../services/apiService.ts";
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
    const controller = new AbortController();

    async function loadCategories() {
      try {
        const data = await fetchCategories(controller.signal);
        setCategories(data);
      } catch (error) {
        if (error instanceof DOMException && error.name === "AbortError") {
          return;
        }

        console.error("Помилка категорій:", error);
      }
    }

    loadCategories();

    return () => controller.abort();
  }, []);

  useEffect(() => {
    const controller = new AbortController();

    async function loadClusters() {
      try {
        const data = await fetchClusters({
          page,
          pageSize: ITEMS_PER_PAGE,
          sortBy,
          categoryId,
          signal: controller.signal,
        });

        setAllNews(data.items);
        setTotalPages(data.totalPages);
      } catch (error) {
        if (error instanceof DOMException && error.name === "AbortError") {
          return;
        }

        console.error("Помилка новин:", error);
      }
    }

    loadClusters();

    return () => controller.abort();
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