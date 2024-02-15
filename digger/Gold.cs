namespace Digger
{
    public class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y) => new CreatureCommand();

        public bool DeadInConflict(ICreature conflictedObject) => true;

        public int GetDrawingPriority() => 10;

        public string GetImageFileName() => "Gold.png";
    }
}