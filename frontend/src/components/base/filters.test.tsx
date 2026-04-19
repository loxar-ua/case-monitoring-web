import { describe, expect, it, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Filters from './filters';

const categories = [
  { id: 1, name: 'Політика' },
  { id: 2, name: 'Корупція' },
];

describe('Filters', () => {
  it('renders sort and category controls and calls handlers', async () => {
    const user = userEvent.setup();
    const onSortChange = vi.fn();
    const onCategoryChange = vi.fn();

    render(
      <Filters
        sortBy="popular"
        categoryId="2"
        categories={categories}
        onSortChange={onSortChange}
        onCategoryChange={onCategoryChange}
      />,
    );

    expect(screen.getByText('Сортування')).toBeInTheDocument();
    expect(screen.getByText('Категорії')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /^Популярні\s*→$/i })).toHaveClass('active');
    expect(screen.getByRole('button', { name: /^Корупція\s*→$/i })).toHaveClass('active');

    await user.click(screen.getByRole('button', { name: /^Нові\s*→$/i }));
    await user.click(screen.getByRole('button', { name: /^Політика\s*→$/i }));

    expect(onSortChange).toHaveBeenCalledWith('newest');
    expect(onCategoryChange).toHaveBeenCalledWith('1');
  });
});
