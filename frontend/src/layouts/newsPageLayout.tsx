import NewsList from "../components/homepage/clusterList.tsx";
import type { ClusterReadDto } from "../types.ts";

interface Props {
  news: ClusterReadDto[];
  page: number;
  totalPages: number;
  changePage: (newPage: number) => void;
}

export default function NewsPageLayout({ news, page, totalPages, changePage }: Props) {
  return (
    <div className="homepage">
      <NewsList news={news} />

      {/* pagination */}
      <section className="pagination">
        <button disabled={page === 1} onClick={() => changePage(page - 1)}>«</button>

        {page > 3 && (
          <>
            <button onClick={() => changePage(1)}>1</button>
            <span className="dots">...</span>
          </>
        )}

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
