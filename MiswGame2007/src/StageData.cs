using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Yanesdk.Ytl;
using Yanesdk.System;

namespace MiswGame2007
{
    public class StageData
    {
        private static string[] TEST = { "\r\n" };

        string path;
        int numRows;
        int numCols;
        string[] mapData;
        string[][] thingData;
        private bool bossStage;

        public StageData(string path, bool bossStage)
        {
            this.path = path;
            string[] stageData;
            int currentLine;
            try
            {
                stageData = Encoding.ASCII.GetString(FileSys.Read(path)).Split(TEST, StringSplitOptions.None);
                currentLine = 0;
            }
            catch
            {
                throw new Exception("ステージデータ「" + path + "」を読み込めません＞＜");
            }
            try
            {
                numRows = int.Parse(stageData[currentLine]);
                currentLine++;
                numCols = int.Parse(stageData[currentLine]);
                currentLine++;
            }
            catch
            {
                throw new Exception("ステージデータ「" + path + "」の第" + (currentLine + 1) + "行のマップのサイズの指定がおかしいです＞＜");
            }
            mapData = new string[numRows];
            for (int row = 0; row < numRows; row++)
            {
                try
                {
                    mapData[row] = stageData[currentLine];
                    currentLine++;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new Exception("ステージデータ「" + path + "」の高さが指定されたものに一致しません＞＜");
                }
                if (mapData[row].Length != numCols)
                {
                    throw new Exception("ステージデータ「" + path + "」の第" + (currentLine + 1) + "行の幅が指定されたものに一致しません＞＜");
                }
            }
            List<string[]> thingDataList = new List<string[]>();
            while (currentLine < stageData.Length)
            {
                thingDataList.Add(stageData[currentLine].Split(' '));
                currentLine++;
            }
            thingData = thingDataList.ToArray();
            this.bossStage = bossStage;
        }

        public Map GetMap(GameScene game)
        {
            return new Map(game, numRows, numCols, mapData);
        }

        public Player GetPlayer(GameScene game)
        {
            try
            {
                for (int i = 0; i < thingData.Length; i++)
                {
                    if (thingData[i][0] == "Player")
                    {
                        int row, col;
                        try
                        {
                            row = int.Parse(thingData[i][1]);
                            col = int.Parse(thingData[i][2]);
                        }
                        catch
                        {
                            throw new Exception("ステージデータ「" + path + "」のPlayerの位置の指定がおかしいです＞＜");
                        }
                        Player.Direction direction;
                        if (thingData[i][3] == "Left")
                        {
                            direction = Player.Direction.Left;
                        }
                        else if (thingData[i][3] == "Right")
                        {
                            direction = Player.Direction.Right;
                        }
                        else
                        {
                            throw new Exception("ステージデータ「" + path + "」のPlayerの向きの指定がおかしいです＞＜");
                        }
                        return new Player(game, row, col, direction);
                    }
                }
                throw new Exception("ステージデータ「" + path + "」にPlayerの定義がありません＞＜");
            }
            catch (Exception e)
            {
                if (e is IndexOutOfRangeException)
                {
                    throw new Exception("ステージデータ「" + path + "」がおかしいです＞＜");
                }
                else
                {
                    throw e;
                }
            }
        }

        public ThingList GetEnemies(GameScene game)
        {
            ThingList enemies = new ThingList();
            for (int i = 0; i < thingData.Length; i++)
            {
                Thing enemy = CreateEnemyFromData(thingData[i], game);
                if (enemy != null)
                {
                    enemies.AddThing(enemy);
                }
            }
            return enemies;
        }

        public ExitDoor GetExitDoor(GameScene game)
        {
            try
            {
                for (int i = 0; i < thingData.Length; i++)
                {
                    if (thingData[i][0] == "ExitDoor")
                    {
                        int row, col;
                        try
                        {
                            row = int.Parse(thingData[i][1]);
                            col = int.Parse(thingData[i][2]);
                        }
                        catch
                        {
                            throw new Exception("ステージデータ「" + path + "」のExitDoorの位置の指定がおかしいです＞＜");
                        }
                        if (!bossStage)
                        {
                            return new ExitDoor(game, row, col);
                        }
                        else
                        {
                            return new ExitDoorForBossStage(game, row, col);
                        }
                    }
                }
                throw new Exception("ステージデータ「" + path + "」にExitDoorの定義がありません＞＜");
            }
            catch (Exception e)
            {
                if (e is IndexOutOfRangeException)
                {
                    throw new Exception("ステージデータ「" + path + "」がおかしいです＞＜");
                }
                else
                {
                    throw e;
                }
            }
        }

