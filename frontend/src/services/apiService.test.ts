import { describe, expect, it, vi } from 'vitest';
import { fetchCategories, fetchClusters } from './apiService';

describe('apiService', () => {
  it('fetchClusters builds query string and returns parsed data', async () => {
    const fetchMock = vi.fn().mockResolvedValue({
      ok: true,
      json: vi.fn().mockResolvedValue({
        items: [{ id: 1, name: 'Cluster 1' }],
        totalPages: 3,
      }),
    });
    vi.stubGlobal('fetch', fetchMock);

    const result = await fetchClusters({
      page: 2,
      pageSize: 12,
      sortBy: 'popular',
      categoryId: '5',
    });

    expect(fetchMock).toHaveBeenCalledWith(
      '/api/Cluster?pageNumber=2&pageSize=12&sortBy=popular&categoryId=5',
      expect.objectContaining({
        method: 'GET',
        headers: { Accept: 'application/json' },
      }),
    );
    expect(result.totalPages).toBe(3);
    expect(result.items).toHaveLength(1);
  });

  it('fetchClusters throws on non-ok response', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: false,
      status: 500,
      statusText: 'Server Error',
      json: vi.fn(),
    }));

    await expect(
      fetchClusters({ page: 1, pageSize: 10 }),
    ).rejects.toThrow('Failed to fetch clusters');
  });

  it('fetchCategories returns categories', async () => {
    const fetchMock = vi.fn().mockResolvedValue({
      ok: true,
      json: vi.fn().mockResolvedValue([
        { id: 1, name: 'Політика' },
        { id: 2, name: 'Корупція' },
      ]),
    });
    vi.stubGlobal('fetch', fetchMock);

    const result = await fetchCategories();

    expect(fetchMock).toHaveBeenCalledWith(
      '/api/Category',
      expect.objectContaining({ method: 'GET' }),
    );
    expect(result).toHaveLength(2);
    expect(result[0].name).toBe('Політика');
  });

  it('fetchCategories throws on invalid payload shape', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: true,
      json: vi.fn().mockResolvedValue({ items: [] }),
    }));

    await expect(fetchCategories()).rejects.toThrow('Invalid response shape');
  });
});
