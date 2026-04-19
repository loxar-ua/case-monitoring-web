import type { CategoryReadDto, ClusterReadDto } from "../types.ts";

export type ClusterListResponse = {
  items: ClusterReadDto[];
  totalPages: number;
};

export type FetchClustersOptions = {
  page: number;
  pageSize: number;
  sortBy?: string;
  categoryId?: string;
  signal?: AbortSignal;
};

export async function fetchClusters({
  page,
  pageSize,
  sortBy,
  categoryId,
  signal,
}: FetchClustersOptions): Promise<ClusterListResponse> {
  const params = new URLSearchParams();
  params.set("pageNumber", String(page));
  params.set("pageSize", String(pageSize));

  if (sortBy) {
    params.set("sortBy", sortBy);
  }

  if (categoryId) {
    params.set("categoryId", categoryId);
  }

  const url = `/api/Cluster?${params.toString()}`;

  try {
    const response = await fetch(url, {
      method: "GET",
      headers: {
        "Accept": "application/json",
      },
      signal,
    });

    if (!response.ok) {
      throw new Error(`Failed to fetch clusters: ${response.status} ${response.statusText}`);
    }

    const data = (await response.json()) as ClusterListResponse | null;

    if (
      !data ||
      !Array.isArray(data.items) ||
      typeof data.totalPages !== "number"
    ) {
      throw new Error("Invalid response shape from /api/Cluster");
    }

    return data;
  } catch (error) {
    console.error("Error fetching cluster list:", error);
    throw error;
  }
}

export async function fetchCategories(
  signal?: AbortSignal,
): Promise<CategoryReadDto[]> {
  try {
    const response = await fetch("/api/Category", {
      method: "GET",
      headers: {
        "Accept": "application/json",
      },
      signal,
    });

    if (!response.ok) {
      throw new Error(`Failed to fetch categories: ${response.status} ${response.statusText}`);
    }

    const data = (await response.json()) as CategoryReadDto[] | null;

    if (!data || !Array.isArray(data)) {
      throw new Error("Invalid response shape from /api/Category");
    }

    return data;
  } catch (error) {
    console.error("Error fetching categories:", error);
    throw error;
  }
}
