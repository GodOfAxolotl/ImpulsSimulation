using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace impulsSimulation
{
    public class Box : BaseNotificationClass
    {

        public double mass { get; set; }
        public double vel { get; set; }
        public double xPos { get; set; }
        public double initialX { get; set; }
        public double size;


        public Box(double mass, double vel, double xPos, double size)
        {
            this.mass = mass;
            this.vel = vel;
            this.xPos = xPos;
            this.size = size;
            this.initialX = xPos;
        }

        public void update(double friction)
        {
            xPos += vel;                                         //Geschwindigkeit wird auf die Position addiert 
            if (vel != 0)
            {
                if (vel > 0)
                {
                    vel -= friction / 2000;

                    if (vel < 0)
                    {
                        vel = 0;
                    }
                }
                else if (vel < 0)
                {
                    vel += friction / 1000;

                    if (vel > 0)
                    {
                        vel = 0;
                    }
                }                                   //Abzug der Reibung unter der Berücksichtigung der Kraft-Vektoren Richtung --- Box 1
            }
        }
    }
}
