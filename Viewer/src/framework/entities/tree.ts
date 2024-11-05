import { LoadImage, Sprite } from "../../lib/renderer.js";
import { World } from "../../lib/world.js";
import { Entity } from "../entity.js";

const ancient = LoadImage("./images/entities/tree.png");

const states = new Map();
states.set("sprout", LoadImage("./images/entities/sprout.png"));
states.set("sapling", LoadImage("./images/entities/sapling.png"));
states.set("mature", LoadImage("./images/entities/mature.png"));
states.set("ancient", LoadImage("./images/entities/ancient.png"));
states.set("decaying", LoadImage("./images/entities/decayed.png"));

export class Tree extends Entity {
    sprite: Sprite;

    constructor(world: World) {
        super(world);

        this.sprite = new Sprite(ancient, world.tileSize, world.tileSize * 1.5);
        this.sprite.layer = 0;
        this.sprite.offset.y = -this.world.tileSize * 0.25;

        this.update();
    }

    public update() {
        this.sprite.image = states.get(this.state);
        if (this.sprite.image === undefined) this.world.renderer.removeSprite(this.sprite);
        else this.world.renderer.addSprite(this.sprite);
        this.sprite.pos.x = this.pos.x * this.world.tileSize * 0.5; 
        this.sprite.pos.y = this.pos.y * this.world.tileSize * 0.5;
    }

    public destroy(): void {
        this.world.renderer.removeSprite(this.sprite);
    }
}