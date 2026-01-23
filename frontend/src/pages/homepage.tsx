import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import Start from "../components/homepage/start.tsx"; 
import NewsPageLayout from "../layouts/newsPageLayout.tsx";
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
    fetch(`/api/Cluster?pageNumber=${page}&pageSize=${ITEMS_PER_PAGE}`)
      .then(res => res.json())
      .then((data: { items: ClusterReadDto[]; totalPages: number }) => {
        setAllNews(data.items);
        setTotalPages(data.totalPages);
      })
      .catch(err => console.error("Помилка завантаження новин:", err));
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
