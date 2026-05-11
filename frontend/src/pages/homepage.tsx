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
  const [isLoadingClusters, setIsLoadingClusters] = useState(false);
  const [errorClusters, setErrorClusters] = useState<string | null>(null);
  const [searchParams, setSearchParams] = useSearchParams();

  const page = parseInt(searchParams.get("page") || "1", 10);
  const sortBy = searchParams.get("sortBy") || "";
  const categoryId = searchParams.get("categoryId") || "";

  // AJAX: Fetch categories on mount
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await fetch(`/api/Category`);
        if (!response.ok) throw new Error(`HTTP Error: ${response.status}`);
        const data: CategoryReadDto[] = await response.json();
        setCategories(data);
      } catch (err) {
        console.error("Categories error:", err);
        setCategories([]);
      }
    };
    fetchCategories();
  }, []);

  // AJAX: Fetch clusters when page/filter changes
  useEffect(() => {
    const fetchClusters = async () => {
      setIsLoadingClusters(true);
      setErrorClusters(null);

      try {
        const params = new URLSearchParams();
        params.set("pageNumber", String(page));
        params.set("pageSize", String(ITEMS_PER_PAGE));
        if (sortBy) params.set("sortBy", sortBy);
        if (categoryId) params.set("categoryId", categoryId);

        const response = await fetch(`/api/Cluster?${params.toString()}`);
        if (!response.ok) throw new Error(`HTTP Error: ${response.status}`);
        const data: { items: ClusterReadDto[]; totalPages: number } = await response.json();
        setAllNews(data.items);
        setTotalPages(data.totalPages);
      } catch (err) {
        const errorMessage = err instanceof Error ? err.message : "Невідома помилка";
        setErrorClusters(`Помилка при завантаженні: ${errorMessage}`);
        console.error("Clusters error:", err);
        setAllNews([]);
      } finally {
        setIsLoadingClusters(false);
      }
    };
    fetchClusters();
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

      {/* Loading indicator for clusters */}
      {isLoadingClusters && (
        <div style={{ textAlign: 'center', padding: '20px', color: '#666' }}>
          Завантажуємо справи...
        </div>
      )}

      {/* Error message for clusters */}
      {errorClusters && (
        <div style={{
          padding: '15px',
          marginBottom: '20px',
          background: '#ffebee',
          color: '#d32f2f',
          borderRadius: '4px',
          border: '1px solid #ef5350',
          marginLeft: '20px',
          marginRight: '20px',
        }}>
          ⚠️ {errorClusters}
        </div>
      )}
      
      <Filters 
        sortBy={sortBy}
        categoryId={categoryId}
        categories={categories}
        onSortChange={changeSort}
        onCategoryChange={changeCategory}
      />

      {/* Results or loading state */}
      {!errorClusters && (
        <NewsPageLayout
          news={allNews}
          page={page}
          totalPages={totalPages}
          changePage={changePage}
        />
      )}
    </>
  );
}