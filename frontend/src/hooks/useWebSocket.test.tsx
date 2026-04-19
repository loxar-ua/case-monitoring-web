import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { renderHook, act } from '@testing-library/react';
import { useWebSocket } from './useWebSocket';

type WebSocketHandler = (() => void) | null;

class MockWebSocket {
  static instances: MockWebSocket[] = [];
  static OPEN = 1;
  static CONNECTING = 0;

  readyState = MockWebSocket.CONNECTING;
  sent: string[] = [];
  onopen: WebSocketHandler = null;
  onmessage: ((event: MessageEvent) => void) | null = null;
  onerror: WebSocketHandler = null;
  onclose: WebSocketHandler = null;

  constructor(public readonly url: string) {
    MockWebSocket.instances.push(this);
  }

  open() {
    this.readyState = MockWebSocket.OPEN;
    this.onopen?.();
  }

  message(data: unknown) {
    this.onmessage?.({ data } as MessageEvent);
  }

  close() {
    this.readyState = 3;
    this.onclose?.();
  }

  send(value: string) {
    this.sent.push(value);
  }
}

describe('useWebSocket', () => {
  beforeEach(() => {
    MockWebSocket.instances = [];
    vi.stubGlobal('WebSocket', MockWebSocket as unknown as typeof WebSocket);
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('connects and parses incoming JSON messages', () => {
    const { result } = renderHook(() => useWebSocket<{ title: string }>('ws://example.test'));

    expect(result.current.status).toBe('connecting');
    expect(MockWebSocket.instances).toHaveLength(1);

    act(() => {
      MockWebSocket.instances[0].open();
      MockWebSocket.instances[0].message('{"title":"Hello"}');
    });

    expect(result.current.status).toBe('open');
    expect(result.current.lastMessage).toEqual({ title: 'Hello' });
  });

  it('sends messages only when socket is open', () => {
    const { result } = renderHook(() => useWebSocket('ws://example.test'));

    act(() => {
      MockWebSocket.instances[0].open();
    });

    act(() => {
      result.current.sendMessage({ clusterId: 7 });
    });

    expect(MockWebSocket.instances[0].sent).toEqual(['{"clusterId":7}']);
  });

  it('marks closed sockets and schedules reconnect', () => {
    vi.useFakeTimers();
    const { result } = renderHook(() => useWebSocket('ws://example.test', { reconnectInterval: 1000, maxReconnectAttempts: 1 }));

    act(() => {
      MockWebSocket.instances[0].close();
    });

    expect(result.current.status).toBe('closed');

    act(() => {
      vi.advanceTimersByTime(1000);
    });

    expect(MockWebSocket.instances).toHaveLength(2);
  });
});
