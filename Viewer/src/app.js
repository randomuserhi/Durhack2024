import { html } from "rhu/html.js";
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

    background-color: #fff;

    overflow: hidden;
    `;

    const body = css.class`
    flex: 1;
    margin: 5px;
    `;

    return {
        wrapper,
        body
    };
});

export const App = (() => {
    return html`
    <div class=${style.wrapper}>
        <div class=${style.body}></div>
    </div>
    `;
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