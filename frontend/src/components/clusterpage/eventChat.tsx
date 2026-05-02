// import React, { useEffect, useState, useRef } from "react";
// import * as signalR from "@microsoft/signalr";
// import "./eventChat.css";

// interface Message {
//   user: string;
//   text: string;
// }

// const EventChat: React.FC<{ eventId: string }> = ({ eventId }) => {
//   // Всі стейти мають бути тут на початку
//   const [isOpen, setIsOpen] = useState(false);
//   const [messages, setMessages] = useState<Message[]>([]);
//   const [input, setInput] = useState("");
//   const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
//   const messagesEndRef = useRef<HTMLDivElement>(null);

//   const [userName] = useState(() => {
//     const saved = localStorage.getItem("chat_user_name");
//     if (saved) return saved;
//     const newName = `Анонім_${Math.floor(Math.random() * 9000) + 1000}`;
//     localStorage.setItem("chat_user_name", newName);
//     return newName;
//   });

//   // Автопрокрутка
//   useEffect(() => {
//     if (isOpen) {
//       messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
//     }
//   }, [messages, isOpen]);

//   // SignalR логіка
//   useEffect(() => {
//     const conn = new signalR.HubConnectionBuilder()
//       .withUrl("http://localhost:5261/chatHub") // Переконайся, що порт вірний
//       .withAutomaticReconnect()
//       .build();

//     conn.start()
//       .then(async () => {
//         console.log("SignalR Connected");
//         await conn.invoke("JoinEvent", eventId);
//       })
//       .catch(err => console.error("SignalR Connection Error: ", err));

//     conn.on("ReceiveMessage", (user, text) => {
//       setMessages(prev => [...prev, { user, text }]);
//     });

//     setConnection(conn);

//     return () => {
//       if (conn) conn.stop();
//     };
//   }, [eventId]);

//   const sendMessage = async () => {
//     if (connection && connection.state === signalR.HubConnectionState.Connected && input.trim() !== "") {
//       try {
//         await connection.invoke("SendMessage", eventId, userName, input);
//         setInput("");
//       } catch (err) {
//         console.error("SendMessage error:", err);
//       }
//     }
//   };

//   return (
//     <>
//       <button 
//         onClick={() => setIsOpen(!isOpen)}
//         className="chat-button"
//       >
//         {isOpen ? "✖" : "💬"}
//       </button>

//       {isOpen && (
//         <div className="chat-window">
//           <div className="chat-header">
//             <strong>Обговорення справи</strong>
//             <small style={{ display: 'block', opacity: 0.8 }}>Ви: {userName}</small>
//           </div>

//           <div className="messages-container">
//             {messages.map((m, i) => (
//               <div 
//                 key={i} 
//                 style={{ 
//                   textAlign: m.user === userName ? "right" : "left",
//                   marginBottom: "10px" 
//                 }}
//               >
//                 <div 
//                   className="message-bubble"
//                   style={{
//                     backgroundColor: m.user === userName ? "#007bff" : "#e9ecef",
//                     color: m.user === userName ? "white" : "black",
//                     borderRadius: m.user === userName ? "15px 15px 2px 15px" : "15px 15px 15px 2px",
//                     display: "inline-block",
//                     padding: "8px 12px",
//                     maxWidth: "80%",
//                     wordWrap: "break-word"
//                   }}
//                 >
//                   <small style={{ display: "block", fontSize: "10px", fontWeight: "bold", marginBottom: "2px" }}>
//                     {m.user}
//                   </small>
//                   {m.text}
//                 </div>
//               </div>
//             ))}
//             <div ref={messagesEndRef} />
//           </div>

//           <div className="input-area">
//             <input
//               className="chat-input"
//               value={input}
//               onChange={(e) => setInput(e.target.value)}
//               onKeyDown={(e) => e.key === "Enter" && sendMessage()}
//               placeholder="Напишіть..."
//             />
//             <button onClick={sendMessage} className="send-button">➤</button>
//           </div>
//         </div>
//       )}
//     </>
//   );
// };

// export default EventChat;

import React, { useEffect, useState, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import "./eventChat.css";

interface Message {
  user: string;
  text: string;
}

const EventChat: React.FC<{ eventId: string }> = ({ eventId }) => {
  const [isOpen, setIsOpen] = useState(false);
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  const [userName] = useState(() => {
    const saved = localStorage.getItem("chat_user_name");
    if (saved) return saved;
    const newName = `Анонім_${Math.floor(Math.random() * 9000) + 1000}`;
    localStorage.setItem("chat_user_name", newName);
    return newName;
  });

  // Прокрутка до останнього повідомлення
  useEffect(() => {
    if (isOpen) {
      messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    }
  }, [messages, isOpen]);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5261/chatHub") // Переконайтеся, що порт збігається з Backend
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, [eventId]);

  useEffect(() => {
    if (connection) {
      connection.start()
        .then(() => {
          console.log("Connected to SignalR");
          connection.invoke("JoinEvent", eventId);
          
          connection.on("ReceiveMessage", (user, text) => {
            setMessages(prev => [...prev, { user, text }]);
          });
        })
        .catch(error => console.log("Connection failed: ", error));
    }

    return () => {
      if (connection) {
        connection.off("ReceiveMessage");
        connection.stop();
      }
    };
  }, [connection, eventId]);

  const sendMessage = async () => {
    if (connection && connection.state === signalR.HubConnectionState.Connected && input.trim() !== "") {
      try {
        await connection.invoke("SendMessage", eventId, userName, input);
        setInput("");
      } catch (err) {
        console.error(err);
      }
    }
  };

  return (
    <div className="chat-wrapper">
      <button className="chat-toggle-btn" onClick={() => setIsOpen(!isOpen)}>
        {isOpen ? "✕" : "💬"}
      </button>

      {isOpen && (
        <div className="chat-popup">
          <div className="chat-header">
            <strong>Чат</strong>
            <span>Ви: {userName}</span>
          </div>

          <div className="chat-messages">
            {messages.map((m, i) => (
              <div key={i} className={`message-row ${m.user === userName ? "my-msg" : ""}`}>
                <div className="msg-bubble">
                  <div className="msg-user">{m.user}</div>
                  <div className="msg-text">{m.text}</div>
                </div>
              </div>
            ))}
            <div ref={messagesEndRef} />
          </div>

          <div className="chat-input-area">
            <input 
              value={input} 
              onChange={e => setInput(e.target.value)} 
              onKeyDown={e => e.key === "Enter" && sendMessage()}
              placeholder="Напишіть повідомлення..."
            />
            <button onClick={sendMessage}>➤</button>
          </div>
        </div>
      )}
    </div>
  );
};

export default EventChat;