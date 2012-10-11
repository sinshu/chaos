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
                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��ǂݍ��߂܂��񁄁�");
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
                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v�̑�" + (currentLine + 1) + "�s�̃}�b�v�̃T�C�Y�̎w�肪���������ł�����");
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
                    throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v�̍������w�肳�ꂽ���̂Ɉ�v���܂��񁄁�");
                }
                if (mapData[row].Length != numCols)
                {
                    throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v�̑�" + (currentLine + 1) + "�s�̕����w�肳�ꂽ���̂Ɉ�v���܂��񁄁�");
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
                            throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Player�̈ʒu�̎w�肪���������ł�����");
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
                            throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Player�̌����̎w�肪���������ł�����");
                        }
                        return new Player(game, row, col, direction);
                    }
                }
                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Player�̒�`������܂��񁄁�");
            }
            catch (Exception e)
            {
                if (e is IndexOutOfRangeException)
                {
                    throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v�����������ł�����");
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
                            throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��ExitDoor�̈ʒu�̎w�肪���������ł�����");
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
                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��ExitDoor�̒�`������܂��񁄁�");
            }
            catch (Exception e)
            {
                if (e is IndexOutOfRangeException)
                {
                    throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v�����������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��TestEnemy�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��TestEnemy�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��House�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��House�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Baaka�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Baaka�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Babo�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Babo�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Kyoro�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Kyoro�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Nurunuru�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Nurunuru�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Byaa�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Byaa�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��ItemEnemy�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��ItemEnemy�̌����̎w�肪���������ł�����");
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
                                    throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��ItemEnemy�̃A�C�e���̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Norio�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Norio�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Mushroom�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Mushroom�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Mushroom�̉��ݒ肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Robot�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Robot�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Skater�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Skater�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��BlackPlayer�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��BlackPlayer�̌����̎w�肪���������ł�����");
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
                                    throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��BlackPlayer�̕���̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Worm�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Worm�̌����̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Starman�̈ʒu�̎w�肪���������ł�����");
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
                                throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v��Starman�̌����̎w�肪���������ł�����");
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
                    throw new Exception("�X�e�[�W�f�[�^�u" + path + "�v�����������ł�����");
                }
                else
                {
                    throw e;
                }
            }
        }
    }
}
