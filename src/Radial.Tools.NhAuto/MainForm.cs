using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Radial.Tools.NhAuto.Kernel;

namespace Radial.Tools.NhAuto
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Profile profile = new Profile
            {
                DataSource = DataSource.SqlServer,
                ConnectionString = "Server=swin.cloudapp.net,4336;Database=rdut;User Id=rduta;Password=rduta;"
            };

            Generator gen=new Generator(profile);

            IList<TableDefinition> tableDefs = TableDefinition.Retrieve(profile);
            foreach (TableDefinition tableDef in tableDefs)
            {
                IList<FieldDefinition> fieldDefs = FieldDefinition.Generate(profile, tableDef);

                gen.Process(tableDef, fieldDefs);
            }
        }
    }
}
