namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Print view configuration options.
    /// </summary>
    /// <remarks>
    /// This service assumes that wkhtmltopdf (http://wkhtmltopdf.org/) utility is installed on the machine. Tested compatible version - 0.12.2.4.
    /// </remarks>
    public class PrintViewOptions : IOptions
    {
        /// <inheritdoc />
        public string SectionName => "printView";


        /// <summary>
        /// Default instance of <see cref="PrintViewOptions" />.
        /// </summary>
        public static readonly PrintViewOptions Default = new PrintViewOptions();


        /// <summary>
        /// Initializes a new instance of <see cref="PrintViewOptions" />.
        /// </summary>
        public PrintViewOptions()
        {
        }


        /// <summary>
        /// Temporary files directory.
        /// </summary>
        /// <remarks>
        /// By default - user profile temp files directory.
        /// </remarks>
        public string TempDirectory { get; set; }

        /// <summary>
        /// Location of wkhtmltopdf utility.
        /// </summary>
        /// <remarks>
        /// By default - auto generated dependent on operation system.
        /// </remarks>
        public string WkHtmlToPdfPath { get; set; }

        /// <summary>
        /// Command line options for wkhtmltopdf utility.
        /// </summary>
        /// <remarks>
        /// By default - auto generated dependent on operation system.
        /// </remarks>
        public string WkHtmlToPdfArguments { get; set; }
    }
}