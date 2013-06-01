using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Star.Game.Level
{
    public enum LV
    {
		BackgroundFX,
		RearParallaxFX,
		FrontParallaxFX,
		CloudsFX,
		LevelFX,
		PostFX,
        GraphXPack,
        RearParallaxLayerImage,
        FrontParallaxLayerImage,
        RearParallaxLayerSpeedDivider,
        FrontParallaxLayerSpeedDivider,
        BackgroundImg,
        CloudLayerImgs,
        QuadtreeLayerDepth,
		Enemies,
		RearDecorationsLayerData,
		FrontDecorationsLayerData,
		InteractiveObjects

    }

    public class LevelVariables
    {
		bool successfull = true;

		public bool Successfull
		{
			get { return successfull; }
		}
        string graphXPack;
        List<string> ErrorMessages;
        string rearParallaxLayerImage;
        string frontParallaxLayerImage;
        int rearParallaxLayerSpeedDivider;
        int frontParallaxLayerSpeedDivider;
        string backgroundImg;
        string[] cloudLayerImgs;
        int quadtreeLayerDepth;
        Rectangle exitrect;
        Dictionary<string, string> dict;
        Dictionary<LV, string> lv;
        List<Rectangle> killrectangles;

        #region Properties

        public Dictionary<LV, string> Dictionary
        {
            get { return lv; }
            set 
            { 
                lv = value;
            }
        }

        //public string GraphXPack
        //{
        //    get { return graphXPack; }
        //    set { graphXPack = value; }
        //}

        //public string RearParallaxLayerImage
        //{
        //    get { return rearParallaxLayerImage; }
        //    set { rearParallaxLayerImage = value; }
        //}

        //public string FrontParallaxLayerImage
        //{
        //    get { return frontParallaxLayerImage; }
        //    set { frontParallaxLayerImage = value; }
        //}

        //public int RearParallaxLayerSpeedDivider
        //{
        //    get { return rearParallaxLayerSpeedDivider; }
        //}

        //public int FrontParallaxLayerSpeedDivider
        //{
        //    get { return frontParallaxLayerSpeedDivider; }
        //}

        //public string BackgroundImg
        //{
        //    get { return backgroundImg; }
        //}

        //public string[] CloudLayerImgs
        //{
        //    get { return cloudLayerImgs; }
        //}

        //public int QuadtreeLayerDepth
        //{
        //    get { return quadtreeLayerDepth; }
        //}

        public Rectangle ExitRectangle
        {
            get { return exitrect; }
        }

        public List<Rectangle> KillRectangles
        {
            get { return killrectangles; }
        }


        #endregion

        //public LevelVariables(string levelfilecontext,Tile[,] leveltiles)
        //{
        //    ErrorMessages = new List<string>();
        //    string[] leveldata = levelfilecontext.Split(';');
        //    dict = new Dictionary<string, string>();
        //    foreach (string data in leveldata)
        //    {
        //        string valuename;
        //        string value;
        //        string[] temp = data.Split('=');
        //        if (temp.Length > 1)
        //        {
        //            valuename = temp[0];
        //            value = temp[1];
        //            valuename = valuename.Trim();
        //            value = value.Trim();

        //            dict.Add(valuename, value);
        //        }
        //    }
            
        //    ParseDict();
        //    GetExit(leveltiles);
        //}

        public LevelVariables(string levelfilecontext, Tile[,] leveltiles)
        {
            ErrorMessages = new List<string>();
            lv = new Dictionary<LV, string>();
            dict = new Dictionary<string, string>();
            killrectangles = new List<Rectangle>();
            string[] leveldata = levelfilecontext.Split(';');
            
            foreach (string data in leveldata)
            {
                string valuename;
                string value;
                string[] temp = data.Split('=');
                if (temp.Length > 1)
                {
                    valuename = temp[0];
                    value = temp[1];
                    valuename = valuename.Trim();
                    value = value.Trim();

                    dict.Add(valuename, value);
                }
            }
            foreach(LV levelvariables in Enum.GetValues(typeof(LV)))
            {
                try
                {
					if (dict.ContainsKey(levelvariables.ToString()))
						lv.Add(levelvariables, dict[levelvariables.ToString()]);
					else
						lv.Add(levelvariables, "");
                }
                catch (Exception e)
                {
                    successfull = false;
                    ErrorMessages.Add("Failed to Load " + levelvariables.ToString() + "\n Additional Info: " + e.Message);
                }
            }
            ParseDict();
            GetExit(leveltiles);
        }

        /// <summary>
        /// Generates a Default LevelVariables
        /// </summary>
        public LevelVariables()
        {
			LayerFXData fxData = LayerFXData.Default;
            graphXPack = "Grassy";
            rearParallaxLayerImage = "Default";
            frontParallaxLayerImage = "Default";
            rearParallaxLayerSpeedDivider = 4;
            frontParallaxLayerSpeedDivider = 2;
            quadtreeLayerDepth = 6;
            ErrorMessages = new List<string>();
            backgroundImg = "Sky";
            killrectangles = new List<Rectangle>();
            cloudLayerImgs = new string[4];
            for (int i = 0; i < 4; i++)
            {
                cloudLayerImgs[i] = "Cloud"+i.ToString();
            }
            lv = new Dictionary<LV, string>();
            lv.Add(LV.GraphXPack, graphXPack);
            lv.Add(LV.RearParallaxLayerImage, rearParallaxLayerImage);
            lv.Add(LV.RearParallaxLayerSpeedDivider, rearParallaxLayerSpeedDivider.ToString());
            lv.Add(LV.QuadtreeLayerDepth, quadtreeLayerDepth.ToString());
            lv.Add(LV.FrontParallaxLayerSpeedDivider, frontParallaxLayerSpeedDivider.ToString());
            lv.Add(LV.FrontParallaxLayerImage, frontParallaxLayerImage);
            lv.Add(LV.BackgroundImg, backgroundImg);
			lv.Add(LV.RearDecorationsLayerData, "");
			lv.Add(LV.FrontDecorationsLayerData, "");
			lv.Add(LV.InteractiveObjects, "");
			foreach (LayerFX layer in Enum.GetValues(typeof(LayerFX)))
				lv.Add((LV)Enum.Parse(typeof(LV), layer.ToString()), string.Empty);
            string temp;
            temp = cloudLayerImgs[0];
            for (int i = 1; i < cloudLayerImgs.Length; i++)
            {
                temp += "," + cloudLayerImgs[i];
            }
            lv.Add(LV.CloudLayerImgs, temp);
            
        }

        private void GetExit(Tile[,] leveltiles)
        {
            foreach (Tile tile in leveltiles)
            {
                if (tile.TileType == TileType.Exit)
                {
                    exitrect = tile.get_rect;
                    break;
                }
            }
        }

        private void GetKillRectangles(Tile[,] leveltiles)
        { 
            foreach(Tile tile in leveltiles)
            {
                if (tile.TileType == TileType.Spike)
                {
                    Rectangle rect = tile.get_rect;
                    rect.Y += 12;
                    rect.Height -= 12;
                    killrectangles.Add(rect);
                }
            }
        }

        private void ParseDict()
        {
            try
            {
                graphXPack = dict["GraphXPack"];
            }
            catch (Exception e)
            {
                successfull = false;
                ErrorMessages.Add("No GraphXPack found in LevelData\nAdditional Info: " +e.Message);
            }
            try
            {
                rearParallaxLayerImage = dict["RearParallaxLayerImage"];
            }
            catch (Exception e)
            {
                successfull = false;
                ErrorMessages.Add("No Rear Parallax Layer Image found in Level Data\nAdditional Info: " + e.Message);
                
            }
            try
            {
                rearParallaxLayerSpeedDivider = int.Parse(dict["RearParallaxLayerSpeedDivider"]);
            }
            catch (Exception e)
            {
                successfull = false;
                ErrorMessages.Add("No Rear Parallax Speed Divider found in Level Data\nAdditional Info: " + e.Message);
                
            }

            try
            {
                frontParallaxLayerImage = dict["FrontParallaxLayerImage"];
            }
            catch (Exception e)
            {
                successfull = false;
                ErrorMessages.Add("No Front Parallax Layer Image found in Level Data\nAdditional Info: " + e.Message);
                
            }
            try
            {
                frontParallaxLayerSpeedDivider = int.Parse(dict["FrontParallaxLayerSpeedDivider"]);
            }
            catch (Exception e)
            {
                successfull = false;
                ErrorMessages.Add("No Front Parallax Speed Divider found in Level Data\nAdditional Info: " + e.Message);
            }
            try
            {
                backgroundImg = dict["BackgroundImg"];
            }
            catch (Exception e)
            {
                successfull = false;
                ErrorMessages.Add("No Background Image found in Level Data\nAdditional Info: " + e.Message);
            }

            try
            {
                quadtreeLayerDepth = int.Parse(dict["QuadtreeLayerDepth"]);
            }
            catch (Exception e)
            {
                successfull = false;
                ErrorMessages.Add("No valid Quadtree Layer Information in Level Data\nAdditional Info: " + e.Message);
            }
            try
            {
                string[] temp = dict["CloudLayerImgs"].Split(',');
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = temp[i].Trim();
                }
                cloudLayerImgs = temp;
            }
            catch (Exception e)
            {
                successfull = false;
                ErrorMessages.Add("No valid Cloudlayer Imgs in Level Data\nAdditional Info: " + e.Message);
            }
        }

        public override string ToString()
        {
            string data;
            data = "";
            foreach (LV levelvariable in Enum.GetValues(typeof(LV)))
            {
                try
                {
                    data += levelvariable.ToString() + "=" + lv[levelvariable] + ";\n";
                }
                catch (Exception)
                {
                    //FileManager.WriteInErrorLog(this, e.Message, e.GetType());
                }
                
            }
            return data;
        }
    }
}
