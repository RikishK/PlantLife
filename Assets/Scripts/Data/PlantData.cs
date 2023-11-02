using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantData
{
    public enum BlockType {
        Stem, Branch, Leaf, Root, Root_Branch
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
        Brown,
        Thick_Brown
    }

    public enum RootBranchState {
        Small,
        Medium,
        Large
    }

    public enum RootState {
        Regular,
        Thick
    }

    public enum LeafState {
        Small,
        Medium,
        Large
    }

    [System.Serializable]
    public class PlantCollider{
        public Vector2 size;
    }

    [System.Serializable]
    public class StemCollider{
        public StemState stemState;
        public PlantCollider plantCollider;
    }

    [System.Serializable]
    public class RootBranchCollider{
        public RootBranchState rootBranchState;
        public PlantCollider plantCollider;
    }
}
