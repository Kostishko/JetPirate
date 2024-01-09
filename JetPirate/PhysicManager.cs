using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetPirate
{
    static class PhysicManager
    {
        static private List<PhysicModule> physicObjects;

        static PhysicManager()
        {
            physicObjects = new List<PhysicModule>();
        }

        /// <summary>
        /// Rework that to make it less required for CPU (add exclusion for checked before object in the secondloop)
        /// </summary>
        static public void UpdateMe()
        {
            for (int i = 0; i<physicObjects.Count; i++)
            {
                for(int j=0; j<physicObjects.Count; j++)
                {
                    if (i!=j)
                    {
                        if (physicObjects[i].GetRectangle().Intersects(physicObjects[j].GetRectangle()))
                        {
                            if (physicObjects[i].isPhysicActive && physicObjects[j].isPhysicActive)
                            { 
                            physicObjects[i].Collided(physicObjects[j]);
                            }
                        }
                    }
                }
                
            }
        }

        public static void AddObject(PhysicModule obj)
        {
            physicObjects.Add(obj);
        }

        public static void RemoveObject(PhysicModule obj)
        { 
            physicObjects.Remove(obj);
        }
    }
}
