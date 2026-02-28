// src/components/clusterpage/EventCard.tsx
import ArticleGrid from "./articleGrid";
import type { EventWithArticlesReadDto } from "../../types";
import "./eventCard.css";

type Props = {
  event: EventWithArticlesReadDto;
  isOpen: boolean;
  toggle: (id: number) => void;
};

export default function EventCard({ event, isOpen, toggle }: Props) {
  return (
    <section className="event-card">
      <h3>{event.title}</h3>
      {event.eventTime && (
        <p className="event-date">
          {new Date(event.eventTime).toLocaleString("uk-UA", {
            dateStyle: "medium",
            timeStyle: "short",
          })}
        </p>
      )}
      {event.description && <h5 className="event-description">{event.description}</h5>}
      {event.articles && event.articles.length > 0 && (
        <>
          <p>
            <button className="statti" onClick={() => toggle(event.id)}>
              Статті {isOpen ? "▲" : "▼"}
            </button>
          </p>
          {isOpen && <ArticleGrid articles={event.articles} />}
        </>
      )}
    </section>
  );
}
