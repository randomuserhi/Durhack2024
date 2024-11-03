import { html } from "rhu/html.js";
import { Style } from "rhu/style.js";
import { App } from "../../app.js";
import { Bird } from "../../framework/entities/bird.js";
import { Dog } from "../../framework/entities/dog.js";
import { Tree } from "../../framework/entities/tree.js";
import { Worm } from "../../framework/entities/worm.js";
import * as BitHelper from "../../lib/bithelper.js";
import { Renderer } from "../../lib/renderer.js";
import { ByteStream } from "../../lib/stream.js";
import { World } from "../../lib/world.js";

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
    renderer.background = "#FEFAE0";
    
    const world = new World(renderer, 20, 20, 40);
    (window as any).world = world;

    const renderLoop = () => {
        renderer.pos.x = 0.5 * (-canvas.width + world.width * world.tileSize);
        renderer.pos.y = 0.5 * (-canvas.height + world.width * world.tileSize);  

        world.update();
        renderer.draw();
    };

    App.ws.addEventListener("message", (e) => {
        const data = new ByteStream(new Uint8Array(e.data));

        const step = BitHelper.readInt(data);
    
        const numWorldStates = BitHelper.readInt(data);
        for (let i = 0; i < numWorldStates; ++i) {
            const state = BitHelper.readString(data); 
            switch (state) {
            default: throw new Error(`${state} not implemented`);
            }
        }

        const width = BitHelper.readInt(data);
        const height = BitHelper.readInt(data);
        const size = width * height;
        for (let i = 0; i < size; ++i) {
            world.tiles[i].parse(data);
        }

        const numEntities = BitHelper.readInt(data);
        for(let i = 0; i < numEntities; ++i) {
            const type = BitHelper.readString(data);
            const id = BitHelper.readInt(data);
            const pos = BitHelper.readVector(data);
            const dir = BitHelper.readVector(data);
            const state = BitHelper.readString(data);

            let entity = world.entities.get(id);
            if (entity === undefined) {
                switch (type) {
                case "Tree": {
                    entity = new Tree(world);
                } break;
                case "Worm": {
                    entity = new Worm(world);
                } break;
                case "Bird": {
                    entity = new Bird(world);
                } break;
                case "Dog": {
                    entity = new Dog(world);
                } break;
                default: throw new Error(`${type} not implemented`);
                }
            }
        
            entity.parse(data, id, state, pos, dir);
            world._entities.set(id, entity);
        }
        for (const [id, entity] of world.entities) {
            if (!world._entities.has(id)) entity.destroy();
        }
        world.entities.clear();
        const temp = world.entities;
        world.entities = world._entities;
        world._entities = temp;

        renderLoop();
    });

    const test = new ByteStream();
    BitHelper.writeString("get", test);
    const code: string = makeid(5);
    App.code(code);
    BitHelper.writeString(code, test);
    App.ws.send(BitHelper.GetBytes(test));

    return dom;
};

function makeid(length: number) {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    const charactersLength = characters.length;
    let counter = 0;
    while (counter < length) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
        counter += 1;
    }
    return result;
}