using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using Panel.BusinessLogic.SnipingToolLogic;
using Gma.System.MouseKeyHook;

namespace Panel.UserControls
{
    /// <summary>
    /// Interaction logic for SnipingTool.xaml
    /// </summary>
    public partial class SnipingTool : Window
    {
        private IKeyboardMouseEvents m_GlobalHook;
        public BitmapSource mTakenScreenShot;
        private System.Drawing.Point mStartPoint;
        private System.Drawing.Point mEndPoint;
        private System.Drawing.Rectangle mDrawRectangle;

        public SnipingTool()
        {
            InitializeComponent();
            InitMainWindow();

            //m_GlobalHook = Hook.GlobalEvents();
            //m_GlobalHook.KeyUp += M_GlobalHook_KeyUp;
            //m_GlobalHook.MouseDown += M_GlobalHook_MouseDown;
            //m_GlobalHook.MouseMove += M_GlobalHook_MouseMove;
            //m_GlobalHook.MouseUp += M_GlobalHook_MouseUp;

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Cross;

            mStartPoint = new System.Drawing.Point();
            mEndPoint = new System.Drawing.Point();
        }

        private void InitMainWindow()
        {
            this.WindowStyle = WindowStyle.None;
            this.Title = string.Empty;
            this.ShowInTaskbar = false;
            this.AllowsTransparency = true;
            this.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0x10, 0x10, 0x10, 0x10));
            //this.Topmost = true;
            this.Left = SystemParameters.VirtualScreenLeft;
            this.Top = SystemParameters.VirtualScreenTop;
            this.Width = SystemParameters.VirtualScreenWidth;
            this.Height = SystemParameters.VirtualScreenHeight;
        }

        private void M_GlobalHook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyValue == (int)Key.Escape)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                this.Close();
            }
        }

        //private void M_GlobalHook_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        //{
        //    //if (mDrawRectangle != null)
        //    //    this.Children.Remove(mDrawRectangle);

        //    mStartPoint = new System.Drawing.Point(e.X, e.Y);
        //    mDrawRectangle = new System.Windows.Shapes.Rectangle
        //    {
        //        Stroke = Brushes.Red,
        //        StrokeThickness = 0.5
        //    };
        //    Canvas.SetLeft(mDrawRectangle, mStartPoint.X);
        //    Canvas.SetTop(mDrawRectangle, mStartPoint.Y);
        //    this.cnDrawingArea.Children.Add(mDrawRectangle);
        //}

        //private void M_GlobalHook_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Released)
        //    {
        //        return;
        //    }

        //    Point tmpPoint = e.GetPosition(this.cnDrawingArea);

        //    int xPos = (int)Math.Min(tmpPoint.X, mStartPoint.X);
        //    int yPos = (int)Math.Min(tmpPoint.Y, mStartPoint.Y);

        //    int recWidth = (int)Math.Max(tmpPoint.X, mStartPoint.X) - xPos;
        //    int recHeight = (int)Math.Max(tmpPoint.Y, mStartPoint.Y) - yPos;

        //    mDrawRectangle.Width = recWidth;
        //    mDrawRectangle.Height = recHeight;
        //    Canvas.SetLeft(mDrawRectangle, xPos);
        //    Canvas.SetTop(mDrawRectangle, yPos);
        //}

    //    private void CaptureScreen(int X1, int Y1, int X2, int Y2)
    //    {
    //        int StartXPosition = 0;
    //        int StartYPosition = 0;
    //        int tmpWidth = 0;
    //        int tmpHeight = 0;

    //        if (X1 < X2 && Y1 < Y2)          /*Drawing Left to Right*/
    //        {
    //            StartXPosition = X1;
    //            StartYPosition = Y1;
    //            tmpWidth = X2 - X1;
    //            tmpHeight = Y2 - Y1;
    //        }
    //        else if (X1 > X2 && Y1 < Y2)     /*Drawing Top to Down*/
    //        {
    //            StartXPosition = X2;
    //            StartYPosition = Y1;
    //            tmpWidth = X1 - X2;
    //            tmpHeight = Y2 - Y1;
    //        }
    //        else if (X1 > X2 && Y1 > Y2)     /*Drawing Down to Top*/
    //        {
    //            StartXPosition = X2;
    //            StartYPosition = Y2;
    //            tmpWidth = X1 - X2;
    //            tmpHeight = Y1 - Y2;
    //        }
    //        else if (X1 < X2 && Y1 > Y2)      /*Drawing Right to Left */
    //        {
    //            StartXPosition = X1;
    //            StartYPosition = Y2;
    //            tmpWidth = X2 - X1;
    //            tmpHeight = Y1 - Y2;
    //        }
    //        StartXPosition += 2;
    //        StartYPosition += 2;
    //        tmpWidth -= 2;
    //        tmpHeight -= 2;
    //        mTakenScreenShot = ScreenCapture.CaptureRegion(StartXPosition, StartYPosition, tmpWidth, tmpHeight, false);
    //        Mouse.OverrideCursor = Cursors.Arrow;
    //    }

    //    private void M_GlobalHook_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
    //    {
    //        if (e.LeftButton == MouseButtonState.Released)
    //        {
    //            mEndPoint = e.GetPosition(this.cnDrawingArea);

    //            if (mDrawRectangle != null)
    //                this.cnDrawingArea.Children.Remove(mDrawRectangle);

    //            Point StartDesktopPosition = this.PointToScreen(mStartPoint);
    //            Point EndDesktopPosition = this.PointToScreen(mEndPoint);

    //            int tempX1 = (int)StartDesktopPosition.X;
    //            int tempY1 = (int)StartDesktopPosition.Y;
    //            int tempX2 = (int)EndDesktopPosition.X;
    //            int tempY2 = (int)EndDesktopPosition.Y;

    //            CaptureScreen(tempX1, tempY1, tempX2, tempY2);
    //            this.DialogResult = true;
    //            this.Close();
    //        }
    //    }
    //}


    //private void SnipingTool_Loaded(object sender, RoutedEventArgs e)
    //    {
    //        this.Hide();
    //        System.Drawing.Rectangle DeskTop = SnipingToolHelperMethods.GetDestopScreens();
    //        Bitmap printscreen = new Bitmap(DeskTop.Width, DeskTop.Height);
    //        Graphics graphics = Graphics.FromImage(printscreen as System.Drawing.Image);
    //        graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
    //        using (MemoryStream s = new MemoryStream())
    //        {
    //            printscreen.Save(s, ImageFormat.Bmp);
    //            Border border = new Border();
                
    //            //imgImageBox.Size = new System.Drawing.Size(this.Width, this.Height);
    //            //imgImageBox.Image = System.Drawing.Image.FromStream(s);
    //        }
    //        this.Show();
    //        Cursor = System.Windows.Input.Cursors.Cross;
    //    }
    }
}
