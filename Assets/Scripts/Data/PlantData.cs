using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantData
{
    public enum BlockType {
        Core, Stem, Stem_Shoot, Branch, Leaf, Root_Stem, Root_Branch, Bacteria_Hub, Nitrate_Intake, FlowerBud
    }

    public enum CoreState{
        Level1, Level2, Level3
    }

    public enum BranchState {
        Small_Nub,
        Growing_Leaf_Attatchments_A, 
        Growing_Leaf_Attatchments_B,
        Grown_Nub
    }

    public enum StemState {
        Green,
        Mid,
        BrownStem,
        BrownLink,
        Thick_Brown
    }

    public enum StemShootState {
        Baby,
        Mid,
        Full
    }

    public enum RootBranchState {
        Small,
        Medium,
        Large
    }

    public enum RootStemState {
        Regular,
        Thick,
        Full
    }

    public enum LeafState {
        Small,
        Medium,
        Large
    }

    public enum FlowerBudState {
        stage1, stage2, bloomReady
    }

    [System.Serializable]
    public class PlantCollider{
        public Vector2 size;
        public Vector2 offset;
    }

    [System.Serializable]
    public class StemCollider{
        public StemState stemState;
        public PlantCollider plantCollider;
    }

    [System.Serializable]
    public class StemShootCollider{
        public StemShootState stemShootState;
        public PlantCollider plantCollider;
    }

    [System.Serializable]
    public class RootBranchCollider{
        public RootBranchState rootBranchState;
        public PlantCollider plantCollider;
    }

    [System.Serializable]
    public class LeafCollider{
        public LeafState leafState;
        public PlantCollider plantCollider;
    }

    [System.Serializable]
    public class RootStemCollider{
        public RootStemState rootStemState;
        public PlantCollider plantCollider;
    }

    public enum Resource{
        Glucose, N2, Ammonium, Nitrite, Nitrate
    }

    public class UpgradeData {
        public string name;
        public int cost;
        public Resource resource;

        public UpgradeData(string name, int cost, Resource resource){
            this.name = name;
            this.cost = cost;
            this.resource = resource;
        }
    }
}
