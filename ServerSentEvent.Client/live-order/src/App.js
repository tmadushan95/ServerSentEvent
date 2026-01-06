import logo from "./logo.svg";
import "./App.css";
import { useState, useEffect } from "react";

function App() {
  const [latestOrder, setLatestOrder] = useState(null);

  useEffect(() => {
    // Create the EventSource inside useEffect
    const evtSource = new EventSource("https://localhost:7211/live-Order");

    evtSource.addEventListener("order", (event) => {
      const order = JSON.parse(event.data);
      setLatestOrder(order);
    });

    evtSource.onerror = function (err) {
      console.error("SSE error:", err);
      evtSource.close(); // Close the connection on error
    };

    // Cleanup: close SSE when component unmounts
    return () => {
      evtSource.close();
    };
  }, []); // Empty dependency array = run once

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />

        {!latestOrder && <p>Loading...</p>}
        {latestOrder && (
          <div className="order-card">
            <span className="order-label">Latest Order:</span>
            <span className="order-name">{latestOrder?.name}</span>
            <span className="order-price">${latestOrder?.price}</span>
          </div>
        )}
      </header>
    </div>
  );
}

export default App;
