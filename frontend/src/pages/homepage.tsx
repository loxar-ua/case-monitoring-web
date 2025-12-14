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
        {Array.from({ length: totalPages }, (_, i) => (
          <button
            key={i + 1}
            className={page === i + 1 ? "active" : ""}
            onClick={() => setPage(i + 1)}
          >
            {i + 1}
          </button>
        ))}
      </section>
    </div>
  );
}
