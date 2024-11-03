import { Entity } from "../framework/entity.js";
import { Tile } from "../framework/tile.js";
import { Renderer } from "./renderer.js";

export class World {
    width: number;
    height: number;
    tileSize: number;

    entities = new Map<number, Entity>();
    _entities = new Map<number, Entity>();
    tiles: Tile[];

    renderer: Renderer;

    constructor(renderer: Renderer, width: number, height: number, tileSize: number) {
        this.renderer = renderer;
        
        this.width = width;
        this.height = height;
        this.tileSize = tileSize;

        this.tiles = [];
        for (let x = 0; x < width; ++x) {
            for (let y = 0; y < height; ++y) {
                this.tiles.push(new Tile(this, new Vec3(x, y)));
            }
        }
    }

    public update() {
        for (const tile of this.tiles) {
            tile.update();
        }

        for (const entity of this.entities.values()) {
            entity.update();
        }
    }
}

export class Vec3 {
    x: number = 0;
    y: number = 0;
    plane: number = 0;

    constructor(x: number = 0, y: number = 0, plane: number = 0) {
        this.x = x;
        this.y = y;
        this.plane = plane;
    }

    public add(vec: Vec3) {
        this.x += vec.x;
        this.y += vec.y;
        this.plane += vec.plane;
        return this;
    }

    public sub(vec: Vec3) {
        this.x -= vec.x;
        this.y -= vec.y;
        this.plane -= vec.plane;
        return this;
    }

    public copy(vec: Vec3) {
        this.x = vec.x;
        this.y = vec.y;
        this.plane = vec.plane;
        return this;
    }
}