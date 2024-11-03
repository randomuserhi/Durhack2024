export interface Vec2 {
    x: number;
    y: number;
}

export function LoadImage(src: string) {
    const img = new Image();
    img.src = src;
    return img;
}

export class Sprite {
    pos: Vec2;
    offset: Vec2;
    layer: number;
    image: HTMLImageElement;
    width: number;
    height: number;

    constructor(image: HTMLImageElement, width: number, height: number) {
        this.image = image;
        this.width = width;
        this.height = height;
        this.layer = 0;
        this.pos = { x: 0, y: 0 };
        this.offset = { x: 0, y: 0 };
    }

    public draw(ctx: CanvasRenderingContext2D, dt: number) {
        ctx.drawImage(this.image, this.pos.x - this.width / 2 + this.offset.x, this.pos.y - this.height / 2 + this.offset.y, this.width, this.height);
    }
}

export class Renderer {
    canvas: HTMLCanvasElement;
    ctx: CanvasRenderingContext2D;

    private spriteSet = new Set<Sprite>();
    private sprites: Sprite[] = [];
    public addSprite(...sprites: Sprite[]) {
        for (const sprite of sprites) {
            if (this.spriteSet.has(sprite)) continue;
            this.spriteSet.add(sprite);
            this.sprites.push(sprite);
        }
    }
    public removeSprite(sprite: Sprite) {
        if(!this.spriteSet.has(sprite)) return;
        const i = this.sprites.indexOf(sprite);
        if (i < 0) return;
        this.sprites.splice(i, 1);
        this.spriteSet.delete(sprite);
    }

    pos: Vec2 = { x: 0, y: 0 };
    zoom: number;
    background: string = "#000";

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
            let diff = a.layer - b.layer;
            if (diff === 0) diff = a.pos.y - b.pos.y;
            return diff;
        });

        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this.ctx.fillStyle = this.background;
        this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);

        this.ctx.save();
        this.ctx.translate(-this.pos.x, -this.pos.y);
        this.ctx.scale(this.zoom, this.zoom);

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