        private Thing CreateEnemyFromData(string[] thingData, GameScene game)
        {
            try
            {
                switch (thingData[0])
                {
                    case "TestEnemy":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のTestEnemyの位置の指定がおかしいです＞＜");
                            }
                            TestEnemy.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = TestEnemy.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = TestEnemy.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のTestEnemyの向きの指定がおかしいです＞＜");
                            }
                            return new TestEnemy(game, row, col, direction);
                        }
                    case "House":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のHouseの位置の指定がおかしいです＞＜");
                            }
                            House.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = House.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = House.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のHouseの向きの指定がおかしいです＞＜");
                            }
                            return new House(game, row, col, direction);
                        }
                    case "Baaka":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のBaakaの位置の指定がおかしいです＞＜");
                            }
                            Baaka.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = Baaka.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = Baaka.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のBaakaの向きの指定がおかしいです＞＜");
                            }
                            return new Baaka(game, row, col, direction);
                        }
                    case "Babo":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のBaboの位置の指定がおかしいです＞＜");
                            }
                            Babo.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = Babo.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = Babo.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のBaboの向きの指定がおかしいです＞＜");
                            }
                            return new Babo(game, row, col, direction);
                        }
                    case "Kyoro":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のKyoroの位置の指定がおかしいです＞＜");
                            }
                            Kyoro.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = Kyoro.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = Kyoro.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のKyoroの向きの指定がおかしいです＞＜");
                            }
                            return new Kyoro(game, row, col, direction);
                        }
                    case "Nurunuru":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のNurunuruの位置の指定がおかしいです＞＜");
                            }
                            Nurunuru.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = Nurunuru.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = Nurunuru.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のNurunuruの向きの指定がおかしいです＞＜");
                            }
                            return new Nurunuru(game, row, col, direction);
                        }
                    case "Byaa":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のByaaの位置の指定がおかしいです＞＜");
                            }
                            Byaa.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = Byaa.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = Byaa.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のByaaの向きの指定がおかしいです＞＜");
                            }
                            return new Byaa(game, row, col, direction);
                        }
                    case "ItemEnemy":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のItemEnemyの位置の指定がおかしいです＞＜");
                            }
                            ItemEnemy.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = ItemEnemy.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = ItemEnemy.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のItemEnemyの向きの指定がおかしいです＞＜");
                            }
                            switch (thingData[4])
                            {
                                case "Machinegun":
                                    return new ItemEnemy(game, row, col, direction, ItemEnemy.Item.Machinegun);
                                case "Rocket":
                                    return new ItemEnemy(game, row, col, direction, ItemEnemy.Item.Rocket);
                                case "Shotgun":
                                    return new ItemEnemy(game, row, col, direction, ItemEnemy.Item.Shotgun);
                                case "Flame":
                                    return new ItemEnemy(game, row, col, direction, ItemEnemy.Item.Flame);
                                case "Health":
                                    return new ItemEnemy(game, row, col, direction, ItemEnemy.Item.Health);
                                default:
                                    throw new Exception("ステージデータ「" + path + "」のItemEnemyのアイテムの指定がおかしいです＞＜");
                            }
                            
                        }
                    case "Norio":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のNorioの位置の指定がおかしいです＞＜");
                            }
                            Norio.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = Norio.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = Norio.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のNorioの向きの指定がおかしいです＞＜");
                            }
                            return new Norio(game, row, col, direction);
                        }
                    case "Mushroom":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のMushroomの位置の指定がおかしいです＞＜");
                            }
                            Mushroom.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = Mushroom.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = Mushroom.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のMushroomの向きの指定がおかしいです＞＜");
                            }
                            bool visible;
                            if (thingData[4] == "Visible")
                            {
                                visible = true;
                            }
                            else if (thingData[4] == "Invisible")
                            {
                                visible = false;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のMushroomの可視設定がおかしいです＞＜");
                            }
                            return new Mushroom(game, row, col, direction, visible);
                        }
                    case "Robot":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のRobotの位置の指定がおかしいです＞＜");
                            }
                            Robot.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = Robot.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = Robot.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のRobotの向きの指定がおかしいです＞＜");
                            }
                            return new Robot(game, row, col, direction);
                        }
                    case "Skater":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のSkaterの位置の指定がおかしいです＞＜");
                            }
                            Skater.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = Skater.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = Skater.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のSkaterの向きの指定がおかしいです＞＜");
                            }
                            return new Skater(game, row, col, direction);
                        }
                    case "BlackPlayer":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のBlackPlayerの位置の指定がおかしいです＞＜");
                            }
                            BlackPlayer.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = BlackPlayer.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = BlackPlayer.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のBlackPlayerの向きの指定がおかしいです＞＜");
                            }
                            switch (thingData[4])
                            {
                                case "Pistol":
                                    return new BlackPlayer(game, row, col, direction, BlackPlayer.Weapon.Pistol);
                                case "Machinegun":
                                    return new BlackPlayer(game, row, col, direction, BlackPlayer.Weapon.Machinegun);
                                case "Rocket":
                                    return new BlackPlayer(game, row, col, direction, BlackPlayer.Weapon.Rocket);
                                case "Shotgun":
                                    return new BlackPlayer(game, row, col, direction, BlackPlayer.Weapon.Shotgun);
                                case "Flame":
                                    return new BlackPlayer(game, row, col, direction, BlackPlayer.Weapon.Flamethrower);
                                default:
                                    throw new Exception("ステージデータ「" + path + "」のBlackPlayerの武器の指定がおかしいです＞＜");
                            }

                        }
                    case "Worm":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のWormの位置の指定がおかしいです＞＜");
                            }
                            Worm.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = Worm.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = Worm.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のWormの向きの指定がおかしいです＞＜");
                            }
                            return new Worm(game, row, col, direction);
                        }
                    case "Starman":
                        {
                            int row, col;
                            try
                            {
                                row = int.Parse(thingData[1]);
                                col = int.Parse(thingData[2]);
                            }
                            catch
                            {
                                throw new Exception("ステージデータ「" + path + "」のStarmanの位置の指定がおかしいです＞＜");
                            }
                            Starman.Direction direction;
                            if (thingData[3] == "Left")
                            {
                                direction = Starman.Direction.Left;
                            }
                            else if (thingData[3] == "Right")
                            {
                                direction = Starman.Direction.Right;
                            }
                            else
                            {
                                throw new Exception("ステージデータ「" + path + "」のStarmanの向きの指定がおかしいです＞＜");
                            }
                            return new Starman(game, row, col, direction);
                        }
                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                if (e is IndexOutOfRangeException)
                {
                    throw new Exception("ステージデータ「" + path + "」がおかしいです＞＜");
                }
                else
                {
                    throw e;
                }
            }
        }
    }
}
