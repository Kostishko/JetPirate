using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetPirate
{
    static class PhysicManager
    {
        static private List<Object2D> physicObjects;


        static public void UpdateMe()
        {
            for (int i = 0; i<physicObjects.Count; i++)
            {
                for(int j=0; j<physicObjects.Count; j++)
                {
                    if (i!=j)
                    {
                        if(physicObjects[i].GetRectangle().Intersects(physicObjects[j].GetRectangle()))
                        {
                            physicObjects[i].Collided(physicObjects[j]);
                        }
                    }
                }
                
            }
        }

        public static void AddObject(Object2D obj)
        {
            physicObjects.Add(obj);
        }

        public static void RemoveObject(Object2D obj)
        { 
            physicObjects.Remove(obj);
        }
    }
}
