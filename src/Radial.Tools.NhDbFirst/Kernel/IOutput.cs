using System.IO;

namespace Radial.Tools.NhDbFirst.Kernel
{
    interface IOutput
    {
        DataSource DataSource { get; set; }

        void WriteCode(TextWriter writer);

        void WriteXml(TextWriter writer);

    }
}
