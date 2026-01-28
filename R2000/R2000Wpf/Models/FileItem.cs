namespace R2000Wpf.Models;

public class FileItem
{
    public required string Name { get; set; }
    public FileType Type { get; set; }
}
public enum FileType
{
    Txt,
    Word,
    Pdf,
    Folder
}