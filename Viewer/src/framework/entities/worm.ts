import { LoadImage, Sprite } from "../../lib/renderer.js";
import { World } from "../../lib/world.js";
import { Entity } from "../entity.js";

const ancient = LoadImage("./images/entities/worm.png");

export class Worm extends Entity {
    sprite: Sprite;

    constructor(world: World) {
        super(world);

        this.sprite = new Sprite(ancient, world.tileSize, world.tileSize);
        this.sprite.layer = -1;

        this.world.renderer.addSprite(this.sprite);

        this.update();
    }

    public update() {
        this.sprite.pos.x = this.pos.x * this.world.tileSize * 0.5; 
        this.sprite.pos.y = this.pos.y * this.world.tileSize * 0.5;
    }

    public destroy(): void {
        this.world.renderer.removeSprite(this.sprite);
    }
}