using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edge.Hyperion.BSP {
    public class BspRec {
        private static int MIN_SIZE = 6;
        private static int MAX_SIZE = 20;
        private static Random rng = new Random();

        private int Top, Left, Width, Height;

        private BspRec leftChild;
        private BspRec rightChild;
        private BspRec dungeon;

        public BspRec(int top, int left, int width, int height) {
            Top = top;
            Left = left;
            Width = width;
            Height = height;
        }

        public bool split() {
            if (leftChild != null) //if already split, bail out
                return false;
            bool horizontal = rng.Next(1) > 1 ? true: false; //direction of split
            int max = (horizontal ? Height : Width) - MIN_SIZE; //maximum height/width we can split off
            if (max <= MIN_SIZE) // area too small to split, bail out
                return false;
            int split = rng.Next(MAX_SIZE); // generate split point 
            if (split < MIN_SIZE)  // adjust split point so there's at least MIN_SIZE in both partitions
                split = MIN_SIZE;
            if (horizontal) { //populate child areas
                leftChild = new BspRec(Top, Left, split, Width);
                rightChild = new BspRec(Top + split, Left, Height - split, Width);
            }
            else {
                leftChild = new BspRec(Top, Left, Height, split);
                rightChild = new BspRec(Top, Left + split, Height, Width - split);
            }
            return true; //split successful
        }

        public void generateDungeon() {
            if (leftChild != null) { //if current are has child areas, propagate the call
                leftChild.generateDungeon();
                rightChild.generateDungeon();
            }
            else { // if leaf node, create a dungeon within the minimum size constraints
                int dungeonTop = (Height - MIN_SIZE <= 0) ? 0 : rng.Next(Height - MIN_SIZE);
                int dungeonLeft = (Width - MIN_SIZE <= 0) ? 0 : rng.Next(Width - MIN_SIZE);
                int dungeonHeight = Math.Max(rng.Next(Height - dungeonTop), MIN_SIZE);
                int dungeonWidth = Math.Max(rng.Next(Width - dungeonLeft), MIN_SIZE);
                dungeon = new BspRec(Top + dungeonTop, Left + dungeonLeft, dungeonHeight, dungeonWidth);
            }
        }
    }
}
