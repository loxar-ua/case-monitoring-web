// import type { ArticleReadDto } from "../../types.ts";

// type Props = { item: ArticleReadDto };

// export default function ArticleCard({ item }: Props) {
//   return (
//     <a
//       href={item.link}                // зовнішнє посилання з DTO
//       className="article-card"
//       target="_blank"                // відкривати у новій вкладці
//       rel="noopener noreferrer"      // безпека
//     >
//       <img
//         src={item.featuredImageURL}
//         alt={item.title}
//         className="article-image"
//       />
//       <div className="article-content">
//         <h4 className="article-title">{item.title}</h4>
//         <p className="article-excerpt">{item.content.slice(0, 100)}...</p>
//         <div className="article-meta">
//           <span className="article-time">
//             {new Date(item.publishedAt).toLocaleTimeString("uk-UA", {
//               hour: "2-digit",
//               minute: "2-digit",
//             })}
//           </span>
//           <span className="article-source">{item.media?.name ?? "Джерело"}</span>
//           <span className="article-author">Автор: {item.author}</span>
//         </div>
//       </div>
//     </a>
//   );
// }

import type { ArticleReadDto } from "../../types.ts";

type Props = { item: ArticleReadDto };

export default function ArticleCard({ item }: Props) {
  return (
    <a
      href={item.link}
      className="article-card"
      target="_blank"
      rel="noopener noreferrer"
    >
      <img
        src={item.featuredImageURL}
        alt={item.title}
        className="article-image"
      />
      <div className="article-content">
        <h4 className="article-title">{item.title}</h4>
        <p className="article-excerpt">{item.content}</p>
        <div className="article-meta">
          <span className="article-source">{item.media?.name ?? "Джерело"}</span>
          <span className="article-author">Автор: {item.author}</span>
        </div>
      </div>
    </a>
  );
}
