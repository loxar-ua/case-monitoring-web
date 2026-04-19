import { useEffect, useMemo, useState } from "react";
import { useWebSocket } from "../hooks/useWebSocket.ts";
import "./LiveAlerts.css";

const WEBSOCKET_URL = "ws://localhost:5261/ws/alerts";
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
    let title = "Новий алерт";
    if (typeof payload.title === "string") {
      title = payload.title;
    } else if (typeof payload.type === "string") {
      title = payload.type;
    }

    let description = JSON.stringify(payload);
    if (typeof payload.message === "string") {
      description = payload.message;
    } else if (typeof payload.body === "string") {
      description = payload.body;
    }

    return { title, description };
  }

  return {
    title: "Новий алерт",
    description: JSON.stringify(message),
  };
}

function removeAlertById(items: AlertItem[], alertId: string) {
  return items.filter((item) => item.id !== alertId);
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

    const autoHide = globalThis.setTimeout(() => {
      setAlerts((current) => removeAlertById(current, newAlert.id));
    }, AUTO_HIDE_MS);

    return () => {
      globalThis.clearTimeout(autoHide);
    };
  }, [lastMessage]);

  const dismissAlert = (alertId: string) => {
    setAlerts((current) => removeAlertById(current, alertId));
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
