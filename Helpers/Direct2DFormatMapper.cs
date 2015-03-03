using SharpDX.WIC;
using System;


public class Direct2DFormatMapper
{
    public static Guid ConvertImageFormat(Direct2DImageFormatType format)
    {
        switch (format)
        {
            case Direct2DImageFormatType.Bmp:
                return ContainerFormatGuids.Bmp;
            case Direct2DImageFormatType.Ico:
                return ContainerFormatGuids.Ico;
            case Direct2DImageFormatType.Gif:
                return ContainerFormatGuids.Gif;
            case Direct2DImageFormatType.Jpeg:
                return ContainerFormatGuids.Jpeg;
            case Direct2DImageFormatType.Png:
                return ContainerFormatGuids.Png;
            case Direct2DImageFormatType.Tiff:
                return ContainerFormatGuids.Tiff;
            case Direct2DImageFormatType.Wmp:
                return ContainerFormatGuids.Wmp;
        }
        throw new NotSupportedException();
    }
}