namespace Digger
{
    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y) =>
            new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };

        public bool DeadInConflict(ICreature conflictedObject) => true;

        public int GetDrawingPriority() => 10;

        public string GetImageFileName() => "Terrain.png";
    }
}