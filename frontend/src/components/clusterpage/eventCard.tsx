import { useState } from "react";
import type { EventReadDto, EventWithArticlesReadDto, ArticleReadDto } from "../../types";
import ArticleGrid from "./articleGrid"; 
import "./eventCard.css";

interface Props {
  event: EventReadDto;
}

export default function EventCard({ event }: Props) {
  const [articles, setArticles] = useState<ArticleReadDto[] | null>(null);
  const [loading, setLoading] = useState(false);
  const [isOpen, setIsOpen] = useState(false);

  const formatDate = (dateString?: string) => {
    if (!dateString) return "";
    try {
      const date = new Date(dateString);
      if (isNaN(date.getTime())) return dateString; 

      return date.toLocaleDateString("uk-UA", {
        day: "numeric",
        month: "long",
        year: "numeric",
      });
    } catch (e) {
      return dateString;
    }
  };

  const rawDate = event.date || event.Date;
  const formattedDate = formatDate(rawDate);

  const toggleArticles = async () => {
    if (isOpen) {
      setIsOpen(false);
      return;
    }

    if (!articles) {
      setLoading(true);
      try {
        const res = await fetch(`/api/Event/${event.id}`);
        if (!res.ok) throw new Error("Помилка завантаження статей");
        
        const data: EventWithArticlesReadDto = await res.json();
        setArticles(data.articles);
      } catch (err) {
        console.error("Fetch error:", err);
      } finally {
        setLoading(false);
      }
    }
    setIsOpen(true);
  };

  return (
    <div className="event-card">
      <h3>{event.title}</h3>
      <span className="event-date">{formattedDate}</span>
      <p>{event.description}</p>

      <div style={{ margin: "1rem" }}>
        <button 
          className="statti" 
          onClick={toggleArticles} 
          disabled={loading}
        >
          {loading ? "Завантаження..." : isOpen ? "Сховати статті" : "Статті"}
        </button>
      </div>

      {isOpen && articles && (
        <div className="articles-dropdown" style={{ marginTop: "1rem" }}>
          {articles.length > 0 ? (
            <ArticleGrid articles={articles} />
          ) : (
            <p style={{ marginLeft: "1rem", fontStyle: "italic" }}>
              Статей для цієї події поки не знайдено.
            </p>
          )}
        </div>
      )}
    </div>
  );
}