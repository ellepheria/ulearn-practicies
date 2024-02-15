namespace Digger
{
    public class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var diggerCoordinates = GetDiggerCoordinates(Game.Map);

            if (diggerCoordinates == (-1, -1))
            {
                Game.IsOver = true;
                return new CreatureCommand();
            }

            var delta = GetDeltaToDigger(diggerCoordinates, (x, y));

            return new CreatureCommand { DeltaX = delta.x, DeltaY = delta.y };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject == null)
                return false;

            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority() => 8;

        public string GetImageFileName() => "Monster.png";

        private static (int x, int y) GetDiggerCoordinates(ICreature[,] map)
        {
            var width = map.GetLength(0);
            var height = map.GetLength(1);

            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    if (map[x, y] != null)
                        if (map[x, y] is Player)
                            return (x, y);
            return (-1, -1);
        }

        private static (int x, int y) GetDeltaToDigger((int x, int y) diggerCoordinates,
            (int x, int y) currentCoordinates)
        {
            var deltaX = 0;
            var deltaY = 0;

            if (diggerCoordinates.x == currentCoordinates.x)
                deltaY = diggerCoordinates.y < currentCoordinates.y ? -1 : 1;
            else
                deltaX = diggerCoordinates.x < currentCoordinates.x ? -1 : 1;

            return CanMoveTo(Game.Map, currentCoordinates.x + deltaX, currentCoordinates.y + deltaY) ?
                (deltaX, deltaY) : (0, 0);
        }

        private static bool CanMoveTo(ICreature[,] map, int x, int y)
        {
            if (x < 0 || y < 0 ||
                x >= Game.MapWidth || y >= Game.MapHeight)
                return false;

            if (map[x, y] == null)
                return true;

            return !(map[x, y] is Terrain) &&
                !(map[x, y] is Sack) &&
                !(map[x, y] is Monster);
        }
    }
}