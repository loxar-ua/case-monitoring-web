import { useEffect, useState } from "react";
import { useSearchParams, Link } from "react-router-dom";
import NewsPageLayout from "../layouts/newsPageLayout.tsx";
import type { ClusterReadDto } from "../types.ts";

const ITEMS_PER_PAGE = 12;

export default function SearchPage() {
  const [results, setResults] = useState<ClusterReadDto[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [searchParams, setSearchParams] = useSearchParams();

  const query = searchParams.get("query") || "";
  const page = parseInt(searchParams.get("page") || "1", 10);

  useEffect(() => {
    if (!query) return;

    fetch(
      `/api/Cluster?name=${encodeURIComponent(query)}&pageNumber=${page}&pageSize=${ITEMS_PER_PAGE}`
    )
      .then((res) => res.json())
      .then((data: { items: ClusterReadDto[]; totalPages: number }) => {
        setResults(data.items);
        setTotalPages(data.totalPages);
      })
      .catch((err) => console.error("Помилка пошуку:", err));
  }, [query, page]);

  const changePage = (newPage: number) => {
    setSearchParams({ query, page: String(newPage) });
  };

  return (
    <div className="search-page">
      <h4><Link to="/" className="back-home">
        ← На головну
      </Link></h4>
      <h3>Результати пошуку для: {query}</h3>

      <NewsPageLayout
        news={results}
        page={page}
        totalPages={totalPages}
        changePage={changePage}
      />
    </div>
  );
}