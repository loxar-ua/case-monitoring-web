import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import Start from "../components/homepage/start.tsx"; 
import NewsPageLayout from "../layouts/newsPageLayout.tsx";
import { fetchClusters } from "../services/apiService.ts";
import type { ClusterReadDto } from "../types.ts";
import "./homepage.css";

const ITEMS_PER_PAGE = 12;

export default function HomePage() {
  const [allNews, setAllNews] = useState<ClusterReadDto[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [searchParams, setSearchParams] = useSearchParams();
  const initialPage = parseInt(searchParams.get("page") || "1", 10);
  const [page, setPage] = useState(initialPage);

  useEffect(() => {
    const controller = new AbortController();

    async function loadClusters() {
      try {
        const data = await fetchClusters(page, ITEMS_PER_PAGE, controller.signal);
        setAllNews(data.items);
        setTotalPages(data.totalPages);
      } catch (error) {
        if (error instanceof DOMException && error.name === "AbortError") {
          return;
        }

        console.error("Помилка завантаження новин:", error);
      }
    }

    loadClusters();

    return () => controller.abort();
  }, [page]);

  const changePage = (newPage: number) => {
    setPage(newPage);
    setSearchParams({ page: String(newPage) });
  };

  return (
    <>
      <Start /> {/* Hero секція */}
      <NewsPageLayout
        news={allNews}
        page={page}
        totalPages={totalPages}
        changePage={changePage}
      />
    </>
  );
}
