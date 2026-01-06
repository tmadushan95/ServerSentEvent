# Live Orders SSE Project (Only Prototype)

## Overview

This project demonstrates a **real-time live order feed prototype** using **Server-Sent Events (SSE)** in **.NET 10 Minimal API** and a **React frontend**.  

The system streams live order data continuously:

- **Backend (.NET 10 Minimal API)**: Generates fake orders every 2 seconds using `IAsyncEnumerable<SseItem<Order>>`.  
- **Frontend (React)**: Consumes SSE via `EventSource` and renders orders in real-time.

This is intended as a **prototype** for dashboards or live feeds.

---

## Features

- Real-time streaming of orders without polling  
- Clean and responsive React component for live orders  
- Handles client disconnects gracefully  
- CORS configured for React development  
- Professional, maintainable, and extensible code structure  
- Prototype-level SSE event naming (`order`)

---
