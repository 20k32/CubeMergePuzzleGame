using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;

namespace DicedomPuzzleGame
{
    public class Engine
    {
        private Button[,] Dices;
        private Random Random = new Random((int)DateTime.Now.Ticks);
        public Engine(Button[] tmp, int dim)
        {
            Dices = new Button[dim, dim];
            int counterX = 0, counterY = 0;
            for (int i = 0; i < tmp.Length; i++)
            {
                if (i % dim == 0 && i != 0 && i != 25) counterY++;
                if (counterX == 5) counterX = 0;
                Dices[counterY, counterX] = tmp[i];
                counterX++;
            }
        }
        public void Reset()
        {
            for (int i = 0; i < Dices.GetLength(0); i++)
            {
                for (int j = 0; j < Dices.GetLength(1); j++)
                {
                    Dices[i, j].Tag = "0";
                    Dices[i, j].BackgroundImage = Image.FromFile($@"Dices/0.bmp"); ;
                }
            }
        }
        public void Generate(int MaxDiceCount)
        {
            int[,] PosYX = new int[2, MaxDiceCount];
            for (int i = PosYX.GetLength(1) - 1; i >= 0; i--)
            {
                PosYX[0, i] = Random.Next(0, 5);
                PosYX[1, i] = Random.Next(0, 5);
            }
            for (int i = 0; i < PosYX.GetLength(1); i++)
            {
                int tmp = Random.Next(1, 7);
                Dices[PosYX[1, i], PosYX[0, i]].Tag = tmp;
                Dices[PosYX[1, i], PosYX[0, i]].BackgroundImage = Image.FromFile($@"Dices/{tmp}.bmp");
            }
        }
        public void ShowTags(bool show)
        {
            if (show)
            {
                for (int i = 0; i < Dices.GetLength(0); i++)
                {
                    for (int j = 0; j < Dices.GetLength(1); j++)
                    { 
                        Dices[i, j].Text = Dices[i, j].Tag.ToString();
                    }
                }
            }
            else
            {
                for (int i = 0; i < Dices.GetLength(0); i++)
                {
                    for (int j = 0; j < Dices.GetLength(1); j++)
                    {
                        Dices[i, j].Text = "";
                    }
                }
            }
        }
        public void GenerateUserDice(Button button)
        {
            int RandomValue = Random.Next(1, 7);
            button.BackgroundImage = Image.FromFile($@"Dices/{RandomValue}.bmp");
            button.Tag = RandomValue.ToString();
        }
    }
}
