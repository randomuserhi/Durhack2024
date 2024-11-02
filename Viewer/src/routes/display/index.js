import { html } from "rhu/html.js";
import { Style } from "rhu/style.js";

const style = Style(({ css }) => {
    const wrapper = css.class`
    width: 100%;
    height: 100%;
    padding: 0px;
    margin: 0px;
    `;

    return {
        wrapper,
    };
});

export const Display = () => {
    const dom = html`<canvas m-id="canvas" width="1920" height="1080" class="${style.wrapper}"></canvas>`;
    const { canvas } = dom;
    
    const ctx = canvas.getContext("2d");
    
    const resize = () => {
        let computed = getComputedStyle(canvas);
        canvas.width = parseInt(computed.width);
        canvas.height = parseInt(computed.height);        
    };
    window.addEventListener("resize", resize);
    canvas.addEventListener("mount", resize);

    return dom;
}