import type { ArticleReadDto } from "../../types";
import ArticleCard from "./articleCard";
import "./articleGrid.css";

type Props = {
  articles: ArticleReadDto[];
};

export default function ArticleGrid({ articles }: Props) {
  const groupedByYear = groupByYear(articles);

  return (
    <div className="timeline-list">
      {Object.entries(groupedByYear)
        .sort(([a], [b]) => Number(b) - Number(a)) // роки від нових до старих
        .map(([year, items]) => (
          <div key={year}>
            <h2>{year}</h2>
            {groupByDay(items).map(({ key, label, dayArticles }) => (
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
    </div>
  );
}

/**
 * Групування по роках
 */
function groupByYear(articles: ArticleReadDto[]) {
  return articles.reduce((acc, article) => {
    const year = new Date(article.publishedAt).getFullYear();
    (acc[year] ||= []).push(article);
    return acc;
  }, {} as Record<number, ArticleReadDto[]>);
}

/**
 * Групування по днях:
 * - key: ISO-формат YYYY-MM-DD для сортування
 * - label: локалізований "DD місяць"
 */
function groupByDay(articles: ArticleReadDto[]) {
  const map = new Map<
    string,
    { label: string; items: ArticleReadDto[] }
  >();

  for (const article of articles) {
    const d = new Date(article.publishedAt);
    const key = d.toISOString().slice(0, 10); // YYYY-MM-DD
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
