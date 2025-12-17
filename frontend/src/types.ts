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
  publishedAt: string; // ISO string з бекенду
  isChecked: boolean;
};

export type ClusterDetailedReadDto = {
  id: number;
  name: string;
  viewCounter: number;
  summary?: string;
  featuredImageURL?: string;
  articles: ArticleReadDto[];
};