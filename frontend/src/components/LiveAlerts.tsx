import { useEffect, useMemo, useState } from "react";
import { useWebSocket } from "../hooks/useWebSocket.ts";
import "./LiveAlerts.css";

const WEBSOCKET_URL = "ws://localhost:5000/ws/alerts";
const AUTO_HIDE_MS = 8000;

type AlertItem = {
  id: string;
  title: string;
  description: string;
};

function createId() {
  return `alert-${Date.now()}-${Math.floor(Math.random() * 10000)}`;
}

function normalizeMessage(message: unknown) {
  if (!message) {
    return {
      title: "Новий алерт",
      description: "Нове повідомлення з WebSocket.",
    };
  }

  if (typeof message === "string") {
    return {
      title: "Новий алерт",
      description: message,
    };
  }

  if (typeof message === "object") {
    const payload = message as Record<string, unknown>;
    const title =
      typeof payload.title === "string"
        ? payload.title
        : typeof payload.type === "string"
        ? payload.type
        : "Новий алерт";
    const description =
      typeof payload.message === "string"
        ? payload.message
        : typeof payload.body === "string"
        ? payload.body
        : JSON.stringify(payload);

    return { title, description };
  }

  return {
    title: "Новий алерт",
    description: String(message),
  };
}

export default function LiveAlerts() {
  const { status, lastMessage } = useWebSocket<unknown>(WEBSOCKET_URL, {
    reconnectInterval: 3000,
    maxReconnectAttempts: 10,
  });

  const [alerts, setAlerts] = useState<AlertItem[]>([]);

  const connectionLabel = useMemo(() => {
    switch (status) {
      case "connecting":
        return "Підключення...";
      case "open":
        return "Підключено";
      case "closing":
        return "Закривається...";
      case "closed":
        return "Відключено";
      case "error":
        return "Помилка";
      default:
        return status;
    }
  }, [status]);

  useEffect(() => {
    if (!lastMessage) {
      return;
    }

    const payload = normalizeMessage(lastMessage);
    const newAlert: AlertItem = {
      id: createId(),
      title: payload.title,
      description: payload.description,
    };

    setAlerts((current) => [newAlert, ...current]);

    const autoHide = window.setTimeout(() => {
      setAlerts((current) => current.filter((item) => item.id !== newAlert.id));
    }, AUTO_HIDE_MS);

    return () => {
      window.clearTimeout(autoHide);
    };
  }, [lastMessage]);

  const dismissAlert = (alertId: string) => {
    setAlerts((current) => current.filter((item) => item.id !== alertId));
  };

  return (
    <div className="live-alerts-root">
      <div className="live-alerts-status">Live Alerts: {connectionLabel}</div>
      <div className="live-alerts-list">
        {alerts.map((alert) => (
          <div key={alert.id} className="live-alert-card">
            <div className="live-alert-card-header">
              <strong>{alert.title}</strong>
              <button
                type="button"
                className="live-alert-dismiss"
                onClick={() => dismissAlert(alert.id)}
                aria-label="Dismiss alert"
              >
                ×
              </button>
            </div>
            <p className="live-alert-description">{alert.description}</p>
          </div>
        ))}
      </div>
    </div>
  );
}
