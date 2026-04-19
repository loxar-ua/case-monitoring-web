import { useCallback, useEffect, useRef, useState } from "react";

export type WebSocketStatus =
  | "connecting"
  | "open"
  | "closing"
  | "closed"
  | "error";

export function useWebSocket<T = unknown>(
  url: string,
  options?: {
    reconnectInterval?: number;
    maxReconnectAttempts?: number;
  },
) {
  const reconnectInterval = options?.reconnectInterval ?? 3000;
  const maxReconnectAttempts = options?.maxReconnectAttempts ?? 5;

  const wsRef = useRef<WebSocket | null>(null);
  const reconnectCountRef = useRef(0);
  const shouldReconnectRef = useRef(true);
  const reconnectTimerRef = useRef<number | null>(null);

  const [status, setStatus] = useState<WebSocketStatus>("connecting");
  const [lastMessage, setLastMessage] = useState<T | null>(null);

  const connect = useCallback(() => {
    if (!url) {
      setStatus("closed");
      return;
    }

    const socket = new WebSocket(url);
    wsRef.current = socket;
    setStatus("connecting");

    socket.onopen = () => {
      reconnectCountRef.current = 0;
      setStatus("open");
    };

    socket.onmessage = event => {
      try {
        setLastMessage(JSON.parse(event.data) as T);
      } catch (error) {
        console.warn("Unable to parse WebSocket message as JSON:", error);
      }
    };

    socket.onerror = () => {
      setStatus("error");
    };

    socket.onclose = () => {
      setStatus("closed");

      if (
        shouldReconnectRef.current &&
        reconnectCountRef.current < maxReconnectAttempts
      ) {
        reconnectCountRef.current += 1;
        reconnectTimerRef.current = window.setTimeout(
          () => connect(),
          reconnectInterval,
        );
      }
    };
  }, [url, reconnectInterval, maxReconnectAttempts]);

  useEffect(() => {
    shouldReconnectRef.current = true;
    connect();

    return () => {
      shouldReconnectRef.current = false;

      if (reconnectTimerRef.current !== null) {
        window.clearTimeout(reconnectTimerRef.current);
      }

      const socket = wsRef.current;
      if (socket) {
        socket.onopen = null;
        socket.onmessage = null;
        socket.onerror = null;
        socket.onclose = null;

        if (
          socket.readyState === WebSocket.OPEN ||
          socket.readyState === WebSocket.CONNECTING
        ) {
          socket.close();
        }
      }
    };
  }, [connect]);

  const sendMessage = useCallback((payload: unknown) => {
    const socket = wsRef.current;

    if (socket?.readyState === WebSocket.OPEN) {
      const text =
        typeof payload === "string" ? payload : JSON.stringify(payload);
      socket.send(text);
      return;
    }

    console.warn("WebSocket is not open. Message not sent.");
  }, []);

  return {
    status,
    lastMessage,
    sendMessage,
  };
}
