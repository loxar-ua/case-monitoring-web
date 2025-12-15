import { useEffect, useState } from "react";
import NewsList from "../components/homepage/clusterList.tsx";
import Start from "../components/homepage/start.tsx"; 
import "./homepage.css";
import type { ClusterReadDto } from "../types.ts";


const ITEMS_PER_PAGE = 12;

export default function HomePage() {
  const [allNews, setAllNews] = useState<ClusterReadDto[]>([]);
  const [page, setPage] = useState(1);

  useEffect(() => {
    fetch("/news.json")
      .then((res) => res.json())
      .then((data: ClusterReadDto[]) => {
        setAllNews(data);
      })
      .catch((err) => console.error("Помилка завантаження новин:", err));
  }, []);

  const totalPages = Math.ceil(allNews.length / ITEMS_PER_PAGE);
  const paginatedNews = allNews.slice(
    (page - 1) * ITEMS_PER_PAGE,
    page * ITEMS_PER_PAGE
  );

  return (
    <div className="homepage">
      <Start/>
      <NewsList news={paginatedNews} />

      {/* Пагінація */}
      <section className="pagination">
        {/* Кнопка "Назад" */}
        <button
          disabled={page === 1}
          onClick={() => setPage(page - 1)}
        >
          «
        </button>

        {/* Перша сторінка */}
        {page > 3 && (
          <>
            <button onClick={() => setPage(1)}>1</button>
            <span className="dots">...</span>
          </>
        )}

        {/* Поточна сторінка ±2 */}
        {Array.from({ length: totalPages }, (_, i) => i + 1)
          .filter(p => p >= page - 2 && p <= page + 2)
          .map(p => (
            <button
              key={p}
              className={page === p ? "active" : ""}
              onClick={() => setPage(p)}
            >
              {p}
            </button>
          ))}

        {/* Остання сторінка */}
        {page < totalPages - 2 && (
          <>
            <span className="dots">...</span>
            <button onClick={() => setPage(totalPages)}>{totalPages}</button>
          </>
        )}

        {/* Кнопка "Вперед" */}
        <button
          disabled={page === totalPages}
          onClick={() => setPage(page + 1)}
        >
          »
        </button>
      </section>

    </div>
  );
}
