using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;
using System.IO;

namespace Wordle_Assignment;

// Cian Dicker-Hughes
// G00415413@atu.ie

public partial class Scoreboard : ContentPage
{
    // Variables
    private const string FileName = "GameStoreboard.txt";
    private const string GamesPlayedSection = "[GamesPlayed]";
    private const string GameRowsSection = "[GameRows]";
    private List<int> nextRows = new List<int>() { 0, 0, 0, 0, 0, 0 };
    private int gamesPlayedCountwin = 0;
    private int gamesPlayedCount = 0;
    private const int TotalRows = 6;

    public Scoreboard()
	{
		InitializeComponent();
        (nextRows, gamesPlayedCountwin, gamesPlayedCount) = GetFromFile();
        ScoreboardLable();
    }

    // showing Scoreboard
    private void ScoreboardLable() 
    {
        gamesplayed_lbl.Text = gamesPlayedCount.ToString();
        gameswon_lbl.Text = gamesPlayedCountwin.ToString();
        gameswononrow_1.Text = nextRows[0].ToString();
        gameswononrow_2.Text = nextRows[1].ToString();
        gameswononrow_3.Text = nextRows[2].ToString();
        gameswononrow_4.Text = nextRows[3].ToString();
        gameswononrow_5.Text = nextRows[4].ToString();
        gameswononrow_6.Text = nextRows[5].ToString();
    }

    // get next row from wordle.xaml.cs and if player win a game
    public void SetNextRow(int nextRow)
    {
        AddNextRow(nextRow);
    }

    public void Game()
    {
        nextRows.Clear();
        (nextRows, gamesPlayedCountwin, gamesPlayedCount) = GetFromFile();
        gamesPlayedCount++;
        SaveGame(nextRows, gamesPlayedCountwin, gamesPlayedCount);
    }


    // save the game
    public static void SaveGame(List<int> nextRows, int gamesPlayedCountwin, int gamesPlayedCount)
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string filePath = System.IO.Path.Combine(folderPath, FileName);

        // Construct the content for the file
        string content = $"[GamesPlayedWin]\n{gamesPlayedCountwin}\n\n[GamesPlayed]\n{gamesPlayedCount}\n\n[GameRows]\n{string.Join("\n", nextRows)}";

        // Write all the data to the file
        File.WriteAllText(filePath, content);
    }


    // get nextRow and player win conunt and ++
    public void AddNextRow(int nextRow)
    {
        nextRows.Clear();
        (nextRows, gamesPlayedCountwin, gamesPlayedCount) = GetFromFile();

        for (int i = 0; i < TotalRows; i++)
        {
            if ((i + 1) == nextRow)
            {
                nextRows[i]++;
            }
        }

        gamesPlayedCountwin++;
        gamesPlayedCount++;

        SaveGame(nextRows, gamesPlayedCountwin, gamesPlayedCount);
    }

    // read from file
    public (List<int> nextRows, int gamesPlayedCountwin, int gamesPlayedCount) GetFromFile()
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    string filePath = System.IO.Path.Combine(folderPath, FileName);

    List<int> nextRows = new List<int>() { 0, 0, 0, 0, 0, 0 };
    int gamesPlayedCountwin = 0;
    int gamesPlayedCount = 0;

    // if file exist
    if (File.Exists(filePath))
    {
        string[] lines = File.ReadAllLines(filePath);
        int rowIndex = 0;
        bool readGameRows = false;

        // read the file
        foreach (string line in lines)
        {
            if (line == "[GameRows]")
            {
                readGameRows = true;
                continue;
            }
            else if (line == "[GamesPlayedWin]")
            {
                readGameRows = false;
                continue;
            }
            else if (line == "[GamesPlayed]")
            {
                readGameRows = false;
                continue;
            }

            if (readGameRows)
            {
                if (int.TryParse(line, out int nextRow) && rowIndex < TotalRows)
                {
                    nextRows[rowIndex] = nextRow;
                    rowIndex++;
                }
            }
                else
                {
                    if (int.TryParse(line, out int count))
                    {
                        gamesPlayedCount = count; 
                        gamesPlayedCountwin = count; 
                    }
                }
            }
        }

        return (nextRows, gamesPlayedCountwin, gamesPlayedCount);
    }
}