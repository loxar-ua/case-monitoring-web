import EventCard from "./eventCard";
import type { ClusterPresenceStatus, EventReadDto } from "../../types";
import "./clusterDescription.css";

interface ClusterDescriptionProps {
  title: string;
  views: number;
  image?: string;
  events: EventReadDto[];
  activeUsers: number;
  presenceStatus: ClusterPresenceStatus;
}

export default function ClusterDescription({
  title,
  views,
  image,
  events,
  activeUsers,
  presenceStatus,
}: ClusterDescriptionProps) {
  const presenceLabel =
    activeUsers === 1
      ? "1 особа переглядає цю справу зараз"
      : `${activeUsers} осіб переглядають цю справу зараз`;

  return (
    <section className="cluster-description">
      <h1 className="cluster-title">{title}</h1>
      <span className="cluster-views">Відвідано: {views}</span>

      <div className={`cluster-presence cluster-presence-${presenceStatus}`} aria-live="polite">
        <span className="cluster-presence-dot" />
        <span>{presenceLabel}</span>
      </div>

      {image && (
        <div>
          <img src={image} alt={title} className="cluster-image" />
        </div>
      )}

      <div className="events-list" style={{ marginTop: '32px' }}>
        {events && events.length > 0 ? (
          events.map((event) => (
            <EventCard key={event.id} event={event} />
          ))
        ) : (
          <p style={{ textAlign: 'center', color: 'var(--muted)', marginTop: '2rem' }}>
            Подій не знайдено
          </p>
        )}
      </div>
    </section>
  );
}