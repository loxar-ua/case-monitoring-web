import { describe, expect, it, vi, beforeEach } from 'vitest';
import { render, screen } from '@testing-library/react';

const useWebSocketMock = vi.fn();

vi.mock('../hooks/useWebSocket.ts', () => ({
  useWebSocket: (...args: unknown[]) => useWebSocketMock(...args),
}));

import LiveAlerts from './LiveAlerts';

describe('LiveAlerts', () => {
  beforeEach(() => {
    useWebSocketMock.mockReset();
  });

  it('shows compact websocket status label', () => {
    useWebSocketMock.mockReturnValue({
      status: 'open',
      lastMessage: null,
      sendMessage: vi.fn(),
    });

    render(<LiveAlerts />);

    expect(screen.getByText('Live Alerts: Підключено')).toBeInTheDocument();
  });

  it('renders alert from websocket payload', () => {
    useWebSocketMock.mockReturnValue({
      status: 'open',
      lastMessage: {
        title: 'Політика',
        message: 'Кластер оновився.',
      },
      sendMessage: vi.fn(),
    });

    render(<LiveAlerts />);

    expect(screen.getByText('Політика')).toBeInTheDocument();
    expect(screen.getByText('Кластер оновився.')).toBeInTheDocument();
  });
});
