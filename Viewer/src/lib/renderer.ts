export interface Vec2 {
    x: number;
    y: number;
}

export abstract class Sprite {
    pos: Vec2;
    
    constructor() {
        this.pos = { x: 0, y: 0 };
    }

    public abstract draw(ctx: CanvasRenderingContext2D, dt: number): void;
}

export class Renderer {
    canvas: HTMLCanvasElement;
    ctx: CanvasRenderingContext2D;

    tileSize: number = 10;
    private sprites: Sprite[] = [];

    pos: Vec2 = { x: 0, y: 0 };
    zoom: number;
    background: string = "#fff";

    constructor(canvas: HTMLCanvasElement) {
        this.canvas = canvas;
        const _ctx = canvas.getContext("2d");
        if (_ctx === null) throw new Error("Failed to get Canvas Context.");
        this.ctx = _ctx;
    
        window.addEventListener("resize", this.resize.bind(this));
        canvas.addEventListener("mount", this.resize.bind(this));
    }

    private prev?: number = undefined;
    public dt: number = 0;
    public draw() {
        const now = Date.now();
        if (this.prev === undefined) this.prev = now;
        this.dt = now - this.prev;
        this.prev = now;

        this.sprites.sort((a, b) => {
            return a.pos.y - b.pos.y;
        });

        this.ctx.save();
        this.ctx.translate(-this.pos.x, -this.pos.y);
        this.ctx.scale(this.zoom, this.zoom);

        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this.ctx.fillStyle = this.background;
        this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);

        for (const sprite of this.sprites) {
            this.ctx.save();
            this.ctx.translate(sprite.pos.x, sprite.pos.y);
            sprite.draw(this.ctx, this.dt);
            this.ctx.restore();
        }

        this.ctx.restore();

        return this.dt;
    }

    public resize() {
        const computed = getComputedStyle(this.canvas);
        this.canvas.width = parseInt(computed.width);
        this.canvas.height = parseInt(computed.height);
    }
}