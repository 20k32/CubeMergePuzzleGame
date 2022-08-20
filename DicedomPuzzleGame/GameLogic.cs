namespace DicedomPuzzleGame
{
    public class GameLogic
    {
        private Button[,] CurrentDices;
        private Button[] Dices;
        private int[,] Tmp;
        private int MergeCounter = 0;
        private const int DICE_TO_MERGE = 1;
        private int[,] CurrentUserPosition;
        public int Score { get; set; }
        public string GetScore()
        {
            return Score.ToString();
        }
        public void Initialize(Button[] dices, int dim)
        {
            Dices = dices;
            CurrentDices = new Button[dim, dim];
            int counterX = 0, counterY = 0;
            for (int i = 0; i < Dices.Length; i++)
            {
                if (i % dim == 0 && i != 0 && i != 25) counterY++;
                if (counterX == 5) counterX = 0;
                CurrentDices[counterY, counterX] = Dices[i];
                counterX++;
            }
        }
        public GameLogic(Button[] dices, int dim)
        {
            Initialize(dices, dim);
            CurrentUserPosition = new int[CurrentDices.GetLength(0), CurrentDices.GetLength(1)];
            Tmp = new int[dim, dim];
            GenerateCurrentUserPosition();
        }
        private void SpawnHeadDice(int tag, Button button)
        {
            button.BackgroundImage = Image.FromFile($@"Dices/{tag}.bmp");
            button.Tag = tag.ToString();
        }
        private void GenerateCurrentUserPosition()
        {
            for (int i = 0; i < CurrentDices.GetLength(0); i++)
            {
                for (int j = 0; j < CurrentDices.GetLength(1); j++)
                {
                    CurrentUserPosition[i, j] = int.Parse(CurrentDices[i, j].Tag.ToString());
                }
            }
        }
        private (int, int) FindUserPosition()
        {
            for (int i = 0; i < CurrentDices.GetLength(0); i++)
            {
                for (int j = 0; j < CurrentDices.GetLength(1); j++)
                {
                    if (CurrentUserPosition[i, j] != int.Parse(CurrentDices[i, j].Tag.ToString()))
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }
        private bool IsValid(int i, int j, int n, int m)
        {
            return (i >= 0 && i < n && j >= 0 && j < m);
        }
        private void Remove(int[,] arr, int i, int j, int Value, bool flag)
        {
            arr[i, j] = Value;
            if (flag)
            {
                Score += (MergeCounter * int.Parse(CurrentDices[i, j].Tag.ToString())) * 5;
                CurrentDices[i, j].Tag = Value.ToString();    
            }
            else
                MergeCounter++;
        }
        private void _Wave(int[,] arr, int i, int j, int DiceValue, int n, int m, Queue<KeyValuePair<int, int>> pair, bool flag)
        {
            if (IsValid(i - 1, j, n, m) && arr[i - 1, j] == DiceValue)
            {
                Remove(arr, i - 1, j, 0, flag);
                pair.Enqueue(new KeyValuePair<int, int>(i - 1, j));
            }
            if (IsValid(i, j + 1, n, m) && arr[i, j + 1] == DiceValue)
            {
                Remove(arr, i, j + 1, 0, flag);
                pair.Enqueue(new KeyValuePair<int, int>(i, j + 1));
            }
            if (IsValid(i + 1, j, n, m) && arr[i + 1, j] == DiceValue)
            {
                Remove(arr, i + 1, j, 0, flag);
                pair.Enqueue(new KeyValuePair<int, int>(i + 1, j));
            }
            if (IsValid(i, j - 1, n, m) && arr[i, j - 1] == DiceValue)
            {
                Remove(arr, i, j - 1, 0, flag);
                pair.Enqueue(new KeyValuePair<int, int>(i, j - 1));
            }
        }
        private void Wave(int[,] arr, int x, int y, int rows, int cols, int DiceValue, bool flag)
        {
            var queue = new Queue<KeyValuePair<int, int>>();
            queue.Enqueue(new KeyValuePair<int, int>(x, y));
            while (queue.Count > 0)
            {
                int count = queue.Count;
                while (count > 0)
                {
                    KeyValuePair<int, int> point = queue.Dequeue();
                    int i = point.Key;
                    int j = point.Value;
                    _Wave(arr, i, j, DiceValue, rows, cols, queue, flag);
                    count--;
                }
            }
            if (MergeCounter > DICE_TO_MERGE)
            {
                if (DiceValue + 1 == 7) DiceValue = 0;
                SpawnHeadDice(DiceValue + 1, CurrentDices[x, y]);
            }
        }
        private void AdaptArray()
        {
            for (int i = 0; i < CurrentDices.GetLength(0); i++)
            {
                for (int j = 0; j < CurrentDices.GetLength(1); j++)
                {
                    CurrentUserPosition[i, j] = int.Parse(CurrentDices[i, j].Tag.ToString());
                    CurrentDices[i, j].BackgroundImage = Image.FromFile($@"Dices/{CurrentUserPosition[i, j]}.bmp");
                }
            }
        }
        public void Merge(int DiceValue)
        {
            var tuple = FindUserPosition();
            Array.Copy(CurrentUserPosition, Tmp, CurrentUserPosition.GetLength(0) * CurrentUserPosition.GetLength(1));
            Wave(Tmp, tuple.Item1, tuple.Item2, 5, 5, DiceValue, false);
            if (MergeCounter > DICE_TO_MERGE)
                Wave(CurrentUserPosition, tuple.Item1, tuple.Item2, 5, 5, DiceValue, true);
            AdaptArray();
            GenerateCurrentUserPosition();
            MergeCounter = 0;
        }
        public bool Loose()
        {
            bool loose = true;
            for (int i = 0; i < CurrentDices.GetLength(0); i++)
            {
                for (int j = 0; j < CurrentDices.GetLength(1); j++)
                {
                    if (CurrentDices[i, j].Tag == "0")
                        loose = false;
                }
            }
            return loose;
        }
    }
}
