using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calculator_eindopdracht
{
    public partial class RPNCalculator : Form
    {
        /// <summary>
        /// abstracte baseclass met class List, Array en MyList als overervingen
        /// </summary>
        /// 
        public abstract class Stack
        {
            public abstract Stack<double> makeStack();
        }

        /// <summary>
        /// List subclass maken. De methode "makestack" van de baseclass wordt overridden met als functie de nummers van de list in de stack te krijgen
        /// </summary>
        public class List : Stack
        {
            public Stack<double> listStack = new Stack<double>();
            public List<double> list = new List<double>();
            
            // een stack maken uit een list
            public override Stack<double> makeStack()
            {
                foreach (double num in list)
                {
                    listStack.Push(num);
                }
                return listStack;
            }
        }

        /// <summary>
        /// Array subclass maken. De methode "makestack" van de baseclass wordt overridden met als functie de nummers van de array in de stack te krijgen
        /// </summary>
        public class Array : Stack
        {
            Stack<double> arrayStack = new Stack<double>();
            public double[] array = new double[300];

            // een stack maken uit een array
            public override Stack<double> makeStack()
            {
                foreach (double num in array)
                {
                    arrayStack.Push(num);
                }
                return arrayStack;
            }
        }

        /// <summary>
        /// Mylist subclass maken. De methode "makestack" van de baseclass wordt overridden met als functie de nummers van de MyList in de stack te krijgen
        /// </summary>
        public class Mylist : Stack
        {
            public Stack<double> mylistStack = new Stack<double>();
            public double data;
            public Mylist next;
            public Mylist(double i)
            {
                data = i;
                next = null;
            }

            // de else statement gebruikt een recursie om weer te checken of de volgende nummer niet bestaat
            public void AddToEnd(double data)
            {
                if (next == null)
                {
                    next = new Mylist(data);
                }
                else
                {
                    next.AddToEnd(data);
                }
            }

            // stack maken uit een MyList
            public override Stack<double> makeStack()
            {
                mylistStack.Push(data);
                if (next != null)
                {
                    next.makeStack();
                }
                return mylistStack;
            }
        }

        //variabelen declareren 
        public Stack<double> liststack = new Stack<double>();
        public double right;    //"right is de laatste double van de stack
        public double left;     //"left is de laatste double van de stack
        public double result;   // de uitkomst van de berekening
        int itemcount = 0;          // om controleren dat er meer dan 2 nummers in de geheugen zit
        public bool ViewStack = true;    //boolean om de geheugen scherm te tonen
        

        public RPNCalculator()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Nummers in het display scherm te tonen
        /// </summary>
        private void Numbers_Only(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (txtDisplay.Text == "0")
                txtDisplay.Text = "";
            
                if (b.Text == ".")
                {
                    if (!txtDisplay.Text.Contains("."))
                        txtDisplay.Text = txtDisplay.Text + b.Text;
                }
                else
                {
                    txtDisplay.Text = txtDisplay.Text + b.Text;
                }
        }

        /// <summary>
        /// De nummer die op het display scherm toont, gaat naar de geheugen
        /// </summary>
        private void To_Stacklist_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text !="")
            {
                itemcount++;
                liststack.Push(Convert.ToDouble(txtDisplay.Text));
                double peek = liststack.Peek();
                numbersList.Items.Insert(0, peek);
                txtDisplay.Text = "";
            }
        }

        /// <summary>
        /// Verwijderen van de laatste gedrukte nummer in het display scherm
        /// </summary>
        private void Btn_backspace_Click(object sender, EventArgs e)
        {
            if(txtDisplay.Text.Length >0)
            {
                txtDisplay.Text = txtDisplay.Text.Remove(txtDisplay.Text.Length - 1, 1);
            }

            if (txtDisplay.Text =="")         //scherm blijft hetzelfde als er niets meer erin staat
            {
                txtDisplay.Text = "";
            }
        }

        /// <summary>
        /// Maken zodat u alleen nummers in het scherm kunt typen. Geen nummer dus 
        /// </summary>
        private void OnlyIntegers(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // alleen 1 komma mag in het scherm
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// De operator 'plus' en zijn functie komen hierin
        /// </summary>
        private void Plusbutton_Click(object sender, EventArgs e)
        {
            if (itemcount >= 2)
            {
                right = liststack.Pop();     //"right" is de laatste double in de stack
                left = liststack.Pop();      //"left" is de voorlaatste double in de stack
                result = right + left;       // uitkomst van de berekening
                liststack.Push(result);      // uitkomst gaat in de stack
                numbersList.Items.RemoveAt(0);
                numbersList.Items.Insert(0, result);
                numbersList.Items.RemoveAt(1);
                itemcount--;
            }
        }

        /// <summary>
        /// De operator 'min' en zijn functie komen hierin
        /// </summary>
        private void Minbutton_Click(object sender, EventArgs e)
        {
            if (itemcount >= 2)
            {
                right = liststack.Pop();
                left = liststack.Pop();
                result = right - left;
                liststack.Push(result);
                numbersList.Items.RemoveAt(0);
                numbersList.Items.Insert(0, result);
                numbersList.Items.RemoveAt(1);
                itemcount--;
            }
        }

        /// <summary>
        /// De operator 'vermenigvuldig' en zijn functie komen hierin
        /// </summary>
        private void Vermenigvuldigenbutton_Click(object sender, EventArgs e)
        {
            if (itemcount >= 2)
            {
                right = liststack.Pop();
                left = liststack.Pop();
                result = right * left;
                liststack.Push(result);
                numbersList.Items.RemoveAt(0);
                numbersList.Items.Insert(0, result);
                numbersList.Items.RemoveAt(1);
                itemcount--;
            }
        }

        /// <summary>
        /// De operator 'gedeeld door' en zijn functie komen hierin
        /// </summary>
        private void Gedeeldbutton_Click(object sender, EventArgs e)
        {
            if (itemcount >= 2)
            {
                right = liststack.Pop();
                left = liststack.Pop();
                result = right / left;
                liststack.Push(result);
                numbersList.Items.RemoveAt(0);
                numbersList.Items.Insert(0, result);
                numbersList.Items.RemoveAt(1);
                itemcount--;
            }
        }

        /// <summary>
        /// Negatieve getallen wordt juist getoond in de display scherm
        /// </summary>
        private void Negatiefbutton_Click(object sender, EventArgs e)
        {
            if(txtDisplay.Text != "")
            {
                txtDisplay.Text = "-" + txtDisplay.Text;
            }
            else
            {
                txtDisplay.Text = txtDisplay.Text + "-";
            }
        }

        /// <summary>
        /// Verbergt de geheugenscherm en de drie typen stacks
        /// </summary>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ViewStack == true)
            {
                numbersList.Visible = false;
                rbArray.Visible = false;
                rbList.Visible = false;
                rbMyList.Visible = false;
                ViewStack = false;
            }
            else
            {
                numbersList.Visible = true;
                rbArray.Visible = true;
                rbList.Visible = true;
                rbMyList.Visible = true;
                ViewStack = true;
            }
        }
    }
}
