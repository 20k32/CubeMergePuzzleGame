namespace DicedomPuzzleGame
{
    public partial class Form1 : Form
    {
        Button[] Array;
        Engine Engine;
        GameLogic Logic;
        public Form1()
        {
            InitializeComponent();
            Array = new Button[25]
            {
                button1, button2, button3, button4, button5, button6, button7, button8, button9, button10,
                button11, button12, button13, button14, button15, button16, button17, button18, button19, 
                button20, button21, button22, button23, button24, button25
            };
            Engine = new Engine(Array, 5);
            Engine.Generate(8);
            Engine.GenerateUserDice(button26);
            Logic = new GameLogic(Array, 5);
        }

        private void button26_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                button26.DoDragDrop(button26.Tag, DragDropEffects.Move);
            }
        }
        private void button1_DragOver(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.Text) && ((Button)sender).Tag == "0")
            {
                e.Effect = DragDropEffects.Move;
            }
            else
                e.Effect = DragDropEffects.None;
        }
        private void button1_DragDrop(object sender, DragEventArgs e)
        {
            if (((Button)sender).Tag == "0")
            { 
                ((Button)sender).Tag = e.Data.GetData(DataFormats.Text).ToString();
                Logic.Merge(int.Parse(button26.Tag.ToString()));
                Engine.GenerateUserDice(button26);
                label1.Text = Logic.GetScore();
                if(Logic.Loose())
                {
                    MessageBox.Show($"You loose, your score is {Logic.GetScore()} !");
                    Logic.Score = 0;
                    label1.Text = Logic.GetScore();
                    Engine.Reset();
                    Engine.Generate(8);
                }
            }
        }
        private void button27_Click(object sender, EventArgs e)
        {
            Logic.Score = 0;
            label1.Text = Logic.GetScore();
            Engine.Reset();
            Engine.Generate(8);
            Engine.GenerateUserDice(button26);
            Logic = new GameLogic(Array, 5);
        }
    }
}