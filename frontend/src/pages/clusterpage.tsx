// src/pages/ClusterPage.tsx
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ClusterDescription from "../components/clusterpage/clusterDescription";
import ArticleGrid from "../components/clusterpage/articleGrid";
import type { ClusterDetailedReadDto } from "../types";
import "./clusterpage.css";

export default function ClusterPage() {
  const { id } = useParams<{ id: string }>();
  const [cluster, setCluster] = useState<ClusterDetailedReadDto | null>(null);
  const [loading, setLoading] = useState(true);

  const [openEvents, setOpenEvents] = useState<Record<number, boolean>>({}); 
  
  const toggleArticles = (eventId: number) => { 
    setOpenEvents(prev => ({ 
      ...prev, 
      [eventId]: !prev[eventId], 
    })); 
  };

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
      />
      {cluster.events?.map(event => (
          <section key={event.id} className="event-block">
            <h3>{event.title}</h3>
            {event.eventTime && (
              <span className="event-date">
                {new Date(event.eventTime).toLocaleDateString("uk-UA")}
              </span>
            )}
            {event.description && <p>{event.description}</p>}
            {event.articles && event.articles.length > 0 && (
                <>
                  <p>
                    <button 
                    className="statti" 
                    onClick={() => toggleArticles(event.id)}
                    >
                    Статті {openEvents[event.id] ? "▲" : "▼"}
                  </button>
                  </p>
                  {openEvents[event.id] && ( 
                    <ArticleGrid articles={event.articles} /> 
                  )} 
                </> 
              )} 
            </section> 
          ))}
    </div>
  );
}
