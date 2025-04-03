# Hybrid Cache Demo in .NET 9

Welcome to the **Hybrid Cache Demo**! 🚀

This repository demonstrates a simple implementation of a hybrid cache using **.NET 9**. The hybrid cache combines both **in-memory** and **distributed** caching to provide optimal performance and scalability.

## 🛠️ Prerequisites

- .NET 9 SDK
- Redis
- Docker

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/hybrid-cache-demo.git
   cd hybrid-cache-demo
   ```

2. Setup Redis server using Docker
   ```bash
   docker run -- redis -p 6379:6379 -p 8001:8001 redis/redis-stack
   ```
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run
   ```
## 📝 Usage

- Configure cache settings in `appsettings.json`

## 🤝 Contributing

Feel free to submit issues or pull requests. Contributions are welcome!


Happy caching! 😊
