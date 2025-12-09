import ClusterCard from "./clusterCard.tsx";
import "./clusterCard.css";
import type { ClusterReadDto } from "../../types.ts";

type Props = {
  news: ClusterReadDto[];
};

export default function ClusterList({ news }: Props) {
  return (
    <div className="cluster-list">
      {news.map((item, index) => (
        <ClusterCard key={index} item={item} />
      ))}
    </div>
  );
}
