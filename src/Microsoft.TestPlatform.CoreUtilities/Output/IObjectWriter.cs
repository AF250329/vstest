namespace Microsoft.VisualStudio.TestPlatform.Utilities
{
    public interface IObjectWriter : IOutput
    {
        /// <summary>
        /// Function will send actual object
        /// </summary>
        /// <param name="obj">The actual object.</param>
        void SendObject(object obj);
    }
}
