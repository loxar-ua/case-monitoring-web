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
        //завантажуємо конкретний JSON-файл, наприклад cluster-1.json
        const res = await fetch(`/cluster-${id}.json`);
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

  if (loading) return <p>Завантаження…</p>;
  if (!cluster) return <p>Справу не знайдено</p>;

  return (
    <div className="cluster-page">
      <ClusterDescription
        title={cluster.name}
        views={cluster.viewCounter}
        image={cluster.featuredImageURL}
        description={cluster.content}
      />
      <ArticleGrid articles={cluster.articles} />
    </div>
  );
}
