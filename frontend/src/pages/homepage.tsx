import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import NewsList from "../components/homepage/clusterList.tsx";
import Start from "../components/homepage/start.tsx"; 
import "./homepage.css";
import type { ClusterReadDto } from "../types.ts";

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

      data.items.forEach(cluster => {
        fetch(`/api/Cluster/${cluster.id}`, { method: "PATCH" }).catch(() => {});
      });
    })
    .catch(err => console.error("Помилка завантаження новин:", err));
}, [page]);

  const changePage = (newPage: number) => {
    setPage(newPage);
    setSearchParams({ page: String(newPage) });
  };

  return (
    <div className="homepage">
      <Start />
      <NewsList news={allNews} />

      {/* pagination */}
      <section className="pagination">
        <button disabled={page === 1} onClick={() => changePage(page - 1)}>«</button>

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

        <button disabled={page === totalPages} onClick={() => changePage(page + 1)}>»</button>
      </section>
    </div>
  );
}
