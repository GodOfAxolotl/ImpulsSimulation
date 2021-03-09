using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impulsSimulation
{
    public class Session : BaseNotificationClass
    {

        public int collision { get; set; }
        public double friction { get; set; }
        public Box box1, box2;
        public double size1 = 100;
        public double size2 = 100;

        private double _vB1;
        private double _vB2;

        public double vB1
        {
            get {return _vB1;  }
            set { _vB1 = value; OnPropertyChanged(nameof(vB1)); }
        }
        public double vB2
        {
            get { return _vB2; }
            set { _vB2 = value; OnPropertyChanged(nameof(vB2)); }
        }

        int screenRimBuffer = 16;

        public Session(double mb1, double mb2, double vb1, double vb2, double xp1, double xp2)
        {
            box1 = new Box(mb1, vb2, xp1, size1);
            box2 = new Box(mb2, vb2, xp2, size2);

            collision = -1;
            friction = 0;

            notifyVelocityUpdate();
            collidie();
        }

        public void start()
        {
            notifyVelocityUpdate();
        }


        public void reset()
        {
            box1.xPos = box1.initialX;
            box2.xPos = box2.initialX;
            box1.vel = 0;
            box2.vel = 0;
            collision = -1;
            collidie();
        }


        public void loop()
        {
            box1.update(friction);
            box2.update(friction);
            vB1 = box1.vel;
            vB2 = box2.vel;

                                                                                           //Abzug der Reibung unter der Berücksichtigung der Kraft-Vektoren Richtung --- Box 2

            if (box1.xPos + box1.size > box2.xPos || box2.xPos < box1.xPos + box1.size)
            {
                if (box1.mass >= box2.mass)
                {
                    box2.xPos = box1.xPos + box1.size;
                    //box2.xPos -= box2.vel;
                }
                else
                {
                    box1.xPos = box2.xPos - box2.size;
                    //box1.xPos -= box1.vel;
                }
                                                                                         //Richtige Positionierung, der Massereichere Körper schiebt den anderen
                double newVell1 = newVel1(box1.mass, box2.mass, box1.vel, box2.vel);
                double newVell2 = newVel2(box1.mass, box2.mass, box1.vel, box2.vel); 
                                                                                         //Berrechnung der Physik 

                box1.vel = newVell1;
                box2.vel = newVell2;                                                     //Synchronisierung der Geschwindigkeiten

                collidie();
            }
            //Kollision der Boxen 

            if (!Globals.accurateMaths)
            {
                if (box1.xPos < 0 && box2.xPos < box2.size && Globals.leftwall)
                {
                    box2.xPos = 100;
                    box1.xPos = 0;
                    box2.vel = -box2.vel;
                    collidie();
                }
                else if (box1.xPos < 0 && Globals.leftwall)
                {
                    box1.xPos = 0;
                    box1.vel = -box1.vel;
                    collidie();
                }
            } 
            else 
            {
                if (box1.xPos < 0 && Globals.leftwall)
                {
                    box1.xPos = 0;
                    box1.vel = -box1.vel;
                    collidie();
                }
            }

            //Kollision an der Linken Wand. 
            //Wenn beide Boxen an der Linken wand stehen, stehen sie nebeneinander

            if (!Globals.accurateMaths)
            {

                if (box2.xPos > Globals.windowWidth - box2.size - screenRimBuffer && box1.xPos > Globals.windowWidth - box1.size - box2.size - screenRimBuffer && Globals.rightwall)
                {
                    box1.xPos = Globals.windowWidth - box1.size - box2.size - screenRimBuffer;
                    box1.xPos = Globals.windowWidth - box2.size;
                    box2.vel = -box2.vel;
                    collidie();
                }
                else if (box2.xPos > Globals.windowWidth - box2.size - screenRimBuffer && Globals.rightwall)
                {
                    box2.xPos = Globals.windowWidth - box2.size - screenRimBuffer;
                    box2.vel = -box2.vel;
                    collidie();
                }
            } 
            else if (box2.xPos > Globals.windowWidth - box2.size - screenRimBuffer && Globals.rightwall)
            {
                box2.xPos = Globals.windowWidth - box2.size - screenRimBuffer;
                box2.vel = -box2.vel;
                collidie();
            }


            //Kollision an der Rechten Wand. 
            //Wenn beide Boxen an der Rechten wand stehen, stehen sie nebeneinander
        }


        private double newVel1(double mass1, double mass2, double vel1, double vel2)
        {
            return ((mass1 - mass2) / (mass1 + mass2)) * vel1 + ((2 * mass2) / (mass1 + mass2)) * vel2; //Kollsion für Box1
        }


        private double newVel2(double mass1, double mass2, double vel1, double vel2)
        {
            return ((2 * mass1) / (mass1 + mass2)) * vel1 + ((mass2 - mass1) / (mass1 + mass2)) * vel2; //Kollision für Box2
        }


        private void collidie()
        {
            collision++;                                //Zählvariable tickt hoch
            OnPropertyChanged(nameof(collision));       //Notification!!!
            notifyVelocityUpdate();
        }

        private void notifyVelocityUpdate()
        {
            vB1 = Math.Round(box1.vel, 3);
            vB2 = Math.Round(box2.vel, 3);           // Auf 3 Nachkommastellen gerunden 
            OnPropertyChanged(nameof(vB1));
            OnPropertyChanged(nameof(vB2));         //Notfication!!!
        }
    }
}
