using System.Windows.Forms;

namespace Digger
{
    public class Player : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var deltaXY = GetDeltaXY(Game.KeyPressed);

            var deltaX = x + deltaXY.Item1 < Game.MapWidth ? deltaXY.Item1 : 0;
            var deltaY = y + deltaXY.Item2 < Game.MapHeight ? deltaXY.Item2 : 0;

            return CanMoveTo(x + deltaX, y + deltaY) ?
                new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY } :
                new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Gold)
                Game.Scores += 10;

            if (!(conflictedObject is Sack) &&
                !(conflictedObject is Monster))
                return false;
            Game.IsOver = true;
            return true;
        }

        public int GetDrawingPriority() => 9;

        public string GetImageFileName() => "Digger.png";

        private static bool CanMoveTo(int x, int y)
        {
            if (x > Game.MapWidth ||
                y > Game.MapHeight ||
                x < 0 || y < 0)
                return false;

            if (Game.Map[x, y] == null)
                return true;

            return !(Game.Map[x, y] is Sack);
        }

        private static (int, int) GetDeltaXY(Keys keyPressed)
        {
            var deltaX = 0;
            var deltaY = 0;

            switch (keyPressed)
            {
                case System.Windows.Forms.Keys.Left:
                    deltaX--;
                    break;
                case System.Windows.Forms.Keys.Up:
                    deltaY--;
                    break;
                case System.Windows.Forms.Keys.Right:
                    deltaX++;
                    break;
                case System.Windows.Forms.Keys.Down:
                    deltaY++;
                    break;
            }

            return (deltaX, deltaY);
        }
    }
}