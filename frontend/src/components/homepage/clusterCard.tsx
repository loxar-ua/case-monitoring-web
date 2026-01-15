import type { ClusterReadDto } from "../../types.ts";
import "./clusterCard.css";

type Props = {
  item: ClusterReadDto;
};

export default function NewsCard({ item }: Props) {
  return (
    <a href={`/cluster/${item.id}`} className="cluster-card">
      <img src={item.featuredImageURL} alt={item.name} className="cluster-image" />
      <div className="cluster-content">
        <h2 className="cluster-title">{item.name}</h2>
        {item.summary && (
  <div
    className="cluster-text"
    dangerouslySetInnerHTML={{ __html: item.summary }}
  />
)}
        <span className="cluster-updated">
          Останнє оновлення: {item.lastUpdatedAt}
        </span>
      </div>
    </a> 
  );
}
