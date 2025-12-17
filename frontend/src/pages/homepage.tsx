import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import NewsList from "../components/homepage/clusterList.tsx";
import Start from "../components/homepage/start.tsx"; 
import "./homepage.css";
import type { ClusterReadDto } from "../types.ts";

const ITEMS_PER_PAGE = 12;

export default function HomePage() {
  const [allNews, setAllNews] = useState<ClusterReadDto[]>([]);
  const [searchParams, setSearchParams] = useSearchParams();
  const initialPage = parseInt(searchParams.get("page") || "1", 10);
  const [page, setPage] = useState(initialPage);

  useEffect(() => {
    fetch("/news.json")
      .then((res) => res.json())
      .then((data: ClusterReadDto[]) => setAllNews(data))
      .catch((err) => console.error("Помилка завантаження новин:", err));
  }, []);

  const totalPages = Math.ceil(allNews.length / ITEMS_PER_PAGE);
  const paginatedNews = allNews.slice(
    (page - 1) * ITEMS_PER_PAGE,
    page * ITEMS_PER_PAGE
  );

  const changePage = (newPage: number) => {
    setPage(newPage);
    setSearchParams({ page: String(newPage) });
  };

  return (
    <div className="homepage">
      <Start/>
        <NewsList news={paginatedNews} />

      {/* pagination */}
      <section className="pagination">
        {/* back button */}
        <button disabled={page === 1} onClick={() => changePage(page - 1)}>
          «
        </button>

        {/* first page */}
        {page > 3 && (
          <>
            <button onClick={() => changePage(1)}>1</button>
            <span className="dots">...</span>
          </>
        )}

        {/* current page ±2 */}
        {Array.from({ length: totalPages }, (_, i) => i + 1)
          .filter(p => p >= page - 2 && p <= page + 2)
          .map(p => (
            <button
              key={p}
              className={page === p ? "active" : ""}
              onClick={() => changePage(p)}
            >
              {p}
            </button>
          ))}

        {/* last page */}
        {page < totalPages - 2 && (
          <>
            <span className="dots">...</span>
            <button onClick={() => changePage(totalPages)}>{totalPages}</button>
          </>
        )}

        {/* forward button */}
        <button disabled={page === totalPages} onClick={() => changePage(page + 1)}>
          »
        </button>
      </section>
    </div>
  );
}
