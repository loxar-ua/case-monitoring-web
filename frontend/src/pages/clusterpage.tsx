// src/pages/ClusterPage.tsx
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ClusterDescription from "../components/clusterpage/clusterDescription";
import ArticleGrid from "../components/clusterpage/articleGrid";
import type { ClusterDetailedReadDto } from "../types";

export default function ClusterPage() {
  const { id } = useParams<{ id: string }>();
  const [cluster, setCluster] = useState<ClusterDetailedReadDto | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!id) return;

    const loadCluster = async () => {
      try {
        const res = await fetch(`/api/Cluster/${id}`);
        if (!res.ok) throw new Error(`HTTP ${res.status}`);

        const data: ClusterDetailedReadDto = await res.json();
        setCluster(data);
      } catch (err) {
        console.error("Помилка завантаження:", err);
        setCluster(null);
      } finally {
        setLoading(false);
      }
    };

    loadCluster();
  }, [id]);

  if (loading) return <h4>Завантаження…</h4>;
  if (!cluster) return <h4>Справу не знайдено</h4>;

  return (
    <div className="cluster-page">
      <ClusterDescription
        title={cluster.name}
        views={cluster.viewCounter}
        image={cluster.featuredImageURL}
        summary={cluster.summary}
      />
      <ArticleGrid articles={cluster.articles} />
    </div>
  );
}
