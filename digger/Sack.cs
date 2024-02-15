namespace Digger
{
    public class Sack : ICreature
    {
        private int flightDistance;

        public CreatureCommand Act(int x, int y)
        {
            if (y + 1 != Game.MapHeight)
            {
                if (GetBottomCell(x, y) is null ||
                    (GetBottomCell(x, y) is Player && flightDistance >= 1) ||
                    (GetBottomCell(x, y) is Monster && flightDistance >= 1))
                {
                    flightDistance++;
                    return new CreatureCommand { DeltaY = 1 };
                }
                else if (flightDistance > 1)
                    return new CreatureCommand { TransformTo = new Gold() };

                flightDistance = 0;
                return new CreatureCommand();
            }

            if (flightDistance > 1)
                return new CreatureCommand { TransformTo = new Gold() };

            flightDistance = 0;
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject) => false;

        public int GetDrawingPriority() => 10;

        public string GetImageFileName() => "Sack.png";

        private static ICreature GetBottomCell(int x, int y)
        {
            return Game.Map[x, y + 1];
        }
    }
}