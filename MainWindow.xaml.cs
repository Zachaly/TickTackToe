using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace TickTackToe
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        bool GameWon = false;
        Field[,] Fields;

        public MainWindow()
        {
            InitializeComponent();

            Fields = new Field[3, 3];
            //getting game fields
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    foreach (Field el in grid.Children)
                        if (Grid.GetRow(el) == i && Grid.GetColumn(el) == j)
                            Fields[i, j] = el;
                }
        }

        //checks of anyone can win
        void CheckWin()
        {
            int playerWin = 0, aiWin = 0;
            
            //Checks vertically and horizontally
            for (int i = 0; i < 3; i++)
            {
                
                playerWin = 0;
                aiWin = 0;
                //horizontall
                for (int j = 0; j < 3; j++)
                {
                    if (Fields[i, j].Circle)
                        aiWin++;
                    else if (Fields[i, j].Cross)
                        playerWin++;
                }
                
                CheckPoints(playerWin, aiWin);
                if (GameWon) return;

                playerWin = 0;
                aiWin = 0;

                //verticall
                for (int j = 0; j < 3; j++)
                {
                    if (Fields[j, i].Circle)
                        aiWin++;
                    else if (Fields[j, i].Cross)
                        playerWin++;
                }

                CheckPoints(playerWin, aiWin);
                if (GameWon) return;
            }

            playerWin = 0;
            aiWin = 0;
            //Checks first diagonal line
            for (int i = 0; i < 3; i++)
            {
                if (Fields[i, i].Circle)
                    aiWin++;
                else if (Fields[i, i].Cross)
                    playerWin++;
            }

            CheckPoints(playerWin, aiWin);
            if (GameWon) return;

            playerWin = 0;
            aiWin = 0;
            //Checks second diagonal line
            int y;
            for (int i = 2; i >= 0; i--)
            {
                y = 2 - i;
                if (Fields[i, y].Circle)
                    aiWin++;
                else if (Fields[i, y].Cross)
                    playerWin++;
            }

            CheckPoints(playerWin, aiWin);
            if (GameWon) return;

            int markedFields = 0;
            for(int i = 0; i< 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (Fields[i, j].Circle || Fields[i, j].Cross)
                        markedFields++;
                }
            }
            if (markedFields == 9)
                Win(false, false);

        }

        //Checks points for the player and ai
        void CheckPoints(int player, int ai)
        {
            if (player == 3)
            {
                Win(true, false);
                GameWon = true;
                return;
            }
            else if (ai == 3)
            {
                Win(false, true);
                GameWon = true;
                return;
            }
        }

        //AI move
        public void MakeMove()
        {
            CheckWin();
            if (GameWon)
                return;
            bool FieldMarked = false;
            //AI prefers the middle field
            if (!Fields[1, 1].Circle && !Fields[1, 1].Cross)
            {
                Fields[1, 1].Circle = true;
                FieldMarked = true;
            }

            if(!FieldMarked)
            {
                //Creation of a list with empty fields
                List<Field> PossibleTargets = new List<Field>();

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (!Fields[i, j].Circle && !Fields[i, j].Cross)
                            PossibleTargets.Add(Fields[i, j]);
                    }
                }

                // If there are no empty fields game ends
                if(PossibleTargets.Count == 0)
                {
                    Win(false, false);
                    return;
                }
                //If there is one empty field, there is only one solution
                if(PossibleTargets.Count == 1)
                {
                    PossibleTargets[0].Circle = true;
                    CheckWin();
                    return;
                }

                //First checks if ai can win by 3 circles in a row or column, or can it at least block player
                int CirclesInColumn = 0, CirclesInRow = 0,
                    CrossesInColumn = 0, CrossesInRow = 0;
                for (int i = 0; i < 3; i++)
                {
                    CirclesInColumn = 0; CirclesInRow = 0;
                    CrossesInColumn = 0; CrossesInRow = 0;

                    for (int j = 0; j < 3; j++)
                    {
                        //Checks a column
                        if (Fields[i, j].Circle)
                            CirclesInColumn++;
                        else if (Fields[i, j].Cross)
                            CrossesInColumn++;

                        //Checks a row
                        if (Fields[j, i].Circle)
                            CirclesInRow++;
                        else if (Fields[j, i].Cross)
                            CrossesInRow++;
                    }
                    //Can ai win or block in this column
                    if((CrossesInColumn == 2 && CirclesInColumn == 0) || (CirclesInColumn == 2 && CrossesInColumn == 0))
                    {
                        for(int j = 0; j < 3; j++)
                        {
                            if(!Fields[i, j].Circle && !Fields[i, j].Cross)
                            {
                                Fields[i, j].Circle = true;
                                FieldMarked = true;
                            }
                        }
                        
                    }
                    //Can ai win or block in this row
                    else if ((CrossesInRow == 2 && CirclesInRow == 0) || (CirclesInRow == 2 && CrossesInRow == 0))
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (!Fields[j, i].Circle && !Fields[j, i].Cross)
                            {
                                Fields[j, i].Circle = true;
                                FieldMarked = true;
                            }
                        }
                    }

                    if (FieldMarked)
                        break;
                }

                //If there is no good lines horizontally or vertically, AI will try with diagonal lines
                if (!FieldMarked)
                {
                    
                    int Circles = 0, Crosses = 0;
                    for(int i = 0; i < 3; i++)
                    {
                        if (Fields[i, i].Circle)
                            Circles++;
                        else if (Fields[i, i].Cross)
                            Crosses++;
                    }

                    if((Crosses == 2 && Circles == 0) || (Circles == 2 && Crosses == 0))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if(!Fields[i,i].Circle && !Fields[i, i].Cross)
                            {
                                Fields[i, i].Circle = true;
                                FieldMarked = true;
                                break;
                            }
                        }
                    }

                    Circles = 0; Crosses = 0;
                    int y;
                    for (int i = 2; i >= 0; i--)
                    {
                        y = 2 - i;
                        if (Fields[i, y].Circle)
                            Circles++;
                        else if (Fields[i, y].Cross)
                            Crosses++;
                    }

                    if ((Crosses == 2 && Circles == 0) || (Circles == 2 && Crosses == 0))
                    {
                        for (int i = 2; i >= 0; i--)
                        {
                            y = 2 - i;
                            if(Fields[i,y].Circle && Fields[i, y].Cross)
                            {
                                Fields[i, y].Circle = true;
                                FieldMarked = true;
                                break;
                            }
                        }
                    }

                    //If there is still no marked field, it will just pick a random one from possible ones
                    if (!FieldMarked)
                    {
                        Random rand = new Random();
                        PossibleTargets[rand.Next(0, PossibleTargets.Count)].Circle = true;
                    }
                }
            }
            CheckWin();
        }

        void Win(bool player, bool ai)
        {
            if (player)
            {
                MessageBox.Show("Game won!");
            }
            else if(ai)
            {
                MessageBox.Show("Game over!");
            }
            else
                MessageBox.Show("A draw!");
        }
    }
}
