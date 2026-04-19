import type { ClusterReadDto } from "../types.ts";

export type ClusterListResponse = {
  items: ClusterReadDto[];
  totalPages: number;
};

export async function fetchClusters(
  page: number,
  pageSize: number,
  signal?: AbortSignal,
): Promise<ClusterListResponse> {
  const url = `/api/Cluster?pageNumber=${encodeURIComponent(page)}&pageSize=${encodeURIComponent(pageSize)}`;

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
