import { useState } from "react";
import type { ArticleReadDto } from "../../types";
import ArticleCard from "./articleCard";
import "./articleGrid.css";

type Props = {
  articles: ArticleReadDto[];
};

const DAYS_PER_PAGE = 2;

export default function ArticleGrid({ articles }: Props) {
  const [page, setPage] = useState(1);

  const groupedByYear = groupByYear(articles);

  const yearsWithDays = Object.entries(groupedByYear)
    .sort(([a], [b]) => Number(b) - Number(a))
    .map(([year, items]) => ({
      year,
      days: groupByDay(items),
    }));

  const allDays = yearsWithDays.flatMap(y =>
    y.days.map(d => ({ year: y.year, ...d }))
  );

  const totalPages = Math.ceil(allDays.length / DAYS_PER_PAGE);

  const paginatedDays = allDays.slice(
    (page - 1) * DAYS_PER_PAGE,
    page * DAYS_PER_PAGE
  );

  const changePage = (newPage: number) => {
    if (newPage >= 1 && newPage <= totalPages) {
      setPage(newPage);
    }
  };

  const daysByYear = paginatedDays.reduce((acc, day) => {
    (acc[day.year] ||= []).push(day);
    return acc;
  }, {} as Record<string, typeof paginatedDays>);

  return (
    <div className="timeline-list">
      {Object.entries(daysByYear).map(([year, days]) => (
        <div key={year}>
          <h2>{year}</h2>
          {days.map(({ key, label, dayArticles }) => (
            <div key={key}>
              <span className="timeline-date">{label}</span>
              <div className="timeline-day-row">
                {dayArticles
                  .sort(
                    (a, b) =>
                      new Date(b.publishedAt).getTime() -
                      new Date(a.publishedAt).getTime()
                  )
                  .map(article => (
                    <ArticleCard key={article.id} item={article} />
                  ))}
              </div>
            </div>
          ))}
        </div>
      ))}

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

/* group by year */
function groupByYear(articles: ArticleReadDto[]) {
  return articles.reduce((acc, article) => {
    const year = new Date(article.publishedAt).getFullYear();
    (acc[year] ||= []).push(article);
    return acc;
  }, {} as Record<number, ArticleReadDto[]>);
}

/* group by day */
function groupByDay(articles: ArticleReadDto[]) {
  const map = new Map<string, { label: string; items: ArticleReadDto[] }>();

  for (const article of articles) {
    const d = new Date(article.publishedAt);
    const key = d.toISOString().slice(0, 10);
    const label = d.toLocaleDateString("uk-UA", {
      day: "numeric",
      month: "long",
    });
    const bucket = map.get(key) ?? { label, items: [] };
    bucket.items.push(article);
    map.set(key, bucket);
  }

  return Array.from(map.entries())
    .sort(([ka], [kb]) => new Date(kb).getTime() - new Date(ka).getTime())
    .map(([key, { label, items }]) => ({
      key,
      label,
      dayArticles: items,
    }));
}
