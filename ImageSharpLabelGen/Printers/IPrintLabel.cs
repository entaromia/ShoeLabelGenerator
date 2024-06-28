namespace ImageSharpLabelGen.Printers
{
    public interface IPrintLabel
    {
        Task<bool> Print(string data);
    }
}