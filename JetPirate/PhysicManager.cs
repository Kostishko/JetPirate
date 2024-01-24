using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetPirate
{
    /// <summary>
    /// this is a manager of all collision modules. It sent data about collided objects to collided modules
    /// </summary>
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
                            physicObjects[i].Collided(physicObjects[j]); // send the data about two collisions between two objects
                            }
                        }
                    }
                }
                
            }
        }

        /// <summary>
        /// When new physic object are creating it should be added to common list
        /// </summary>
        /// <param name="obj"></param>
        public static void AddObject(PhysicModule obj)
        {
            physicObjects.Add(obj);
        }

        /// <summary>
        /// In case if smth should be deleted from the list (don't use it case all object are reusable
        /// </summary>
        /// <param name="obj"></param>
        public static void RemoveObject(PhysicModule obj)
        { 
            physicObjects.Remove(obj);
        }
    }
}
