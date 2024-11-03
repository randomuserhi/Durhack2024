import { App } from "./app.js";
import { Display } from "./routes/display/index.js";

(window as any).app = App;

const load = () => {
    App.body.append(...Display());
};

if (App.ws.readyState == WebSocket.OPEN) {
    load();
} else {
    App.ws.addEventListener("open", load);
}