import { useEffect, useState } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import type { ClusterPresenceStatus } from "../types";

type PresenceState = {
  activeUsers: number;
  status: ClusterPresenceStatus;
};

export function useClusterPresence(clusterId?: string) {
  const [state, setState] = useState<PresenceState>({
    activeUsers: 0,
    status: "disconnected",
  });

  useEffect(() => {
    if (!clusterId) {
      return;
    }

    let disposed = false;

    const connection = new HubConnectionBuilder()
      .withUrl("/hubs/presence")
      .withAutomaticReconnect()
      .build();

    const setActiveUsers = (count: number) => {
      if (!disposed) {
        setState((current) => ({ ...current, activeUsers: count }));
      }
    };

    connection.on("PresenceUpdated", (updatedClusterId: string, count: number) => {
      if (!disposed && updatedClusterId === clusterId) {
        setActiveUsers(count);
      }
    });

    connection.onreconnecting(() => {
      if (!disposed) {
        setState((current) => ({ ...current, status: "connecting" }));
      }
    });

    connection.onreconnected(async () => {
      if (disposed) {
        return;
      }

      try {
        const currentCount = await connection.invoke<number>("JoinCluster", clusterId);
        setState({ activeUsers: currentCount, status: "connected" });
      } catch (error) {
        console.error("Failed to resubscribe cluster presence:", error);
        setState((current) => ({ ...current, status: "error" }));
      }
    });

    connection.onclose(() => {
      if (!disposed) {
        setState((current) => ({ ...current, status: "disconnected" }));
      }
    });

    const startConnection = async () => {
      setState((current) => ({ ...current, status: "connecting" }));

      try {
        await connection.start();
        if (disposed) {
          return;
        }

        const currentCount = await connection.invoke<number>("JoinCluster", clusterId);
        setState({ activeUsers: currentCount, status: "connected" });
      } catch (error) {
        console.error("Failed to connect cluster presence hub:", error);
        if (!disposed) {
          setState({ activeUsers: 0, status: "error" });
        }
      }
    };

    void startConnection();

    return () => {
      disposed = true;
      void connection.stop();
    };
  }, [clusterId]);

  return state;
}