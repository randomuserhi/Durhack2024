import { html } from "rhu/html.js";
import { Style } from "rhu/style.js";
import { Renderer } from "../../lib/renderer.js";

const style = Style(({ css }) => {
    const wrapper = css.class`
    display: block;
    width: 100%;
    height: 100%;
    border: 0px;
    `;

    return {
        wrapper,
    };
});

export const Display = () => {
    interface Display {
        canvas: HTMLCanvasElement;
    }

    const dom = html<Display>`<canvas m-id="canvas" width="1920" height="1080" class="${style.wrapper}"></canvas>`;
    const { canvas } = dom;

    const renderer = new Renderer(canvas);
    
    const renderLoop = () => {
        const dt = renderer.draw();
        requestAnimationFrame(renderLoop);
    };
    renderLoop();

    return dom;
};