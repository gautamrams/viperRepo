using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using LocalUserManagement;
namespace CheckComboBox
{

   
    public class CheckComboBoxItem
    {
        

        public CheckComboBoxItem( string text, bool initialCheckState )
        {
            _checkState = initialCheckState;
            _text = text;
        }

        private bool _checkState = false;
        /// <summary>
        /// Gets the check value (true=checked)
        /// </summary>
        public bool CheckState
        {
            get { return _checkState; }
            set { _checkState = value; }
        }
        
        public static bool is_checkcomboitem = true ;

        private string _text = "";
        /// <summary>
        /// Gets the label of the check box
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private object _tag = null;
        /// <summary>
        /// User defined data
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        /// <summary>
        /// This is used to keep the edit control portion of the combo box consistent
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "";
        }

    }

    /// <summary>
    /// Inherits from ComboBox and handles DrawItem and SelectedIndexChanged events to create an
    /// owner drawn combo box drop-down.  The contents of the dropdown are rendered using the
    /// CheckBoxRenderer class.
    /// </summary>
    public partial class CheckComboBox : ComboBox
    {
        /// <summary>
        /// C'tor
        /// </summary>
        //
        public CheckComboBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DrawItem += new DrawItemEventHandler(CheckComboBox_DrawItem);
            this.SelectedIndexChanged += new EventHandler( CheckComboBox_SelectedIndexChanged );
            SelectedText = "Select Options";
                       
        }
        [Category("Appearance")]
        [Description("The border color of the drop down list")]
        [DefaultValue(typeof(Color), "LightGray")]
        public Color DropDownBorderColor { get; set; }
        [Category("Appearance")]
        [Description("The background color of the drop down list")]
        [DefaultValue(typeof(Color), "White")]
        public Color DropDownBackColor { get; set; }
        /// <summary>
        /// Invoked when the selected index is changed on the dropdown.  This sets the check state
        /// of the CheckComboBoxItem and fires the public event CheckStateChanged using the 
        /// CheckComboBoxItem as the event sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CheckComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            try
            {
                CheckComboBoxItem item = (CheckComboBoxItem)SelectedItem;
                item.CheckState = !item.CheckState;
                if (CheckStateChanged != null)
                    CheckStateChanged(item, e);
            }
            catch (Exception ee) {
                AddMultipleComputers addcomputers = new AddMultipleComputers();
                
                addcomputers.ShowDialog();
            }
             
        }

        /// <summary>
        /// Invoked when the ComboBox has to render the drop-down items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CheckComboBox_DrawItem( object sender, DrawItemEventArgs e )
        {
			// make sure the index is valid (sanity check)
			if( e.Index == -1 )
			{
				return;
			}
            if (
             ((e.State & DrawItemState.Focus) == DrawItemState.Focus) ||
             ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ||
             ((e.State & DrawItemState.HotLight) == DrawItemState.HotLight)
            )
            {
                //e.DrawBackground();
                e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            }
            else
            {
               // Brush backgroundBrush = new SolidBrush(Brushes.White);
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            }

            e.Graphics.DrawString(Items[e.Index].ToString(), this.Font, Brushes.Black,
              new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                e.DrawFocusRectangle();

            Pen borderPen = new Pen(Color.LightGray, 1);
            Point start;
            Point end;

            if (e.Index == 0)
            {
                //Draw top border
                start = new Point(e.Bounds.Left, e.Bounds.Top);
                end = new Point(e.Bounds.Left + e.Bounds.Width - 1, e.Bounds.Top);
                e.Graphics.DrawLine(borderPen, start, end);
            }

            //Draw left border
            start = new Point(e.Bounds.Left, e.Bounds.Top);
            end = new Point(e.Bounds.Left, e.Bounds.Top + e.Bounds.Height - 1);
            e.Graphics.DrawLine(borderPen, start, end);

            //Draw Right border
            start = new Point(e.Bounds.Left + e.Bounds.Width - 1, e.Bounds.Top);
            end = new Point(e.Bounds.Left + e.Bounds.Width - 1, e.Bounds.Top + e.Bounds.Height - 1);
            e.Graphics.DrawLine(borderPen, start, end);

            if (e.Index == Items.Count - 1)
            {
                //Draw bottom border
                start = new Point(e.Bounds.Left, e.Bounds.Top + e.Bounds.Height - 1);
                end = new Point(e.Bounds.Left + e.Bounds.Width - 1, e.Bounds.Top + e.Bounds.Height - 1);
                e.Graphics.DrawLine(borderPen, start, end);
            }
			// test the item to see if its a CheckComboBoxItem
			if( !( Items[ e.Index ] is CheckComboBoxItem ) )
			{
				// it's not, so just render it as a default string
				//e.Graphics.DrawString(
				//	Items[ e.Index ].ToString(),
				//	this.Font,
				 //   Brushes.Black,
				//	new Point( e.Bounds.X, e.Bounds.Y ),
                //    new StringFormat(StringFormatFlags.DirectionRightToLeft)
                //    );
                TextRenderer.DrawText(e.Graphics, Items[e.Index].ToString(), this.Font,
                new Point(e.Bounds.X + 300, e.Bounds.Y),
                SystemColors.ControlText);
		        return;
			}

			// get the CheckComboBoxItem from the collection
          
			 CheckComboBoxItem box = (CheckComboBoxItem)Items[ e.Index ];
            
			// render it
			//CheckBoxRenderer.RenderMatchingApplicationState = true;
		//	CheckBoxRenderer.DrawCheckBox(
		//		e.Graphics,
		//		new Point( e.Bounds.X+5, e.Bounds.Y+5 ),
		//		e.Bounds,
		//		box.Text,
		//		this.Font,
		//		( e.State & DrawItemState.Focus ) == 0,
		//		box.CheckState ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal );
                CheckBoxRenderer.DrawCheckBox(e.Graphics,new Point(e.Bounds.X + 5, e.Bounds.Y+5),box.CheckState ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
                TextRenderer.DrawText(e.Graphics, box.Text, this.Font,new Point(e.Bounds.X + 20, e.Bounds.Y+3),SystemColors.ControlText);
                start = new Point(e.Bounds.Left, e.Bounds.Top + e.Bounds.Height - 1);
                end = new Point(e.Bounds.Left + e.Bounds.Width - 1, e.Bounds.Top + e.Bounds.Height - 1);
                e.Graphics.DrawLine(borderPen, start, end);
		
        }

        /// <summary>
        /// Fired when the user clicks a check box item in the drop-down list
        /// </summary>
        public event EventHandler CheckStateChanged;

    }
}


