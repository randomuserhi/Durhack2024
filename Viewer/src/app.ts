import { html } from "rhu/html.js";
import { signal, Signal } from "rhu/signal.js";
import { Style } from "rhu/style.js";
import { Theme } from "rhu/theme.js";

export const theme = Theme(({ theme }) => {
    return {
    };
});

const style = Style(({ css }) => {
    const wrapper = css.class`
    font-family: roboto;

    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: column;

    background-color:#FEFAE0;

    overflow: hidden;
    `;

    const body = css.class`
    flex: 1;
    `;

    const button = css.class`
    border: none;
    background-color: #606C38;
    color: #FEFAE0;
    padding: 16px 32px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 16px;
    font-family: serif;
    margin: 4px 2px;
    border-radius: 50px;
    transition-duration: 0.4s;
    cursor: pointer;
    `;
    css`
    ${button}:hover {
        background-color: #DDA15E;
        color: #FEFAE0;
    }`;

    const divider = css.class`
    color: #606C38;
    font-size: 2em;
    font-family: Georgia;
    display: flex;
    align-items: center;
    `;
    
    css`
    ${divider}::before, ${divider}::after {
        flex: 1;
        content: "";
        padding: 2px;
        background-color: #283618;
        margin: 5px;
    }`;

    return {
        wrapper,
        body,
        button,
        divider
    };
});

export const App = (() => {
    interface App {
        ws: WebSocket;
        body: HTMLDivElement;
        button: HTMLButtonElement;
        burn: HTMLButtonElement;
        code: Signal<string>;
    }

    const code = signal("...");

    const app = html<App>/**//*html*/`
    <div class=${style.wrapper}>
        <h1 style="text-align:center; color:#606C38; font-family:Georgia; font-size:3em;"><b>Biosphere</b></h1>
        <div style="width:100%; display: flex; align-items: center; justify-content: center;">
            <div style="display: flex; gap: 10px;">
                <button m-id="button" class="${style.button}">New</button><button m-id="burn" class="${style.button}">Burn</button>
            </div>
        </div>
        <div class="${style.divider}">${code}</div>
        <img src="./images/simon.png" style="height: 60%; position: absolute; top: 50%; left: 10px; transform: translate(0, -50%);"/>
        <img src="./images/simon.png" style="height: 60%; position: absolute; top: 50%; right: 10px; transform: translate(0, -50%);"/>
        <div m-id="body" class=${style.body}>
        </div>
    </div>
    `;

    app.button.addEventListener("click", () => window.location.reload());

    app.ws = new WebSocket("ws://127.0.0.1:8800/");
    app.ws.binaryType = "arraybuffer";

    app.code = code;

    return app;
})();

// Load app
const __load__ = () => {
    document.body.replaceChildren(...App);
};
if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", __load__);
} else {
    __load__();
}