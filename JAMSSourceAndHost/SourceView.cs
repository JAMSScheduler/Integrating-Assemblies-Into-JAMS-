using System;
using System.Windows.Forms;
using MVPSI.JAMS;
using MVPSI.JAMSExtension;

namespace JAMSSourceAndHost
{
    public partial class SourceView : UserControl, IEditJobSource
    {
        private Source m_source;
        public SourceView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Returns the control used to edit the Job source
        /// </summary>
        public Control EditorControl
        {
            get
            {
                return this;
            }
        }
        /// <summary>
        /// The JAMS Job being edited
        /// </summary>
        public Job Job
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the Job source. This property is called by JAMS when loading and saving the Job source
        /// </summary>
        public string SourceCode
        {
            get
            {
                if (m_source == null)
                {
                    return String.Empty;
                }
                else
                {
                    m_source.MethodName = comboMethod.Text;
                }
                return m_source.GetSource();
            }
            set
            {
                try
                {
                    //
                    // Get the Source object from the JSON Source
                    //
                    m_source = Source.Create(value);
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Unable to load source", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    m_source = Source.Create(String.Empty);
                }
                //
                // Set the control values from the Source
                //
                //comboMethod.Text = m_source.MethodName;
                comboMethod.Items.Add("CreateFileA");
                comboMethod.Items.Add("CreateFileB");

                //Set our Source Value to the text value in the combo box
                comboMethod.Text = m_source.MethodName;
            }
        }

        /// <summary>
        /// Marks the Job source as modified so it will be saved
        /// </summary>
        /// <param name="e"></param>
        private void OnSourceCodeChanged(EventArgs e)
        {
            if (SourceCodeChanged != null)
            {
                SourceCodeChanged(this, e);
            }
        }

        public event EventHandler SourceCodeChanged;        
        ///// <summary>
        ///// Mark the Job source as modified after the FileName changes
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        private void comboMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSourceCodeChanged(e);
        }
        public void SaveState() { }
    }
}
