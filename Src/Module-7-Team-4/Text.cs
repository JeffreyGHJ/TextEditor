using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Module_7_Team_4
{
    [Serializable]
    public class Text : INotifyPropertyChanged
    {
        private String storedText;
        private int zOrder;
        private Color textColor;
        private Color backgroundColor;
        private Point location;
        private Font textFont;
        private float textRotation;
        private RectangleF textBounds;
        [NonSerialized]private Region hitTestRegion;
        private bool locationSetByUser = false; // Set to true if the text object gets moved by the mouse
        private bool textChanged = false; 

        //Implements the property changed event handler.
        public event PropertyChangedEventHandler PropertyChanged;
        //Helper function to update the property when it changes/provides binding.
        void OnPropertyChanged(string propertyName)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //Generated constructor that requires evertything..will make another default constructor that defaults some stuff..
        public Text(string storedText, int zOrder, Color textColor, Color backgroundColor, Point location, Font textFont, float textRotation)
        {
            StoredText = storedText;
            //ZOrder = zOrder;
            TextColor = textColor;
            BackgroundColor = backgroundColor;
            Location = location;
            TextFont = textFont;
            TextRotation = textRotation;
        }

        //Default constructor only needs string & graphics a object passed from the MainForm
        public Text(string storedText, Graphics g)
        {
            Location = new Point(0, 0);
            StoredText = storedText;
            //ZOrder = 0;
            TextColor = Color.Black;
            BackgroundColor = Color.Transparent;
            TextFont = new Font("Consolas", 11);
            TextRotation = 0.0f;

            //Calculate a rectangle which represents the bounds of the text area
            TextBounds = CalculateTextBounds(g);
        }

        //Add encapsulation of property with the notify property changed value for binding later with dialog. 
        public String StoredText
        {
            get
            {
                return this.storedText;
            }
            set
            {
                if (value != this.storedText)
                {
                    this.storedText = value;
                    this.TextChanged = true;
                    this.OnPropertyChanged("StoredText");
                }
            }
        }
        public int ZOrder
        {
            get
            {
                return this.zOrder;
            }
            set
            {
                if (value != this.zOrder)
                {
                    this.zOrder = value;
                    this.OnPropertyChanged("ZOrder");
                }
            }
        }

        public Color TextColor
        {
            get
            {
                return this.textColor;
            }
            set
            {
                if (value != this.textColor)
                {
                    this.textColor = value;
                    this.OnPropertyChanged("TextColor");
                }
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                if (value != this.backgroundColor)
                {
                    this.backgroundColor = value;
                    this.OnPropertyChanged("BackgroundColor");
                }
            }
        }

        public Point Location
        {
            get
            {
                return this.location;
            }
            set
            {
                if (value != this.location)
                {
                    this.location = value;
                    this.OnPropertyChanged("Location");
                    UpdateBounds();
                }
            }
        }

        public Font TextFont
        {
            get
            {
                return this.textFont;
            }
            set
            {
                if (value != this.textFont)
                {
                    this.textFont = value;
                    this.textChanged = true;
                    this.OnPropertyChanged("TextFont");
                }
            }
        }

        public float TextRotation
        {
            get
            {
                return this.textRotation;
            }
            set
            {
                if (value != this.textRotation)
                {
                    this.textRotation = value;
                    this.OnPropertyChanged("TextRotation");
                }
            }
        }

        public RectangleF TextBounds
        {
            get { return this.textBounds; }
            set
            {
                if (value != this.textBounds)
                {
                    this.textBounds = value;
                    this.OnPropertyChanged("TextBounds");
                }
            }
        }

        public bool LocationSetByUser
        {
            get { return this.locationSetByUser; }
            set { this.locationSetByUser = value; }
        }

        public PointF CenterPoint
        {
            get {return new PointF(
                  (this.textBounds.Left + (this.TextBounds.Width / 2)),
                  (this.TextBounds.Top + (this.TextBounds.Height / 2)));
                }
        }

        public Region HitTestRegion
        {
            get { return this.hitTestRegion; }
            set { this.hitTestRegion = value; }
        }

        public bool TextChanged
        { 
            get { return this.textChanged; }
            set { this.textChanged = value; }
        }

        //Helper function to update rotation and redraw on the screen
        public void UpdateTextRotation(float newRotation)
        {

        }

        //helper function that will create the graphic type to draw the text after we have everything we need filled out.
        public void DrawText()
        {

        }

        private RectangleF CalculateTextBounds(Graphics g)
        {
            RectangleF bounds = new RectangleF(this.Location, g.MeasureString(this.StoredText, this.TextFont));
            return bounds;
        }

        public void SetNewBounds(Graphics g)
        {
            this.TextBounds = CalculateTextBounds(g);
            UpdateHitTestRegion();
        }

        public void UpdateBounds()
        {
            this.TextBounds = new RectangleF(this.Location, this.TextBounds.Size);
            UpdateHitTestRegion();
        }

        public void UpdateHitTestRegion()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(this.TextBounds);
            Region region = new Region(path);

            Matrix matrix = new Matrix();
            matrix.RotateAt(this.TextRotation, this.CenterPoint);
            region.Transform(matrix);

            this.HitTestRegion = region;
        }
    }
}
