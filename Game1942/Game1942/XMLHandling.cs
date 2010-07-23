using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Game1942.Core;
using System.Text;
using System.IO;
using System.Xml;

namespace Game1942
{
    /// <summary>
    /// This Class handles XML files and stuff linked to that...
    /// </summary>
    class XmlHandling
    {
        /// <summary>
        /// readFromXML takes the path to the XML file and returns an array of highscoreObject, 
        /// line by line
        /// </summary>
        /// <returns></returns>
        public static highscoreObject[] ReadFromXML(string path)
        {
            StreamReader file = new StreamReader(path);
            string line;
            highscoreObject[] tmp = new highscoreObject[10];
            int index = 0;
            while ((line = file.ReadLine()) != null)
            {
                tmp[index].PlayerName = XmlHandling.FindXMLString(line, "<PlayerName>", "</PlayerName>");
                tmp[index].PlayerScore = XmlHandling.FindXMLInt(line, "<Score>", "</Score>");
                index++;

            }
            file.Close();
            return tmp;
        }


        /// <summary>
        /// FindXMLInt takes a XML line, XML tag's and pick outs the data between the XML tag's
        /// It returns the Data as Int
        /// </summary>
        /// <param name="textline"></param>
        /// <param name="tagg"></param>
        /// <param name="sluttagg"></param>
        /// <returns></returns>
        public static int FindXMLInt(string textline, string tagg, string sluttagg)
        {
            int index = 0;
            int indexSlut = 0;
            int lnTagg = tagg.Length;
            int lnSlutTagg = sluttagg.Length;
            if (textline.IndexOf(tagg) > -1)
            {
                index = textline.IndexOf(tagg) + lnTagg;
                indexSlut = textline.IndexOf(sluttagg) - index;
            }
            return Convert.ToInt32(textline.Substring(index, indexSlut));
        }

        /// <summary>
        /// FindXMLString takes a XML line, XML tag's and pick outs the data between the XML tag's
        /// It returns the Data as String
        /// </summary>
        /// <param name="textline"></param>
        /// <param name="tagg"></param>
        /// <param name="sluttagg"></param>
        /// <returns></returns>
        public static string FindXMLString(string textline, string tagg, string sluttagg)
        {
            int index = 0;
            int indexSlut = 0;
            int lnTagg = tagg.Length;
            int lnSlutTagg = sluttagg.Length;
            if (textline.IndexOf(tagg) > -1)
            {
                index = textline.IndexOf(tagg) + lnTagg;
                indexSlut = textline.IndexOf(sluttagg) - index;
            }
            return textline.Substring(index, indexSlut);
        }

        /// <summary>
        /// SortHighscore takes a Highscorelist and sorts it. 
        /// </summary>
        /// <param name="highScoreList"></param>
        /// <returns></returns>
        public static highscoreObject[] SortHighscore(highscoreObject[] highScoreList)
        {
            highscoreObject tmp = new highscoreObject();
            for (int m = highScoreList.Length - 1; m > 0; m--)
            {
                for (int n = 0; n < m; n++)
                {
                    if (highScoreList[n].PlayerScore < highScoreList[n + 1].PlayerScore)
                    {
                        tmp = highScoreList[n];
                        highScoreList[n] = highScoreList[n + 1];
                        highScoreList[n + 1] = tmp;
                    }
                }
            }
            return highScoreList;
        }

        /// <summary>
        /// CheckInsertHighscore take as arguments a sorted Highscorelist[10] and a Score{Player, Score}. The function checks if
        /// the Score is a Highscore and inserts it if so. The result is a new sorted Highscorelist[10].
        /// </summary>
        /// <param name="highScoreList"></param>
        /// <param name="Score"></param>
        /// <returns></returns>
        public static highscoreObject[] CheckInsertHighscore(highscoreObject[] highScoreList, highscoreObject Score)
        {
            if (Score.PlayerScore > highScoreList[9].PlayerScore)
            {
                highScoreList[9] = Score;
                SortHighscore(highScoreList);
            }
            return highScoreList;
        }

        /// <summary>
        /// Checks if score is a highscore. Returns true if so..
        /// </summary>
        /// <param name="highScoreList"></param>
        /// <param name="Score"></param>
        /// <returns></returns>
        public static bool CheckInsertHighscore(highscoreObject[] highScoreList, int Score)
        {
            bool tmp = false;
            if (Score > highScoreList[9].PlayerScore) { tmp = true; }
            return tmp;
        }


        /// <summary>
        /// Saves a given sorted highscorelist as a XML file at the chosen path. If file doesn't exist it will create a
        /// new file on given path and then save the highscorelist
        /// </summary>
        /// <param name="highscoreList"></param>
        /// <param name="path"></param>
        public static void WriteHighScoreToXML(highscoreObject[] highscoreList, string path)
        {
            if (!File.Exists(path)) { CreateDefaultHighScoreList(path); }
            StreamWriter file = new StreamWriter(path);
            for (int i = 0; i < highscoreList.Length - 1; i++)
            {
                file.Write("<Score>");
                file.Write(highscoreList[i].PlayerScore);
                file.Write("</Score>");
                file.Write("<PlayerName>");
                file.Write(highscoreList[i].PlayerName);
                file.WriteLine("</PlayerName>");
            }
            file.Close();
        }

        /// <summary>
        /// Creates and stores a Default highscorelist in chosen path/file. If the file exists, it
        /// overwrite/resets the existing one with a Default highscorelist.
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDefaultHighScoreList(string path)
        {
            XmlTextWriter newFile = new XmlTextWriter(path, System.Text.Encoding.UTF8);
            newFile.Formatting = Formatting.Indented;
            newFile.Close();
            StreamWriter file = new StreamWriter(path);
            for (int i = 0; i < 9; i++)
            {
                file.Write("<Score>");
                file.Write("0");
                file.Write("</Score>");
                file.Write("<PlayerName>");
                file.Write("nil");
                file.WriteLine("</PlayerName>");
            }
            file.Close();
        }
    }
}
