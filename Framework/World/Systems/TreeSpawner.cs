using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Biosphere {
    internal class TreeSpawner : System {

        private float chance = 0.2f;

        public TreeSpawner() : base() { }

        public override void Update() {
            foreach (Tile tile in World.tiles) {
                chance = 0.2f;
                if (tile.planes[(int)Tile.Plane.foliage] != null) {
                    if (tile.HasState("fire")) {
                        continue;
                    }
                    if (World.IsOccupied(new Vec3(tile.pos.x, tile.pos.y + World.width, 2))) {
                        chance += 0.1f;
                    }
                    if (World.IsOccupied(new Vec3(tile.pos.x - 1, tile.pos.y, 2))) {
                        chance += 0.1f;
                    }
                    if (World.IsOccupied(new Vec3(tile.pos.x + 1, tile.pos.y, 2))) {
                        chance += 0.1f;
                    }
                    if (World.IsOccupied(new Vec3(tile.pos.x, tile.pos.y - World.width, 2))) {
                        chance += 0.1f;
                    }

                    var randomValue = Rand.Float(0, 1);
                    if (randomValue <= chance) {
                        World.AddEntity(new Tree(), new Vec3(tile.pos.x, tile.pos.y, 2));
                    }
                }
            }

        }


    }
