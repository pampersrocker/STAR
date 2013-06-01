using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Star.Game.Enemy
{
    public class Keyframe
    {
        Dictionary<string, FrameRectangle> rectangles;
        int keyframenumber;
        SpecialRect killingRect;
        SpecialRect dieRect;

        public Keyframe()
        {
            keyframenumber = 0;
            rectangles = new Dictionary<string, FrameRectangle>();
			killingRect = new SpecialRect(new Rectangle());
			dieRect = new SpecialRect(new Rectangle());
        }

        public Dictionary<string,FrameRectangle> GetRectangles
        {
            get { return rectangles; }
            set { rectangles = value; }
        }

        public int KeyFrameNumber
        {
            get { return keyframenumber; }
            set { keyframenumber = value; }
        }

        public SpecialRect KillingRect
        {
            get { return killingRect; }
            set {
				Rectangle rect = value.Rectangle;
				rect.Width = (int)MathHelper.Clamp(value.Rectangle.Width, 5, float.MaxValue);
				rect.Height = (int)MathHelper.Clamp(value.Rectangle.Height, 5, float.MaxValue);
				value.Rectangle = rect;
				killingRect = value; 
			}
        }

        public SpecialRect DieRect
        {
            get { return dieRect; }
            set {
				Rectangle rect = value.Rectangle;
				rect.Width = (int)MathHelper.Clamp(value.Rectangle.Width, 5, float.MaxValue);
				rect.Height = (int)MathHelper.Clamp(value.Rectangle.Height, 5, float.MaxValue);
				value.Rectangle = rect;
				dieRect = value; }
        }

        public void LoadFrame(string[] names,string pdata,int number)
        {
            FrameRectangle newrect= new FrameRectangle();
            string[] data = pdata.Split('_');
            string[] rects = data[0].Split(':');
            keyframenumber = number;
            if (rects.Length >= names.Length)
            {
                for (int i = 0; i < names.Length; i++)
                {
                    try
                    {
                        newrect.LoadFromData(rects[i]);

                        rectangles.Add(names[i], newrect);
                    }
                    catch
                    { 
                    
                    }
                }
                if (data.Length > 1)
                {
                    int x, y, width, height;
                    string[] killdata = data[1].Split(',');
                    if (killdata.Length >= 8)
                    {
                        Rectangle killRect;
                        x = int.Parse(killdata[0]);
                        y = int.Parse(killdata[1]);
                        width = int.Parse(killdata[2]);
                        height = int.Parse(killdata[3]);
                        killRect = new Rectangle(x, y, width, height);
                        KillingRect = new SpecialRect(killRect);

                        x = int.Parse(killdata[4]);
                        y = int.Parse(killdata[5]);
                        width = int.Parse(killdata[6]);
                        height = int.Parse(killdata[7]);
                        killRect = new Rectangle(x, y, width, height);
                        DieRect = new SpecialRect(killRect);
                    }
                    else
                    {
                        KillingRect = new SpecialRect(new Rectangle(0,0,10,10));
                        DieRect = new SpecialRect(new Rectangle(0,0,10,10));
                    }
                }
                else
                {
                    KillingRect = new SpecialRect(new Rectangle(0, 0, 10, 10));
                    DieRect = new SpecialRect(new Rectangle(0, 0, 10, 10));
                }
            }
            else
            {
                FileManager.WriteInErrorLog(this, "Es sind zu wenig FrameRectangle Daten vorhanden um alle Rectangles zu füllen");
                throw new ArgumentException("Es sind zu wenig FrameRectangle Daten vorhanden um alle Rectangles zu füllen");
            }
        }

        public void Scale(float scale)
        {
            Dictionary<string, FrameRectangle> temp = new Dictionary<string,FrameRectangle>();
            foreach(string Key in rectangles.Keys)
            {
                temp.Add(Key,rectangles[Key].Scale(scale));
            }
            foreach (string Key in temp.Keys)
            {
                rectangles[Key] = temp[Key];
            }
        }

        public string GetDataString()
        {
            string data="";
            foreach (FrameRectangle rect in rectangles.Values)
            {
                data += rect.GetDataString() + ":";
            }
			//if (killingRect == null)
				//killingRect = new SpecialRect(new Rectangle());
			//if (dieRect == null)
				//dieRect = new SpecialRect(new Rectangle());
            data += "_" + killingRect.DataString(",") + "," + dieRect.DataString(",");

            return data;
            
        }

        public override string ToString()
        {
            string ausgabe;

            ausgabe = keyframenumber.ToString("000") + " Rects: ";
            //foreach (string key in rectangles.Keys)
            //{
            //    ausgabe += key + " , ";
            //}
            ausgabe += rectangles.Keys.Count;

            return ausgabe;
        }

        public Keyframe Copy()
        {
            Keyframe frame = new Keyframe();

            foreach (string rect in GetRectangles.Keys)
            {
                frame.GetRectangles.Add(rect, GetRectangles[rect].Copy());
            }
            frame.KeyFrameNumber = keyframenumber;

            return frame;
        }
    }
}
