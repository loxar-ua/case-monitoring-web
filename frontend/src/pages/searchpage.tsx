import { useEffect, useState } from "react";
import { useSearchParams, Link } from "react-router-dom";
import NewsPageLayout from "../layouts/newsPageLayout.tsx";
import type { ClusterReadDto } from "../types.ts";

const ITEMS_PER_PAGE = 12;

export default function SearchPage() {
  const [results, setResults] = useState<ClusterReadDto[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [searchParams, setSearchParams] = useSearchParams();

  const query = searchParams.get("query") || "";
  const page = parseInt(searchParams.get("page") || "1", 10);

  useEffect(() => {
    if (!query) {
      setResults([]);
      setError(null);
      return;
    }

    // AJAX: Fetch clusters with async/await
    const fetchSearchResults = async () => {
      setIsLoading(true);
      setError(null);

      try {
        const url = `/api/Cluster?name=${encodeURIComponent(query)}&pageNumber=${page}&pageSize=${ITEMS_PER_PAGE}`;
        const response = await fetch(url);

        // Error handling: Check HTTP status
        if (!response.ok) {
          throw new Error(`HTTP Error: ${response.status}`);
        }

        // Parse JSON response
        const data: { items: ClusterReadDto[]; totalPages: number } = await response.json();
        setResults(data.items);
        setTotalPages(data.totalPages);
      } catch (err) {
        const errorMessage = err instanceof Error ? err.message : "Невідома помилка";
        setError(`Помилка при пошуку: ${errorMessage}`);
        console.error("Search error:", err);
        setResults([]);
      } finally {
        setIsLoading(false);
      }
    };

    fetchSearchResults();
  }, [query, page]);

  const changePage = (newPage: number) => {
    setSearchParams({ query, page: String(newPage) });
  };

  return (
    <div className="search-page">
      <h4><Link to="/" className="back-home">
        ← На головну
      </Link></h4>
      <h3>Результати пошуку для: {query}</h3>

      {/* Loading state */}
      {isLoading && (
        <div style={{ textAlign: 'center', padding: '20px', color: '#666' }}>
          Завантажуємо результати...
        </div>
      )}

      {/* Error state */}
      {error && (
        <div style={{
          padding: '15px',
          marginBottom: '20px',
          background: '#ffebee',
          color: '#d32f2f',
          borderRadius: '4px',
          border: '1px solid #ef5350',
        }}>
          ⚠️ {error}
        </div>
      )}

      {/* Results */}
      {!isLoading && !error && (
        <NewsPageLayout
          news={results}
          page={page}
          totalPages={totalPages}
          changePage={changePage}
        />
      )}
    </div>
  );
}