// // components/articleGrid.tsx
// import type { ArticleReadDto } from "../../types";
// import ArticleCard from "./articleCard";
// import "./articleGrid.css";

// type Props = {
//   articles: ArticleReadDto[];
// };

// export default function ArticleGrid({ articles }: Props) {
//   const groupedByYear = groupByYear(articles);

//   return (
//     <div className="timeline-list">
//       {Object.entries(groupedByYear).map(([year, items]) => (
//         <div key={year}>
//           <h2>{year}</h2>
//           {groupByDay(items).map(([date, dayArticles]) => (
//             <div key={date} className="timeline-card">
//               <span className="timeline-date">{date}</span>
//               {dayArticles.map(article => (
//                 <ArticleCard key={article.id} item={article} />
//               ))}
//             </div>
//           ))}
//         </div>
//       ))}
//     </div>
//   );
// }

// function groupByYear(articles: ArticleReadDto[]) {
//   return articles.reduce((acc, article) => {
//     const year = new Date(article.publishedAt).getFullYear();
//     (acc[year] ||= []).push(article);
//     return acc;
//   }, {} as Record<number, ArticleReadDto[]>);
// }

// function groupByDay(articles: ArticleReadDto[]) {
//   const map = new Map<string, ArticleReadDto[]>();
//   articles.forEach(article => {
//     const date = new Date(article.publishedAt).toLocaleDateString("uk-UA", {
//       day: "numeric",
//       month: "long",
//     });
//     (map.get(date) ?? map.set(date, []).get(date))!.push(article);
//   });
//   return Array.from(map.entries());
// }

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
        .sort(([a], [b]) => Number(b) - Number(a))
        .map(([year, items]) => (
          <div key={year}>
            <h2>{year}</h2>
            {groupByDay(items).map(([date, dayArticles]) => (
              <div key={date} className="timeline-card">
                <span className="timeline-date">{date}</span>
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
            ))}
          </div>
        ))}
    </div>
  );
}

function groupByYear(articles: ArticleReadDto[]) {
  return articles.reduce((acc, article) => {
    const year = new Date(article.publishedAt).getFullYear();
    (acc[year] ||= []).push(article);
    return acc;
  }, {} as Record<number, ArticleReadDto[]>);
}

function groupByDay(articles: ArticleReadDto[]) {
  const map = new Map<string, ArticleReadDto[]>();
  articles.forEach(article => {
    const date = new Date(article.publishedAt).toLocaleDateString("uk-UA", {
      day: "numeric",
      month: "long",
    });
    (map.get(date) ?? map.set(date, []).get(date))!.push(article);
  });

  return Array.from(map.entries()).sort(
    ([dateA], [dateB]) =>
      new Date(dateB).getTime() - new Date(dateA).getTime()
  );
}
