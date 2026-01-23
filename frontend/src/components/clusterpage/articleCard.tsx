import type { ArticleReadDto } from "../../types";

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
          <span className="article-source">
            {item.media?.name ?? "Джерело"}
          </span>
           <span className="article-author">{item.author}</span>
        </div>
      </div>
    </a>
  );
}
