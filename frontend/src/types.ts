// src/types.ts
export type ClusterReadDto = {
  id: number;
  name: string;
  summary?: string;
  featuredImageURL?: string;
  lastUpdatedAt?: string;
};

export type MediaReadDto = {
  name: string;
};

export type ArticleReadDto = {
  id: number;
  media: MediaReadDto;
  title: string;
  link: string;
  featuredImageURL: string;
  author: string;
  content: string;
  publishedAt: string;
  isChecked: boolean;
};

export type ClusterDetailedReadDto = {
  id: number;
  name: string;
  viewCounter: number;
  summary?: string;
  featuredImageURL?: string;
  events: EventWithArticlesReadDto[];
};

export type EventReadDto = { 
  id: number; 
  title: string; 
  description?: string; 
  eventTime?: string;  
}; 

export type EventWithArticlesReadDto = { 
  id: number; 
  title: string; 
  description?: string; 
  eventTime?: string; 
  articles: ArticleReadDto[]; 
};