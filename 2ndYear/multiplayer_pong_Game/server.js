const http = require("http");
const apiServer = require("./api");
const httpServer = http.createServer(apiServer);
const sockets = require("./sockets");
const io = require("socket.io");
const socketServer = io(httpServer);

sockets.listen(socketServer);

httpServer.listen(3000, () => {
  console.log("Server is listening on port 3000");
});
