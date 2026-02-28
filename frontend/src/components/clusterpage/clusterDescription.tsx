import "./clusterDescription.css";
import type { EventWithArticlesReadDto } from "../../types";
import EventCard from "./eventCard";
import { useState } from "react";

type Props = {
  title: string;
  views: number;
  image?: string;
  events?: EventWithArticlesReadDto[];
};

export default function ClusterDescription({ title, views, image, events }: Props) {
  const [openEvents, setOpenEvents] = useState<Record<number, boolean>>({}); 
  
  const toggleArticles = (eventId: number) => { 
    setOpenEvents(prev => ({ 
      ...prev, 
      [eventId]: !prev[eventId], 
    })); 
  };

  return (
    <section className="cluster-description">
      <h1 className="cluster-title">{title}</h1>
      <span className="cluster-views">Відвідано: {views}</span>

      {image && (
        <div className="cluster-image-wrapper">
          <img src={image} alt={title} className="cluster-image" />
        </div>
      )}

      {events && events.length > 0 && (
        <div className="cluster-events-block">
          {events.map(event => (
            <EventCard
              key={event.id}
              event={event}
              isOpen={openEvents[event.id] || false}
              toggle={toggleArticles}
            />
          ))}
        </div>
      )} 
    </section>
  );
}
